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

    private GameObject player;
    private Vector3 mousePos;

    private List<GameObject> enemiesWhoCanAttack = new List<GameObject>();
    private bool canCreateEnemiesWhoCanAttack = true;
    private bool enemyCanAttack = true;
    public float enemyAttackInterval = 2f;

    [Header("Force")]
    public float throwForce;
    public float maxForce = 80;
    public float forceIncreaseSpeed = 3f;

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
    }

    private void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 9f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (BattleManager.instance.battleState == BattleManager.BattleState.Battling)
        {
            // player can take a turn
            if (BattleManager.instance.turnState == BattleManager.TurnState.Player)
            {
                UIManager.instance.gameplayPanel.SetActive(true);

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
                            ChargeAttack();
                        }

                        // release left mouse button (throws the item)
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (throwForce != 0)
                            {
                                Attack();
                            }
                        }

                        // click right mouse button (cancels the attack)
                        if (Input.GetMouseButtonDown(1))
                        {
                            CancelAttack();
                        }
                    }
                    else
                    {
                        BattleManager.instance.selectedItem = false;
                        UIManager.instance.forceCursor.SetActive(false);
                    }

                    if (enemies.Count == 0 || availableItems.Count == 0)
                    {
                        BattleManager.instance.battleState = BattleManager.BattleState.End;
                    }
                }
            }
            // enemy can take a turn
            else if (BattleManager.instance.turnState == BattleManager.TurnState.Enemy)
            {
                UIManager.instance.gameplayPanel.SetActive(false);

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
                    print("enemies' turn ended, player should be allowed to attack now");
                    BattleManager.instance.turnState = BattleManager.TurnState.Player;
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

    // player attacks
    public void ChargeAttack()
    {
        if (throwForce <= maxForce)
        {
            throwForce += Time.deltaTime * forceIncreaseSpeed;
        }
        UIManager.instance.forceCursorFill.fillAmount = throwForce / maxForce;
    }

    public void Attack()
    {
        BattleManager.instance.playerCanAttack = false;

        // instantiating the item, adding force to it and removing it from the list
        GameObject item = Instantiate(BattleManager.instance.items[BattleManager.instance.itemSelected], player.transform);

        Vector3 direction = (mousePos - player.transform.position).normalized;
        item.GetComponent<Rigidbody>().AddForce(direction * throwForce);

        availableItems.Remove(BattleManager.instance.items[BattleManager.instance.itemSelected]);

        BattleManager.instance.selectedItem = false;

        // resetting force 
        UIManager.instance.forceCursor.SetActive(false);
        throwForce = 0;
        UIManager.instance.forceCursorFill.GetComponent<Image>().fillAmount = 0;
    }

    public void CancelAttack()
    {
        BattleManager.instance.selectedItem = false;

        // resetting force
        UIManager.instance.forceCursor.SetActive(false);
        throwForce = 0;
        UIManager.instance.forceCursorFill.fillAmount = 0;

        // setting the item button to interactable again
        BattleManager.instance.lastSelectedItemButton.interactable = true;
    }

    // enemy attacks
    public IEnumerator EnemyAttack(int enemy)
    {
        print(enemies[enemy].name + " is attacking!");
        enemyCanAttack = false;
        enemies[enemy].GetComponent<Enemy>().Attack();

        yield return new WaitForSeconds(enemyAttackInterval);
        enemyCanAttack = true;
    }

    private void EndGame(bool victory)
    {
        if (victory == true)
        {
            print("victory");
        }
        else
        {
            print("defeat");
        }
    }
}
