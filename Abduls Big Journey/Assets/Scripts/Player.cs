using UnityEngine;

public class Player : MonoBehaviour
{

    public Animator anim;

    public int maxHealth;
    public int currentHealth;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            BattleManager.instance.battleState = BattleManager.BattleState.End;
        }
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
    }
}
