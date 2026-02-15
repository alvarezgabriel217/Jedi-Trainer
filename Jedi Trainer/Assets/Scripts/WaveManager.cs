using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public List<Wave> waves;
    public float spawnTimer;
    public int currentWave = 0;
    public GameObject enemyPrfab;
    public GameObject bossPrefab;

    [Header("UI")]
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI enemiesLeftText;


    public IEnumerator Spawn()
    {
        while (currentWave < waves.Count)
        {
            Debug.Log($"WAVE {currentWave+1}");
            currentWaveText.text = $"Wave: {(currentWave+1).ToString()}";
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
}
