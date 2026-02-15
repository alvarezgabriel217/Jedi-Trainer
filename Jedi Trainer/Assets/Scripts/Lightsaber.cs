using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    public int damage;
    public float minSwingSpeed = 1.5f;
    public Vector3 lastPosition;
    public float currentSpeed;
    public bool equipped;
    public AudioSource audioSource;
    public AudioClip SaberMove;
    public AudioClip SaberClash;

    public float SwingCd = 0.25f;
    public float TimeSinceLastSwing;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (currentSpeed > 0.6f && Time.time >= TimeSinceLastSwing + SwingCd)
        {
            audioSource.PlayOneShot(SaberMove);
            TimeSinceLastSwing = Time.time;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (currentSpeed < minSwingSpeed) return;

        other.GetComponent<Enemy>().TakeDamage(damage);
        audioSource.PlayOneShot(SaberClash);
    }
}
