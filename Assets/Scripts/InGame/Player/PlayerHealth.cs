using UnityEngine;

public class PlayerHealth : Mob_HealthBase
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void MobDead()
    {
        AudioManager.Instance.PlaySFX("PlayerDead");
        base.MobDead();
    }

}
