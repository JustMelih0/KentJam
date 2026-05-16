using UnityEngine;

[CreateAssetMenu(fileName = "Bear_SleepState", menuName = "SO/States/Mob/Bear/SleepState", order = 0)]
public class Bear_SleepState : MobState
{
    [SerializeField] protected bool noWakeAnim = false;
    private const float StopDistance = 0.1f;

    private bool wake = false;
    private bool moveTarget = false;
    protected Bear_Mob bear_Mob;
    public override void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        base.InitState(stateMachine, defaultMob);
        bear_Mob = defaultMob as Bear_Mob;
    }
    public override void EnterState()
    {
        base.EnterState();
        mob.rgb2d.linearVelocityX = 0f;
        mob.ResetAnimatorTriggers();
        mob.anim.SetTrigger("LocomotionState");
    }
    public override void Execute()
    {
        base.Execute();
        if(moveTarget == true) mob.anim.SetFloat("HorizontalInput", 1);
        else mob.anim.SetFloat("HorizontalInput", 0);
    }
    public override void PhysicExecute()
    {
        Move();
        Collider2D col = Physics2D.OverlapCircle(mob.attackPoint.position, bear_Mob.attackRange, mob.attackLayer);
        if (col)
        {
            machine.StateRequest("NearTarget");
        }
    }
    public void Wake()
    {
        if(wake) return;

        Instantiate(bear_Mob.eyeParticle, mob.rootPoint.position, Quaternion.identity);

        mob.FaceToTarget(bear_Mob.targetMovePoint.position);
        if(noWakeAnim == false)
            mob.anim.SetTrigger("Wake");
        else moveTarget = true; 
        wake = true;
    }
    public void Sleep()
    {
        if(!wake) return;

        wake = false;
        moveTarget = false;
    }
    public override void AnimationEvent(string actionName)
    {
        base.AnimationEvent(actionName);
        if (actionName == "Wake")
        {
            moveTarget = true;
        }
    }
    public void Move()
    {
        if(wake == false || moveTarget == false || bear_Mob == null || bear_Mob.targetMovePoint == null)
        {
            mob.rgb2d.linearVelocityX = 0f;
            return;
        }

        Vector2 targetPosition = bear_Mob.targetMovePoint.position;
        Vector2 currentPosition = mob.rgb2d.position;
        Vector2 targetDirection = targetPosition - currentPosition;

        mob.FaceToTarget(targetPosition);

        if (Mathf.Abs(targetDirection.x) <= StopDistance)
        {
            wake = false;
            moveTarget = false;
            mob.anim.SetFloat("HorizontalInput", 0);
            mob.rgb2d.linearVelocityX = 0f;
            return;
        }

        mob.rgb2d.linearVelocityX = Mathf.Sign(targetDirection.x) * mob.stats.moveSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
        mob.rgb2d.linearVelocityX = 0f;
    }
}
