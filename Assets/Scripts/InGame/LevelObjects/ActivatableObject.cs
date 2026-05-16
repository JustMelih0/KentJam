using UnityEngine;

public class ActivatableObject : MonoBehaviour, IActivate
{
    protected bool activated = false;
    public virtual void Activate()
    {
        activated = true;
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

}
