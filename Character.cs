using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public OverlayTile CurrentTile {get; set;}
    public OverlayTile PreviousTile {get; set;}
    public int MovePoints {get {return movePoints;} set{ movePoints = MovePoints;}}
    [SerializeField] int movePoints;
    public int CurrentMovePoints {get; set;}
    public bool hasMovement;
    public int Initiative {get; set;}
    public int HealthPoints {get; set;}
    public int ArmorClass {get; set;}
    public Stats Intelligence, Strength, Wisdom, Charisma, Constitution, Dexterity;
    public List<Ability> Abilities = new List<Ability>();
    // Start is called before the first frame update

    public void Update()
    {
        if(CurrentTile.isSelected)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        }
    }

    public int RollD20(bool advantage, bool disadvantage, int bonus)
    {
        int rollValue = bonus;
        if(advantage && !disadvantage)
        {
            int rollA = Random.Range(1,20);
            int rollB = Random.Range(1,20);
            if(rollA >= rollB)
            {
                rollValue += rollA;
            }
            else
            {
                rollValue += rollB;
            }
        }
        else if(disadvantage && !advantage)
        {
            int rollA = Random.Range(1,20);
            int rollB = Random.Range(1,20);
            if(rollA <= rollB)
            {
                rollValue += rollA;
            }
            else
            {
                rollValue += rollB;
            }
        }
        else
        {
            rollValue += Random.Range(1,20);
        }
        return rollValue;
    }

    public void RollInitiative()
    {
        Initiative = RollD20(false, false, Dexterity.Modifier);
    }

    public void ResetTurn()
    {
        CurrentMovePoints = MovePoints;
        hasMovement = true;
    }
}
