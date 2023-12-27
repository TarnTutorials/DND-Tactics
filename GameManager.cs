using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public List<Character> Characters = new List<Character>();
    public List<Character> Enemies = new List<Character>();
    private List<Character> InitiativeOrder = new List<Character>();
    public bool charactersSpawned = false;
    private int currentInitiative = 0;
    // Start is called before the first frame update
    
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

    public void Update()
    {
        InitialiseCombat();
    }

    private void InitialiseCombat()
    {
        if(charactersSpawned)
        {
            charactersSpawned = false;
            foreach(var character in Characters)
            {
                character.RollInitiative();
                InitiativeOrder.Add(character);
                Debug.Log(character + " " + character.Initiative);
            }
            foreach(var enemy in Enemies)
            {
                enemy.RollInitiative();
                InitiativeOrder.Add(enemy);
            }
            InitiativeOrder.OrderBy(x => x.Initiative).Last();
            EndTurn();
        }
        
    }

    public void EndTurn()
    {
        MouseController.Instance.CurrentCharacter = InitiativeOrder[currentInitiative];
        currentInitiative++;
        if(currentInitiative == InitiativeOrder.Count)
        {
            currentInitiative = 0;
        }
        MouseController.Instance.CurrentCharacter.ResetTurn();
        MouseController.Instance.ResetTurn();
    }
}
