using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    public int damage;
    public float minSwingSpeed = 1.5f;
    public Transform lightsaberSocket;
    public Vector3 lastPosition;
    public float currentSpeed;
    public bool equipped;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        //if(equipped)
        //{
            currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
            lastPosition = transform.position;

            //if (currentSpeed < 0.2f) ;

        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (currentSpeed < minSwingSpeed) return;

        other.GetComponent<Enemy>().TakeDamage(damage);
    }
}
