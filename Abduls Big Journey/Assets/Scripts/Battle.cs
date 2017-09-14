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

    public GameObject player;
    private Vector3 mousePos;

    [Header("Force")]
    public float projectileForce = 5f;
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
            UIManager.instance.itemPanel.SetActive(true);

            if (BattleManager.instance.turnState == BattleManager.TurnState.Player)
            {
                if (BattleManager.instance.selectedItem == true)
                {
                    // if player has selected an item, show the force charge cursor
                    UIManager.instance.forceCursor.SetActive(true);
                    UIManager.instance.forceCursor.transform.position = Input.mousePosition;

                    if (Input.GetMouseButton(0))
                    {
                        ChargeAttack();
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        if (projectileForce != 0)
                        {
                            Attack();
                        }
                    }

                    if (Input.GetMouseButton(1))
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
            else if (BattleManager.instance.turnState == BattleManager.TurnState.Enemy)
            {
                // enemy attack
            }
        }
        else
        {
            UIManager.instance.itemPanel.SetActive(false);
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

    public void ChargeAttack()
    {
        if (projectileForce <= maxForce)
        {
            projectileForce += Time.deltaTime * forceIncreaseSpeed;
        }
        UIManager.instance.forceCursorFill.fillAmount = projectileForce / maxForce;
    }

    // careful, bunch of ugly code below
    public void Attack()
    {
        GameObject projectile = Instantiate(BattleManager.instance.items[BattleManager.instance.itemSelected], player.transform);
        projectile.GetComponent<Rigidbody>().AddForce(new Vector3(mousePos.x, mousePos.y, 0) * projectileForce);
        availableItems.Remove(BattleManager.instance.items[BattleManager.instance.itemSelected]);
        BattleManager.instance.selectedItem = false;
        UIManager.instance.forceCursor.SetActive(false);
        projectileForce = 0;
        UIManager.instance.forceCursorFill.GetComponent<Image>().fillAmount = 0;
    }

    public void CancelAttack()
    {
        BattleManager.instance.selectedItem = false;
        UIManager.instance.forceCursor.SetActive(false);
        projectileForce = 0;
        BattleManager.instance.lastSelectedItemButton.interactable = true;
    }

    private void EndGame(bool victory)
    {
        if (victory == true)
        {
            // won game
        }
        else
        {
            // lost game
        }
    }
}
