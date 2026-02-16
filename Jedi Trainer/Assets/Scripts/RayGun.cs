using UnityEngine;

public class RayGun : MonoBehaviour
{
    [Header("Laser Settings")]
    public GameObject laserPrefab;
    public GameObject targetPrefab; // The target to aim at
    public float fireIntervalMin = 1.0f;
    public float fireIntervalMax = 3.0f;
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
        if (laserPrefab == null)
            return;

        Vector3 fireOrigin = transform.position;
        Vector3 direction = Vector3.forward;
        Quaternion rotation = Quaternion.identity;

        if (targetPrefab != null)
        {
            Vector3 targetPosition = targetPrefab.transform.position;
            direction = (targetPosition - fireOrigin).normalized;
            rotation = Quaternion.LookRotation(direction);
        }

        GameObject laser = Instantiate(laserPrefab, fireOrigin, rotation);

        // Configure laser to move toward the target
        LaserMover mover = laser.GetComponent<LaserMover>();
        if (mover == null)
        {
            mover = laser.AddComponent<LaserMover>();
        }
        mover.Initialize(direction, laserSpeed, laserDistance, laserDuration);

        Destroy(laser, laserDuration);
    }

    // Helper component to move the laser toward its direction
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
