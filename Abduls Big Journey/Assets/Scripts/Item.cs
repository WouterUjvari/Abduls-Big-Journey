﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    public bool canRotate;
    public float rotateSpeed;

    [Header("Extras")]
    public GameObject itemToSpawnOnDestroy;
    public int amountOfItemsToSpawnOnDestroy;

    private void Awake()
    {
        ItemManager.instance.itemsInScene.Add(gameObject);
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.Rotate(-Vector3.forward * (Time.deltaTime * rotateSpeed));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (type == Type.PlayerItem)
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyComponent>().enemy.Hit(damage);

                for (int i = 0; i < amountOfItemsToSpawnOnDestroy; i++)
                {
                    GameObject itemSpawn = Instantiate(itemToSpawnOnDestroy, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    itemSpawn.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(200, 300) + Vector2.right * Random.Range(-200, 200));
                }

                float randomSpeech = Random.value;
                if (randomSpeech < 0.5f)
                {
                    UIManager.instance.NewSpeechBubble(GameObject.FindWithTag("Player").GetComponent<Player>().speechBubbleSpawn, "Gotcha!", 2, 1, 15, 5);
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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        ItemManager.instance.itemsInScene.Remove(gameObject);
    }
}
