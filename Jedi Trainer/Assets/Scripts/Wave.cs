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
    public List<Vector3> spawnPositions;
    public List<GameObject> spawnPositionObjects;

    public void Spawn()
    {
        GameObject newEnemy = WaveManager.instance.SpawnEnemy(spawnPositions[enemiesSummoned]);
        newEnemy.GetComponent<Enemy>().SetDestination();
        newEnemy.GetComponent<Enemy>().wave = this;
        enemiesSummoned++;
        enemies.Add(newEnemy);
        WaveManager.instance.enemiesLeftText.text = $"x{enemiesSummoned - deadEnemies.Count}";
    }

    public void SpawnBoss()
    {
        GameObject newEnemy = WaveManager.instance.SpawnEnemy(spawnPositions[enemiesSummoned], true);
        newEnemy.GetComponent<Enemy>().SetDestination();
        newEnemy.GetComponent<Enemy>().wave = this;
        enemiesSummoned++;
        enemies.Add(newEnemy);
        WaveManager.instance.enemiesLeftText.text = $"x{enemiesSummoned - deadEnemies.Count}";
    }

    public void CalculateSpawns()
    {
        for (int i = 0; i < enemiesToSummon; i++)
        {
            float bounds = GameManager.instance.planeRenderer.bounds.size.x * 0.4f;
            Vector3 randomSpawnPosition = new Vector3(UnityEngine.Random.Range(-bounds, bounds), 1, UnityEngine.Random.Range(-bounds, bounds));
            spawnPositions.Add(randomSpawnPosition);
        }
    }
}
