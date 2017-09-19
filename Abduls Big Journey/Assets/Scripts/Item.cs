using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type
    {
        PlayerItem,
        EnemyItem
    }
    public Type type;

    public int damage;

    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(-Vector3.forward * (Time.deltaTime * rotateSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == Type.PlayerItem)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<Enemy>().Hit(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().Hit(damage);
                Destroy(gameObject);
            }
        }
    }
}
