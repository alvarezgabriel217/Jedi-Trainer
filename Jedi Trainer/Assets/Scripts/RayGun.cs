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

    [Header("Laser Origin")]
    public Transform laserOrigin; // Assign this in the inspector to set the 'middle' of the model

    [Header("Collision Settings")]
    public string clashTag = "ClashObject"; // Tag of the object to trigger sound on collision
    public AudioClip clashSound; // Assign this in the inspector
    public float clashVolume = 1.0f;

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

        // Use specified laserOrigin if set, else default to transform's center
        Vector3 fireOrigin = laserOrigin != null ? laserOrigin.position : GetComponent<Renderer>() != null ? GetComponent<Renderer>().bounds.center : transform.position;
        Vector3 direction = Vector3.forward;
        Quaternion rotation = Quaternion.identity;

        if (targetPrefab != null)
        {
            Vector3 targetPosition = targetPrefab.transform.position;
            direction = (targetPosition - fireOrigin).normalized;
            rotation = Quaternion.LookRotation(direction);
        }
        else if (laserOrigin != null)
        {
            direction = laserOrigin.forward;
            rotation = laserOrigin.rotation;
        }
        else
        {
            direction = transform.forward;
            rotation = transform.rotation;
        }

        GameObject laser = Instantiate(laserPrefab, fireOrigin, rotation);

        // Configure laser to move toward the target
        LaserMover mover = laser.GetComponent<LaserMover>();
        if (mover == null)
        {
            mover = laser.AddComponent<LaserMover>();
        }

        mover.Initialize(
            direction,
            laserSpeed,
            laserDistance,
            laserDuration,
            clashTag,
            clashSound,
            clashVolume
        );

        Destroy(laser, laserDuration);
    }

    // Helper component to move the laser toward its direction and handle clash sound
    public class LaserMover : MonoBehaviour
    {
        private Vector3 moveDirection;
        private float moveSpeed;
        private float maxDistance;
        private float traveled = 0f;
        private float duration;
        private LineRenderer lr;
        private string clashTag;
        private AudioClip clashSound;
        private float clashVolume;
        private AudioSource audioSource; // For playing sound

        public void Initialize(
            Vector3 direction,
            float speed,
            float distance,
            float dur,
            string clashTag = "",
            AudioClip clashSound = null,
            float clashVolume = 1.0f
        )
        {
            moveDirection = direction.normalized;
            moveSpeed = speed;
            maxDistance = distance;
            duration = dur;
            this.clashTag = clashTag;
            this.clashSound = clashSound;
            this.clashVolume = clashVolume;

            lr = GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position);
            }

            // Setup collider for collision detection
            Collider col = GetComponent<Collider>();
            if (col == null)
            {
                SphereCollider sc = gameObject.AddComponent<SphereCollider>();
                sc.isTrigger = true;
                sc.radius = 0.1f;
            }

            // Move audio source setup to NOT depend on clashSound being present, so laser always has AudioSource
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.playOnAwake = false;
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

        void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(clashTag) && other.CompareTag(clashTag))
            {
                // Play the clash sound ON THE LASER (not the RayGun) if assigned
                if (clashSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(clashSound, clashVolume);
                }
                // Immediately destroy the laser on collision with the clashObject
                Destroy(gameObject);
            }
        }
    }

}
