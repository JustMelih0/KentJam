using System;
using System.Collections;
using UnityEngine;

public class Mob_HealthBase : MonoBehaviour, IHitable
{
    protected Mob mob;

    public float currentHealth = 0;
    public ParticleSystem deathParticle;
    public Material impactMaterial;
    private Material defaultMaterial;
    private Coroutine hitCoroutine;
    private Coroutine stunCoroutine;

    public event Action<float, float, Vector2> damagedEvent;
    public event Action deadEvent;
    public static event Action<Mob> mobDeadEvent;

    [HideInInspector] public bool isHitting = false;
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        TryGetComponent(out mob);
    }

    protected virtual void Start()
    {
        if(mob)
        {
            currentHealth = mob.stats.maxHP;
            spriteRenderer = mob.spriteRenderer;
        }
        else
        {
            TryGetComponent(out spriteRenderer);
        }

        if(spriteRenderer)
        defaultMaterial = spriteRenderer.material;
    }

    public bool TakeDamage(float damage, Vector2 hitPositin)
    {


        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, mob.stats.maxHP);
        damagedEvent?.Invoke(currentHealth, mob.stats.maxHP, hitPositin);

        hitCoroutine ??= StartCoroutine(HitImpactTimer());


        if (currentHealth <= 0)
        {
            MobDead();
        }
        return true;



    }

    public void Stun(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }

        stunCoroutine = StartCoroutine(StunTimer(duration));
    }

    protected virtual void MobDead()
    {
        deadEvent?.Invoke();
        if(mob) mobDeadEvent?.Invoke(mob);
        if(deathParticle)
        {
            deathParticle.transform.SetParent(null);
            deathParticle.Play();
        }

        Destroy(gameObject);
    }
    private IEnumerator HitImpactTimer()
    {
        spriteRenderer.material = impactMaterial;
        isHitting = true;
        yield return new WaitForSeconds(0.2f);
        isHitting = false;
        spriteRenderer.material = defaultMaterial;
        hitCoroutine = null;
    }

    private IEnumerator StunTimer(float duration)
    {
        isHitting = true;
        yield return new WaitForSeconds(duration);
        isHitting = false;
        stunCoroutine = null;
    }
}
