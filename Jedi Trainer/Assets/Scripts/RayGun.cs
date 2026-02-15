using UnityEngine;

public class RayGun : MonoBehaviour
{
    [Header("Laser Settings")]
    public GameObject laserPrefab;
    public float fireIntervalMin = 1.0f;
    public float fireIntervalMax = 3.0f;
    public float angleMin = -45f;
    public float angleMax = 45f;
    public float laserDistance = 10f;
    public float laserDuration = 0.2f;
    public float laserSpeed = 20f; // How fast the laser moves

    private float nextFireTime = 0f;

    void Start()
    {
        ScheduleNextFire();
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireLaser();
            ScheduleNextFire();
        }
    }

    private void ScheduleNextFire()
    {
        float interval = UnityEngine.Random.Range(fireIntervalMin, fireIntervalMax);
        nextFireTime = Time.time + interval;
    }

    private void FireLaser()
    {
        // Pick a random angle in Z on the enemy's local forward axis
        float randomZAngle = UnityEngine.Random.Range(angleMin, angleMax);
        Quaternion rotation = Quaternion.Euler(0f, 0f, randomZAngle);

        Vector3 direction = rotation * Vector3.right;

        if (laserPrefab != null)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, rotation);

            // Configure laser to move forward in its local direction
            LaserMover mover = laser.GetComponent<LaserMover>();
            if (mover == null)
            {
                mover = laser.AddComponent<LaserMover>();
            }
            mover.Initialize(direction, laserSpeed, laserDistance, laserDuration);

            Destroy(laser, laserDuration);
        }
    }

    // Helper component to move the laser forward in its direction
    public class LaserMover : MonoBehaviour
    {
        private Vector3 moveDirection;
        private float moveSpeed;
        private float maxDistance;
        private float traveled = 0f;
        private float duration;
        private LineRenderer lr;

        public void Initialize(Vector3 direction, float speed, float distance, float dur)
        {
            moveDirection = direction.normalized;
            moveSpeed = speed;
            maxDistance = distance;
            duration = dur;

            lr = GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position);
            }
        }

        void Update()
        {
            float moveStep = moveSpeed * Time.deltaTime;
            float nextStep = Mathf.Min(moveStep, maxDistance - traveled);
            traveled += nextStep;

            transform.position += moveDirection * nextStep;

            // Optionally, extend the LineRenderer
            if (lr != null)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position + moveDirection * (maxDistance - traveled));
            }

            if (traveled >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

}
