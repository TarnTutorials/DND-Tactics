using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MouseController : MonoBehaviour
{
    public static MouseController Instance {get; private set;}
    [SerializeField] GameObject[] characterPrefab;
    public Character CurrentCharacter {get {return character;} set{ character = value;}}
    public Ability SelectedAbility {get {return selectedAbility;} set{ selectedAbility = value;}}
    private Character character;
    private Ability selectedAbility;
    private PathFinder pathFinder;
    private ArrowTranslator arrowTranslator;
    public bool placeCharacters = true;
    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> inAbilityRange = new List<OverlayTile>();
    private RangeFinder rangeFinder;
    private bool isMoving, usingAbility = false;
    private int numCharacters;
    public bool useAbility = false;
    public bool useMovement = false;
    [SerializeField] GameObject selectedIcon;
    
    public void Awake() 
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        arrowTranslator = new ArrowTranslator();
        numCharacters = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!placeCharacters)
        {
            selectedIcon.transform.position = character.gameObject.transform.position;
            MoveCharacter();
        }
        var focusedTileHit = GetFocusedTile();
        if(focusedTileHit.HasValue)
        {
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            if(placeCharacters)
            {
                PlaceCharacters(overlayTile);
            }
            else if(useMovement)
            {
                ShowMovement(overlayTile);
                if(Input.GetMouseButtonDown(0))
                {
                    isMoving = true;
                }
            }
            else if(useAbility)
            {
                if(selectedAbility != null)
                {
                    ShowAbility(overlayTile);
                    if(Input.GetMouseButtonDown(0))
                    {
                        usingAbility = true;
                        //Add in for each highlighted enemy AC for enemy
                        if(character.RollD20(false, false, 0) > 0) //need to calculate advantages
                        {

                        }

                    }
                }
            }
            else
            {
                foreach(var tile in inRangeTiles)
                {
                    tile.HideTile();
                } 
            }
        }
    }

    private void PlaceCharacters(OverlayTile overlayTile)
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(numCharacters < characterPrefab.Length)
            {
                SpawnCharacter(overlayTile, characterPrefab[numCharacters], numCharacters);
                numCharacters++;
            }
            else
            {
                placeCharacters = false;
                GameManager.Instance.charactersSpawned = true;
            }
        }
    }

    private void ShowMovement(OverlayTile overlayTile)
    {
        if(!isMoving)
        {
            GetInMovementRange(character.CurrentTile, character.CurrentMovePoints);
            if(inRangeTiles.Contains(overlayTile))
            {
                path = pathFinder.FindPath(character.CurrentTile, overlayTile, inRangeTiles);
                path.Reverse();
                foreach(var tile in inRangeTiles)
                {
                    tile.SetArrowSprite(0);
                }
                for(int i=0; i< path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i-1] : character.CurrentTile;
                    var futureTile = i < path.Count - 1 ? path[i+1] : null;
                    var arrowDirection = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].SetArrowSprite(arrowDirection);
                }    
            }
        } 
    }

    private void GetInMovementRange(OverlayTile start, int range)
    {
        foreach(var tile in inRangeTiles)
        {
            tile.HideTile();
        }
        inRangeTiles = rangeFinder.GetTilesInRange(start, range, pathFinder);
        foreach(var tile in inRangeTiles)
        {
            tile.ShowTile();
        }
    }

    private void ShowAbility(OverlayTile overlayTile)
    {
        foreach(var tile in inAbilityRange)
        {
            tile.HideTile();
            tile.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
            tile.isSelected = false;
        }
        GetInAbilityRange(character.CurrentTile, selectedAbility.Range, selectedAbility.Straight);
        
        if(inRangeTiles.Contains(overlayTile))
        {        
            inAbilityRange = selectedAbility.ShowRange(overlayTile);
            foreach(var tile in inAbilityRange)
            {
                tile.ShowTile();
                tile.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
                if(tile.hasCharacter != null)
                {
                    tile.isSelected = true;
                }
            }
        }
    }

    private void GetInAbilityRange(OverlayTile start, int range, bool straight)
    {
        foreach(var tile in inRangeTiles)
        {
            tile.HideTile();
            tile.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        }
        if(straight)
        {
            inRangeTiles = rangeFinder.GetTilesInRangeStraight(start, range);
        }
        else
        {
            inRangeTiles = rangeFinder.GetTilesInRange(start, range, pathFinder);
        }
        foreach(var tile in inRangeTiles)
        {
            tile.ShowTile();
            tile.GetComponent<SpriteRenderer>().color = new Color(1,1,0,1);
        }
    }

    private void MoveCharacter()
    {
        if(isMoving)
        {
            if(path.Count > 0)
            {
                var step = 3 * Time.deltaTime;
                character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
                if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.001f)
                {
                    PositionCharacter(path[0]);
                    path.RemoveAt(0);
                    character.CurrentMovePoints--;
                }
            }
            if(path.Count == 0)
            {
                isMoving = false;
                if(character.CurrentMovePoints == 0)
                {
                     character.hasMovement = false;
                }
            }
        } 
    }
    
    public RaycastHit2D? GetFocusedTile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition2D, Vector2.zero);
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    private void SpawnCharacter(OverlayTile tile, GameObject charPrefab, int x)
    {
        GameObject characterTemp = Instantiate(charPrefab);
        character = characterTemp.GetComponent<Character>();
        GameManager.Instance.Characters.Add(character);
        PositionCharacter(tile);
        character.hasMovement = true;
    }

    private void PositionCharacter(OverlayTile tile)
    { 
        character.PreviousTile = character.CurrentTile;
        if(character.PreviousTile != null)
        {
            character.PreviousTile.isBlocked = false;
            character.PreviousTile.hasCharacter = null;
        }
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.CurrentTile = tile;
        character.CurrentTile.isBlocked = true;
        tile.hasCharacter = character;
        tile.SetArrowSprite(0);
    }

    public void ResetTurn()
    {
        selectedAbility = null;
        useAbility = false;
        useMovement = false;
    }
}
