using UnityEngine;

public class Character : MonoBehaviour
{
    public int MaxHp;
    public int Hp;

    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            Debug.Log($"Took {damage} damage");
            Hp = 0;
            Kill();
        }
    }
    public virtual void Kill()
    {
        Debug.Log("Unit Died");
    }
}
