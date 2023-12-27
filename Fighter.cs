using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Character
{

    public void Start()
    {
        SetAbilities();
    }

    //Ability needs (range, width, length, numOfDice, sizeOfDice, levelRequired, Line targeting (true/false))
    private void SetAbilities()
    {
        Ability MeleeAttack = new Ability(5, 1, 1, 1, 6, 1, true);
        Abilities.Add(MeleeAttack);
    }
}
