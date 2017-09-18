using UnityEngine;

public class Player : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    private void Update()
    {
        if (currentHealth <= 0)
        {
            BattleManager.instance.battleState = BattleManager.BattleState.End;
        }
    }
}
