
using UnityEngine;

public class AO_Ivy : ActivatableObject
{
    [SerializeField] private GameObject ivyObject;
    [SerializeField] private float deActivateTime = 5f;
    public override void Activate()
    {
        if (activated) return;

        base.Activate();
        ivyObject.SetActive(true);
        Invoke(nameof(Deactivate), deActivateTime);
    }
    public override void Deactivate()
    {
        if (!activated) return;

        base.Deactivate();
        ivyObject.SetActive(false);
    }
}
