﻿using UnityEngine;

public class DisplayHealth : MonoBehaviour
{

    private Player player;

    private int enemyMaxHealth;

    private void Awake()
    {
        player = Battle.instance.player.GetComponent<Player>();
    }

    private void Start()
    {
        enemyMaxHealth = CalculateEnemyMaxHealth();
    }

    private void Update()
    {
        UIManager.instance.enemyTotalHealthbar.fillAmount = (float)CalculateEnemyCurrentHealth() / enemyMaxHealth;
        UIManager.instance.playerHealthbar.fillAmount = player.currentHealth / player.maxHealth;
    }

    private int CalculateEnemyCurrentHealth()
    {
        int currentHealth = new int();

        foreach (GameObject enemy in Battle.instance.enemies)
        {
            currentHealth += enemy.GetComponent<Enemy>().currentHealth;
        }

        return currentHealth;
    }

    private int CalculateEnemyMaxHealth()
    {
        int maxHealth = new int();

        foreach (GameObject enemy in Battle.instance.enemies)
        {
            maxHealth += enemy.GetComponent<Enemy>().maxHealth;
        }

        return maxHealth;
    }
}
