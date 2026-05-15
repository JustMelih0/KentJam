using UnityEngine;

public class ActivatableObject : MonoBehaviour, IActivate
{
    protected bool activated = false;
    public virtual void Activate()
    {
        Debug.Log("Obje uyanış geçirddi dang mami");
        activated = true;
    }

    public virtual void Deactivate()
    {
        activated = false;
    }

}
