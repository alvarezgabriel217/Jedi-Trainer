using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public int damage = 20;
    public BoxCollider attackCollider;
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CloseCollider();
        other.GetComponent<Player>().TakeDamage(damage);
    }

    public void OpenCollider()
    {
        attackCollider.enabled = true;
    }

    public void CloseCollider()
    {
        attackCollider.enabled = false;
    }
}
