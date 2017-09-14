using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {

    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
    }

    public void Die()
    {
        Battle.instance.enemies.Remove(gameObject);
        Destroy(gameObject);
        print("killed enemy");
    }
}
