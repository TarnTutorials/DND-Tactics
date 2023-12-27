using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    public void Start()
    {
        SetAbilities();
    }

    //Ability needs (range, width, length, numOfDice, sizeOfDice, levelRequired, Line targeting (true/false))
    private void SetAbilities()
    {
        Ability Fireball = new Ability(5, 2, 2, 1, 6, 1, false);
        Abilities.Add(Fireball);
    }
    //Ability needs (range, width, length, numOfDice, sizeOfDice, levelRequired, Line targeting (true/false))

}
