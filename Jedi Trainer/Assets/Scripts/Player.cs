using UnityEngine;

public class Player : Character
{
    public int MaxForce;
    public int Force;

    protected override void Start()
    {
        base.Start();
        Force = MaxForce;
        GameManager.instance.healthText.text = $"{Hp}/{MaxHp}";
        GameManager.instance.healthBar.fillAmount = (Hp * 1.0f) / MaxHp;
        GameManager.instance.forceText.text = $"{Force}/{MaxForce}";
        GameManager.instance.forceBar.fillAmount = (Force * 1.0f) / MaxForce;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameManager.instance.healthBar.fillAmount = Hp * 1.0f / MaxHp;
        GameManager.instance.healthText.text = $"{Hp}/{MaxHp}";
    }
}
