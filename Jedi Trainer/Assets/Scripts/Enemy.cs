using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public Wave wave;

    void Update()
    {
        
    }

    public void SetDestination()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }
}
