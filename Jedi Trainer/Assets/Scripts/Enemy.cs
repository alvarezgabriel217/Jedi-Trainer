using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent agent;
    public GameObject target;
    public Wave wave;
    public Animator animator;
    public AttackCollider attackCollider;

    public float attackCooldown = 1f;
    public float attackRange = 2.0f;
    float lastAttackTime;

    public void SetDestination()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public override void Kill()
    {
        base.Kill();
        wave.deadEnemies.Add(this.gameObject);
        wave.enemies.Remove(this.gameObject);
        animator.SetTrigger("Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        agent.enabled = false;
        attackCollider.enabled = false;
        WaveManager.instance.enemiesLeftText.text = $"x{wave.enemiesSummoned - wave.deadEnemies.Count}";
    }

    public void OpenCollider()
    {
        attackCollider.OpenCollider();
    }

    public void CloseCollider()
    {
        attackCollider.CloseCollider();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if(Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= attackRange)
        {
            agent.isStopped = true;
            transform.rotation = Quaternion.LookRotation(GameManager.instance.player.transform.position - transform.position).normalized;

            if(Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
            }
        }
    }
}
