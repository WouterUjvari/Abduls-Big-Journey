﻿using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type
    {
        PlayerItem,
        EnemyItem
    }
    [Header("Item Properties")]
    public Type type;

    public int damage;

    public float rotateSpeed;

    [Header("Extras")]
    public GameObject itemToSpawnOnDestroy;
    public int amountOfItemsToSpawnOnDestroy;

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

                for (int i = 0; i < amountOfItemsToSpawnOnDestroy; i++)
                {
                    GameObject itemSpawn = Instantiate(itemToSpawnOnDestroy, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    itemSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(200, 300) + Vector3.right * Random.Range(-200, 200));
                }

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
