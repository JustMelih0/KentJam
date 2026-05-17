using UnityEngine;

public class ActivatableObject : MonoBehaviour, IActivate
{
    [SerializeField] protected ParticleSystem sleepParticle;
    public GameObject eyeParticle;
    protected bool activated = false;
    public virtual void Activate()
    {
        if(activated) return;

        activated = true;
        sleepParticle.Stop();
        Instantiate(eyeParticle, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX("wakeup");
    }

    public virtual void Deactivate()
    {
        if(!activated) return;

        sleepParticle.Play();
        activated = false;
    }

}
