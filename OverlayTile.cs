using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;

public class OverlayTile : MonoBehaviour
{
    public int G {get; set;}
    public int H {get; set;}
    public int F {get {return G + H;}}
    public OverlayTile PreviousTile {get; set;}
    public Vector3Int Position {get; set;}
    public Vector2Int Position2D {get {return new Vector2Int(Position.x, Position.y);}}
    public List<Sprite> arrows;
    public bool isBlocked, isSelected;
    public Character hasCharacter;

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        gameObject.GetComponent<SpriteRenderer>().sprite = arrows[0];
    }

    public void SetArrowSprite(ArrowDirection d)
    {
        this.GetComponent<SpriteRenderer>().sprite = arrows[(int)d];
    }
}
