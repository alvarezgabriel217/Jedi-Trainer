using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : Character
{
    public int MaxForce;
    public int Force;
    public bool usingElectricity;
    public float forceCd = 1f;
    public int forcePerCd = 2;
    public Coroutine electricityRoutine;
    public float lightningCd = 0.25f;
    public int lightningCost = 20;
    public GameObject electricityObject;
    public Transform leftController;
    public LayerMask enemyLayer;
    public GameObject healEffect;
    public AudioClip healSound;
    public int healCost = 50;
    public float futureDuration = 10f;
    public int futureCost = 50;
    public bool seeingFuture = false;
    public GameObject secondSaberSocket;
    public bool dualWield = false;

    protected override void Start()
    {
        base.Start();
        Force = MaxForce;
        GameManager.instance.healthText.text = $"{Hp}/{MaxHp}";
        GameManager.instance.healthBar.fillAmount = (Hp * 1.0f) / MaxHp;
        GameManager.instance.forceText.text = $"{Force}/{MaxForce}";
        GameManager.instance.forceBar.fillAmount = (Force * 1.0f) / MaxForce;
        StartCoroutine(ForceGain());
    }

    public IEnumerator ForceGain()
    {
        while (true)
        {
            yield return new WaitForSeconds(forceCd);
            if (Force < MaxForce && !usingElectricity) GainForce(forcePerCd);
        }
    }

    public void ShootLightning()
    {
        electricityRoutine = StartCoroutine(ShootLightningRoutine());
        electricityObject.SetActive(true);
        usingElectricity = true;
    }

    public void StopLightning()
    {
        if (electricityRoutine != null) StopCoroutine(electricityRoutine);
        electricityObject.SetActive(false);
        usingElectricity = false;
    }

    public IEnumerator ShootLightningRoutine()
    {
        while (Force > lightningCost)
        {
            yield return new WaitForSeconds(lightningCd);
            ConsumeForce(lightningCost);
            Ray ray = new Ray(leftController.position, leftController.forward);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 2f, 1500f, enemyLayer);
            //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 10);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("Enemy")) hit.collider.GetComponent<Enemy>().TakeDamage(20);

            }
        }
        StopLightning();
    }

    public void GainForce(int amount)
    {
        Force = Mathf.Clamp(Force + amount, 0, MaxForce);
        GameManager.instance.forceText.text = $"{Force}/{MaxForce}";
        GameManager.instance.forceBar.fillAmount = (Force * 1.0f) / MaxForce;
    }

    public void ConsumeForce(int amount)
    {
        Force = Mathf.Clamp(Force - amount, 0, MaxForce);
        GameManager.instance.forceText.text = $"{Force}/{MaxForce}";
        GameManager.instance.forceBar.fillAmount = (Force * 1.0f) / MaxForce;
    }

    public void Heal(int amount)
    {
        if (Force >= healCost) Force -= healCost;
        else return;
        Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
        healEffect.SetActive(true);
        healEffect.GetComponent<ParticleSystem>().Play();
        healEffect.GetComponent<AudioSource>().PlayOneShot(healSound);
        GameManager.instance.healthText.text = $"{Hp}/{MaxHp}";
        GameManager.instance.healthBar.fillAmount = (Hp * 1.0f) / MaxHp;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        GameManager.instance.healthBar.fillAmount = Hp * 1.0f / MaxHp;
        GameManager.instance.healthText.text = $"{Hp}/{MaxHp}";
    }

    public IEnumerator SeeFuture()
    {
        ConsumeForce(futureCost);
        seeingFuture = true;
        foreach (GameObject spawnPos in WaveManager.instance.waves[WaveManager.instance.currentWave].spawnPositionObjects)
        {
            spawnPos.GetComponent<SpawnPosition>().Enable();
        }
        yield return new WaitForSeconds(futureDuration);
        foreach (GameObject spawnPos in WaveManager.instance.waves[WaveManager.instance.currentWave].spawnPositionObjects)
        {
            spawnPos.GetComponent<SpawnPosition>().Disable();
        }
        seeingFuture = false;
    }

    public void DualWield()
    {
        ConsumeForce(Force);
        secondSaberSocket.SetActive(true);
        dualWield = true;
    }
}
