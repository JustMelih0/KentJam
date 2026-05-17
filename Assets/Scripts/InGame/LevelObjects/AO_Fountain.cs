using UnityEngine;

public class AO_Fountain : ActivatableObject
{
    public GameObject waterProjectile;
    public float waterRiseSpeed = 3f;
    public float waterRiseHeight = 3f;
    public Transform waterObject;

    private ParticleSystem waterParticle;
    private Vector3 waterStartPosition;
    private Vector3 waterTargetPosition;

    private void Awake()
    {
        if (waterProjectile != null)
        {
            waterParticle = waterProjectile.GetComponentInChildren<ParticleSystem>();
        }

        if (waterObject != null)
        {
            waterStartPosition = waterObject.position;
            waterTargetPosition = waterStartPosition;
        }
    }

    private void Update()
    {
        WaterRise();
    }

    public override void Activate()
    {
        if (activated) return;

        base.Activate();

        if (waterParticle != null)
        {
            waterParticle.Play();
        }

        waterTargetPosition = waterStartPosition + Vector3.up * waterRiseHeight;
    }

    public override void Deactivate()
    {
        if (!activated) return;

        base.Deactivate();

        if (waterParticle != null)
        {
            waterParticle.Stop();
        }
    }

    public void WaterRise()
    {
        if (waterObject == null) return;

        waterObject.position = Vector3.MoveTowards(
            waterObject.position,
            waterTargetPosition,
            waterRiseSpeed * Time.deltaTime
        );
    }
}
