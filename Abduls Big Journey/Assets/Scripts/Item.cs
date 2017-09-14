using UnityEngine;

public class Item : MonoBehaviour {

    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
