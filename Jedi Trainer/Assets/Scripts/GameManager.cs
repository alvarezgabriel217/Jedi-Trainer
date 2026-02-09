using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public MeshRenderer planeRenderer;

    private void Awake()
    {
        instance = this;
    }
}
