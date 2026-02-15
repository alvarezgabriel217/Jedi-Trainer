using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave 
{
    public int enemiesToSummon;
    public int enemiesSummoned = 0;
    public List<GameObject> enemies;
    public List<GameObject> deadEnemies;

    public void Spawn()
    {
        float bounds = GameManager.instance.planeRenderer.bounds.size.x * 0.4f;
        Vector3 randomSpawnPosition = new Vector3(UnityEngine.Random.Range(-bounds, bounds), 1, UnityEngine.Random.Range(-bounds, bounds));
        GameObject newEnemy = WaveManager.instance.SpawnEnemy(randomSpawnPosition);
        newEnemy.GetComponent<Enemy>().SetDestination();
        newEnemy.GetComponent<Enemy>().wave = this;
        enemiesSummoned++;
        enemies.Add(newEnemy);
        WaveManager.instance.enemiesLeftText.text = $"x{enemiesSummoned - deadEnemies.Count}";
    }
}
