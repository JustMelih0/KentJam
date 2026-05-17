using UnityEngine;

[CreateAssetMenu(fileName = "Bear_AttackState", menuName = "SO/States/Mob/Bear/AttackState", order = 0)]
public class Bear_AttackState : MobState
{
    public GameObject woodBreakParticle;
    public override void EnterState()
    {
        base.EnterState();
        mob.ResetAnimatorTriggers();
        mob.anim.SetTrigger("AttackState");
    }
    public override void PhysicExecute()
    {
    }
    public void Attack()
    {
        Collider2D col = Physics2D.OverlapCircle(mob.attackPoint.position, mob.attackRange, mob.attackLayer);
        if(col.gameObject)
        {
            Instantiate(woodBreakParticle, col.gameObject.transform.position, Quaternion.identity);
            Destroy(col.gameObject);
        }
    }
    public override void AnimationEvent(string actionName)
    {
        base.AnimationEvent(actionName);
        if (actionName == "Attack")
        {
            AudioManager.Instance.PlaySFX("bear_crash");
            Attack();
        }
        else if (actionName == "AttackEnd")
        {
            machine.StateRequest("AttackEnd");
        }
    }
}
