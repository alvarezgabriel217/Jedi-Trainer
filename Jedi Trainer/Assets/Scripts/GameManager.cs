using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public MeshRenderer planeRenderer;
    public TextMeshProUGUI healthText;
    public Image healthBar;
    public TextMeshProUGUI forceText;
    public Image forceBar;
    public enum gameMode
    {
        training,
        wave,
    }
    public gameMode mode = gameMode.wave;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(mode == gameMode.wave) StartCoroutine(WaveManager.instance.Spawn());
    }
}
