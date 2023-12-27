using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance {get {return _instance;}}
    [SerializeField] Tilemap floor;
    [SerializeField] OverlayTile overlayTilePrefab;
    [SerializeField] GameObject overlayContainer;
    public OverlayTile startLocation;

    public Dictionary<Vector2Int, OverlayTile> map;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();
        BoundsInt bounds = floor.cellBounds;
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                Vector2Int tileKey = new Vector2Int(x,y);
                if(floor.HasTile(tileLocation))
                {
                    var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                    var cellWorldPosition = floor.GetCellCenterWorld(tileLocation);
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z+1);
                    overlayTile.Position = tileLocation;
                    overlayTile.HideTile();
                    map.Add(tileKey, overlayTile);
                }
            }
        }
    }
}
