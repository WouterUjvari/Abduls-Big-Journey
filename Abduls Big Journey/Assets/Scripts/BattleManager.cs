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

    public int currentTurn;

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

    public void EndTurn()
    {
        if (Battle.instance.enemies.Count != 0)
        {
            turnState = TurnState.Enemy;
        }
    }
}
