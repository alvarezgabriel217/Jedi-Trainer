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
    public GameObject spawnPosition;

    [Header("UI")]
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI enemiesLeftText;


    public IEnumerator Spawn()
    {
        while (currentWave < waves.Count)
        {
            Debug.Log($"WAVE {currentWave + 1}");
            currentWaveText.text = $"Wave: {(currentWave + 1).ToString()}";
            for (int i = 0; i < waves[currentWave].spawnPositionObjects.Count; i++)
            {
                waves[currentWave].spawnPositionObjects[i].GetComponent<SpawnPosition>().spawnTime = spawnTimer * (i + 1);
            }
            if (GameManager.instance.player.GetComponent<Player>().seeingFuture)
            {
                foreach (GameObject spawnPos in waves[currentWave].spawnPositionObjects)
                {
                    spawnPos.GetComponent<SpawnPosition>().Enable();
                }
            }
            if (currentWave == waves.Count - 1)
            {
                waves[currentWave].SpawnBoss();
            }
            else
            {
                while (waves[currentWave].enemiesSummoned < waves[currentWave].enemiesToSummon)
                {
                    yield return new WaitForSeconds(spawnTimer);
                    waves[currentWave].Spawn();
                }
            }
            while (waves[currentWave].deadEnemies.Count < waves[currentWave].enemiesToSummon)
            {
                yield return null;
            }
            currentWave++;
        }

    }

    public GameObject SpawnEnemy(Vector3 _spawnPositioon, bool boss = false)
    {
        return Instantiate(boss == false ? enemyPrfab : bossPrefab, _spawnPositioon, Quaternion.identity);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Wave wave in waves)
        {
            wave.CalculateSpawns();
            foreach (Vector3 spawnPos in wave.spawnPositions)
            {
                GameObject newSpawn = Instantiate(spawnPosition, spawnPos, Quaternion.identity);
                wave.spawnPositionObjects.Add(newSpawn);
            }
        }
        StartCoroutine(Spawn());
    }
}
