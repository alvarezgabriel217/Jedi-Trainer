using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent agent;
    public GameObject target;
    public Wave wave;

    

    public void SetDestination()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public override void Kill()
    {
        base.Kill();
        wave.deadEnemies.Add(this.gameObject);
        wave.enemies.Remove(this.gameObject);
        gameObject.SetActive(false);
    }
}
