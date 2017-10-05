using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator anim;

    public int maxHealth;
    public int currentHealth;

    public GameObject throwingItem;
    public float throwForce;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Attack()
    {
        GameObject item = Instantiate(throwingItem, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);

        Vector3 playerHitDirectionRandomizer = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));

        Vector3 playerHitDirection = new Vector3(Battle.instance.player.transform.position.x + playerHitDirectionRandomizer.x,
                                                 Battle.instance.player.transform.position.y + 5 + playerHitDirectionRandomizer.y,
                                                 Battle.instance.player.transform.position.z + playerHitDirectionRandomizer.z);
        Vector3 shootDirection = (playerHitDirection - transform.position).normalized;

        item.GetComponent<Rigidbody>().AddForce(shootDirection * throwForce);
    }

    public virtual void Hit(int damage)
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
