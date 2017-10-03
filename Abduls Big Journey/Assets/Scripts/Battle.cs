using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{

    public static Battle instance;

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> availableItems = new List<GameObject>();

    public int item1;
    public int item2;
    public int item3;
    public int item4;

    public GameObject player;
    private Player playerScript;
    public Vector3 mousePos;

    private List<GameObject> enemiesWhoCanAttack = new List<GameObject>();
    private bool canCreateEnemiesWhoCanAttack = true;
    private bool enemyCanAttack = true;
    public float enemyAttackInterval = 2f;

    public GameObject lastThrowLocation;

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

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();

        // finds all the enemies in the scene and adds them to the enemies list
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy);
        }
    }

    private void Start()
    {
        // clears the itemPanel to make room for this levels available items
        for (int i = 0; i < UIManager.instance.itemPanel.transform.childCount; i++)
        {
            Destroy(UIManager.instance.itemPanel.transform.GetChild(i).gameObject);
        }

        // adds the specified amount of items to the available items list and creates a button in the itemPanel
        for (int i = 0; i < item1; i++)
        {
            availableItems.Add(BattleManager.instance.items[0]);
            GameObject itemButton = Instantiate(BattleManager.instance.itemButtons[0], UIManager.instance.itemPanel.transform);
        }
        for (int i = 0; i < item2; i++)
        {
            availableItems.Add(BattleManager.instance.items[1]);
            GameObject itemButton = Instantiate(BattleManager.instance.itemButtons[1], UIManager.instance.itemPanel.transform);
        }
        for (int i = 0; i < item3; i++)
        {
            availableItems.Add(BattleManager.instance.items[2]);
            GameObject itemButton = Instantiate(BattleManager.instance.itemButtons[2], UIManager.instance.itemPanel.transform);
        }
        for (int i = 0; i < item4; i++)
        {
            availableItems.Add(BattleManager.instance.items[3]);
            GameObject itemButton = Instantiate(BattleManager.instance.itemButtons[3], UIManager.instance.itemPanel.transform);
        }
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 9f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //anim.SetFloat("Force", throwForce);

        if (BattleManager.instance.battleState == BattleManager.BattleState.Battling)
        {
            // player can take a turn
            if (BattleManager.instance.turnState == BattleManager.TurnState.Player)
            {
                UIManager.instance.gameplayPanel.SetActive(true);
                UIManager.instance.itemPanel.SetActive(true);

                if (BattleManager.instance.playerCanAttack)
                {
                    if (BattleManager.instance.selectedItem == true)
                    {
                        // if player has selected an item, show the force charge cursor
                        UIManager.instance.forceCursor.SetActive(true);
                        UIManager.instance.forceCursor.transform.position = Input.mousePosition;

                        // hold left mouse button (slowly increases the force at which the item is gonna get launched at)
                        if (Input.GetMouseButton(0))
                        {
                            playerScript.ChargeAttack();
                        }

                        // release left mouse button (throws the item)
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (playerScript.throwForce != 0)
                            {
                                playerScript.Attack();
                            }
                        }

                        // click right mouse button (cancels the attack)
                        if (Input.GetMouseButtonDown(1))
                        {
                            playerScript.CancelAttack();
                        }
                    }
                    else
                    {
                        BattleManager.instance.selectedItem = false;
                        UIManager.instance.forceCursor.SetActive(false);
                    }

                }

                if (enemies.Count == 0)
                {
                    BattleManager.instance.battleState = BattleManager.BattleState.End;
                }
            }
            // enemy can take a turn
            else if (BattleManager.instance.turnState == BattleManager.TurnState.Enemy)
            {
                UIManager.instance.itemPanel.SetActive(false);

                // extra if statement cuz this is all done in update but we only want to do this once
                if (canCreateEnemiesWhoCanAttack)
                {
                    canCreateEnemiesWhoCanAttack = false;

                    // creates a list to store all enemies who can attack
                    enemiesWhoCanAttack = new List<GameObject>();

                    // loops through all alive enemies and adds them to the above list
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        enemiesWhoCanAttack.Add(enemies[i]);
                    }
                }

                // this bool is default set to true, if its true it lets a random enemy from the list enemiesWhoCanAttack attack, 
                // while hes attacking the bool is set to false and he gets thrown out of the list, after hes done the next random enemy can attack
                if (enemyCanAttack)
                {
                    int enemyWhoGetsToAttack = (Random.Range(0, enemiesWhoCanAttack.Count));

                    StartCoroutine(EnemyAttack(enemyWhoGetsToAttack));

                    enemiesWhoCanAttack.Remove(enemiesWhoCanAttack[enemyWhoGetsToAttack]);
                }

                // if the list enemiesWhoCanAttack is empty, all the enemies have attacked and the player gets to play again
                if (enemiesWhoCanAttack.Count == 0)
                {
                    if (availableItems.Count == 0)
                    {
                        BattleManager.instance.battleState = BattleManager.BattleState.End;
                    }

                    print("enemies' turn ended, player should be allowed to attack now");
                    BattleManager.instance.turnState = BattleManager.TurnState.Player;

                    BattleManager.instance.currentTurn++;
                    UIManager.instance.turnText.text = "Turn: " + BattleManager.instance.currentTurn;

                    BattleManager.instance.playerCanAttack = true;
                    canCreateEnemiesWhoCanAttack = true;
                }
            }
        }
        
        // if all enemies are dead, end the game with a victory, else with a defeat
        if (BattleManager.instance.battleState == BattleManager.BattleState.End)
        {
            if (enemies.Count == 0)
            {
                EndGame(true);
            }
            else
            {
                EndGame(false);
            }
        }
    }

    public void SelectItem(int item, Button button)
    {
        BattleManager.instance.selectedItem = true;
        BattleManager.instance.itemSelected = item;
        BattleManager.instance.lastSelectedItemButton = button;

        button.interactable = false;
    }

    public IEnumerator EnemyAttack(int enemy)
    {
        print(enemies[enemy].name + " is attacking!");
        enemyCanAttack = false;
        enemies[enemy].GetComponent<EnemySquidScript>().Attack();

        yield return new WaitForSeconds(enemyAttackInterval);
        enemyCanAttack = true;
    }

    public void EndGame(bool victory)
    {
        if (victory == true)
        {
            UIManager.instance.victoryOrDefeatText.text = "Victory!";
            UIManager.instance.gameEndPanel.SetActive(true);
        }
        else
        {
            UIManager.instance.victoryOrDefeatText.text = "Defeat!";
            UIManager.instance.gameEndPanel.SetActive(true);
        }
    }
}
