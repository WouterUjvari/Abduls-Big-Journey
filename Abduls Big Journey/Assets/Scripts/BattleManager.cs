using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    public List<GameObject> items = new List<GameObject>();
    public List<GameObject> itemButtons = new List<GameObject>();

    public enum BattleState
    {
        Start,
        Battling,
        End
    }
    public BattleState battleState;

    public enum TurnState
    {
        Player,
        Enemy
    }
    public TurnState turnState;

    public bool playerCanAttack = true;

    public int currentTurn = 1;

    public int itemSelected;
    public bool selectedItem;
    public Button lastSelectedItemButton;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        #endregion

        DontDestroyOnLoad(gameObject);
    }

    // sets all variables to the default settings to that you start each level 'fresh' with no leftovers from last level
    public void StartLevel()
    {
        battleState = BattleState.Start;
        turnState = TurnState.Player;
        playerCanAttack = true;
        currentTurn = 1;
        UIManager.instance.turnText.text = "Turn: " + currentTurn;
        selectedItem = false;
        lastSelectedItemButton = null;
    }

    public void EndTurn()
    {
        if (Battle.instance.enemies.Count != 0 && ItemManager.instance.itemsInScene.Count < 1)
        {
            turnState = TurnState.Enemy;
        }
    }
}
