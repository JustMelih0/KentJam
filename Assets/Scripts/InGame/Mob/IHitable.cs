using UnityEngine;

public interface IHitable 
{
    public abstract bool TakeDamage(float damage, Vector2 hitPositin);
}
