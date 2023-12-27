using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public int Width {get; set;}
    public int Length {get; set;}
    public int Range {get; set;}
    public int NumOfDice {get; set;}
    public int DamageDice {get; set;}
    public int LevelRequired {get; set;}
    public bool Straight {get; set;}

    public Ability(int range, int width, int length, int numOfDice, int damageDice, int levelRequired, bool straight)
    {
        Range = range;
        Width = width;
        Length = length;
        NumOfDice = numOfDice;
        DamageDice = damageDice;
        LevelRequired = levelRequired;
        Straight = straight;
    }

    public List<OverlayTile> ShowRange(OverlayTile target)
    {
        List<OverlayTile> inRange = new List<OverlayTile>();
        inRange.Add(target);
        for(int y = 1-Width; y < Width; y++)
        {
            for(int x = 1-Length; x < Length; x++)
            {
                Vector2Int locationToCheck = new Vector2Int(target.Position.x + x,target.Position.y + y);
                var map = MapManager.Instance.map;
                if(map.ContainsKey(locationToCheck))
                {
                    inRange.Add(map[locationToCheck]);
                }
            }
        }
        return inRange;
    }

    public int RollDamage(int bonus)
    {
        int totalDamage = bonus;
        List<int> diceRolls = new List<int>();
        for(int x = 0; x < NumOfDice; x++)
        {
            diceRolls[x] = Random.Range(1, DamageDice);
            Debug.Log(diceRolls[x]);
            totalDamage = diceRolls[x];
            Debug.Log(totalDamage);
        }
        return totalDamage;
    }
}
