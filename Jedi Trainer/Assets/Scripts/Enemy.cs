using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent agent;
    public GameObject target;
    public Wave wave;
    public Animator animator;
    public AttackCollider attackCollider;

<<<<<<< HEAD
    // Natural orbit/wander variables
    public float minDistanceFromPlayer = 5f;
    public float maxDistanceFromPlayer = 15f;
    public float moveIntervalMin = 2f;
    public float moveIntervalMax = 4.5f;
    public float positionNoise = 1.5f; // How much jitter allowed for natural movement
    private float timer;
    private float currentMoveInterval;
    private Vector3 currentDestination;

    void Start()
    {
        ScheduleNextMove();
        ChooseSmoothOrbitDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            // If the destination has been reached or it's time for a new move, select a new orbit point
            if ((!agent.pathPending && agent.remainingDistance < 0.5f) || timer >= currentMoveInterval)
            {
                ChooseSmoothOrbitDestination();
                ScheduleNextMove();
            }
        }
        else
        {
            // If player is missing, wander more randomly around current position
            if ((!agent.pathPending && agent.remainingDistance < 0.5f) || timer >= currentMoveInterval)
            {
                ChooseRandomWanderDestination();
                ScheduleNextMove();
            }
        }
    }

    void ScheduleNextMove()
    {
        timer = 0f;
        currentMoveInterval = Random.Range(moveIntervalMin, moveIntervalMax);
    }

    void ChooseSmoothOrbitDestination()
    {
        if (GameManager.instance == null || GameManager.instance.player == null)
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;

        // Random direction with some bias based on current position for smoothness
        Vector3 toEnemy = (transform.position - playerPos).normalized;
        if (toEnemy.sqrMagnitude < 0.1f)
            toEnemy = Random.onUnitSphere;
        Vector3 randomTangent = Vector3.Cross(toEnemy, Vector3.up).normalized;
        randomTangent = Quaternion.AngleAxis(Random.Range(-60f, 60f), Vector3.up) * randomTangent;

        float randomRadius = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);

        // Add small noise for natural motion
        Vector3 jitter = new Vector3(
            Random.Range(-positionNoise, positionNoise),
            0,
            Random.Range(-positionNoise, positionNoise)
        );

        Vector3 orbitPos = playerPos + toEnemy * randomRadius;
        Vector3 candidate = orbitPos + randomTangent * Random.Range(-3f, 3f) + jitter;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(candidate, out navHit, 2.0f, NavMesh.AllAreas))
        {
            currentDestination = navHit.position;
            agent.SetDestination(currentDestination);
        }
        else
        {
            // fallback to an outward position
            agent.SetDestination(playerPos + (toEnemy * randomRadius));
        }
    }

    void ChooseRandomWanderDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * maxDistanceFromPlayer;
        randomDirection += transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, maxDistanceFromPlayer, NavMesh.AllAreas))
        {
            currentDestination = navHit.position;
            agent.SetDestination(currentDestination);
        }
    }
=======
    public float attackCooldown = 1f;
    public float attackRange = 2.0f;
    float lastAttackTime;
>>>>>>> 7b4ab9ea8627a2e117d214059a54e60153f6c5cb

    public void SetDestination()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
            agent.SetDestination(GameManager.instance.player.transform.position);
    }

    public override void Kill()
    {
        base.Kill();
        wave.deadEnemies.Add(this.gameObject);
        wave.enemies.Remove(this.gameObject);
        animator.SetTrigger("Dead");
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        agent.isStopped = true;
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

    public override void TakeDamage(int damage)
    {
        if (GameManager.instance.player.GetComponent<Player>().dualWield)
        {
            damage = Hp;
        }
        base.TakeDamage(damage);
    }
}
