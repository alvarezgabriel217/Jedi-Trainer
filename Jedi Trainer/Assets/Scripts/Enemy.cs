using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;
    public Wave wave;

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

    public void SetDestination()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
            agent.SetDestination(GameManager.instance.player.transform.position);
    }
}
