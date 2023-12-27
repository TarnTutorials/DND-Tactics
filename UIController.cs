using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance {get; private set;}
    private MouseController controller;
    private Character character;

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

    public void Start()
    {
        controller = MouseController.Instance;
    }

    public void SetCharacterMovement()
    {
        if(!controller.placeCharacters)
        {
            controller.useMovement = true;
            controller.useAbility = false;
        }
    }

    public void SetCharacterAbility()
    {
        if(!controller.placeCharacters)
        {
            controller.useMovement = false;
            controller.useAbility = true;
            character = controller.CurrentCharacter;
            MouseController.Instance.SelectedAbility = character.Abilities[0];
        }  
    }
}
