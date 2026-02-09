using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public List<Wave> waves;
    public float spawnTimer;
    public int currentWave = 0;
    public GameObject enemyPrfab;
    public GameObject bossPrefab;

    public IEnumerator Spawn()
    {
        while (currentWave < waves.Count)
        {
            while (waves[currentWave].enemiesSummoned < waves[currentWave].enemiesToSummon)
            {
                yield return new WaitForSeconds(spawnTimer);
                waves[currentWave].Spawn();
            }
            while (waves[currentWave].deadEnemies.Count < waves[currentWave].enemiesToSummon)
            {
                yield return null;
            }
            currentWave++;
        }

    }

    public GameObject SpawnEnemy(Vector3 _spawnPositioon)
    {
        return Instantiate(enemyPrfab, _spawnPositioon, Quaternion.identity);
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
