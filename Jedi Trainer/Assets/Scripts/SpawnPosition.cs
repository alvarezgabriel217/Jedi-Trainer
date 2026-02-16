using TMPro;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public TextMeshProUGUI spawnTimer;
    public GameObject mesh;
    public GameObject effect;
    public float spawnTime = 0.5f;
    public bool finishedSpawning = false;

    public void Enable()
    {
        if (finishedSpawning) return;
        spawnTimer.gameObject.SetActive(true);
        effect.SetActive(true);
        mesh.SetActive(true);
    }

    public void Disable()
    {
        spawnTimer.gameObject.SetActive(false);
        effect.SetActive(false);
        mesh.SetActive(false);
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        spawnTimer.transform.rotation = Quaternion.LookRotation(spawnTimer.transform.position - Camera.main.transform.position);
        spawnTimer.text = $"Spawning in: {spawnTime}";
        if (spawnTime <= 0)
        {
            finishedSpawning = true;
            gameObject.SetActive(false);
        }
    }
}
