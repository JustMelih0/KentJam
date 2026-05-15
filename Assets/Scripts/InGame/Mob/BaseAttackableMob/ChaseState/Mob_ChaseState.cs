using UnityEngine;

[CreateAssetMenu(fileName = "Mob_ChaseState", menuName = "SO/States/Mob/ChaseState", order = 0)]
public class Mob_ChaseState : MobState
{
    public float extraRange = 1f;

    [Range(0.1f, 1f)]
    public float engageMultiplier = 0.75f;
    [HideInInspector] public float chaseTime;

    protected Collider2D target;
    private bool canMove;

    [SerializeField] private float tickInterval = 0.15f;
    protected override float TickInterval => tickInterval;

    public override void EnterState()
    {
        canMove = false;
        chaseTime = 0f;
        mob.anim.SetTrigger("LocomotionState");
        mob.anim.SetFloat("HorizontalInput", 1);
        ResetTick(true);
    }

    protected override void OnTick()
    {
        target = defaultMob.IsTargetInViewRange(mob.viewRange + extraRange);
    }

    public override void Execute()
    {
        base.Execute();
        chaseTime += Time.deltaTime;

        if (target == null)
        {
            canMove = false;
            mob.anim.SetFloat("HorizontalInput", 0);
            machine.StateRequest("TargetLost");
            return;
        }

        if(mob.mob_HealthBase.isHitting) return;

        mob.FaceToTarget(target.transform.position);

        float engageRange = mob.attackRange * engageMultiplier;

        float dist = Vector2.Distance(mob.attackPoint.position, target.ClosestPoint(mob.attackPoint.position));
        if (dist <= engageRange)
        {
            canMove = false;
            mob.anim.SetFloat("HorizontalInput", 0);
            machine.StateRequest("NearTheTarget");
            return;
        }

        canMove = true;
        mob.anim.SetFloat("HorizontalInput", 1);
    }

    public override void PhysicExecute()
    {
        if (!canMove || target == null) return;
        if(mob.mob_HealthBase.isHitting) return;
        /*
        float nextX = Mathf.MoveTowards(
            mob.rgb2d.position.x,
            target.transform.position.x,
            mob.stats.moveSpeed * Time.fixedDeltaTime
        );

        mob.rgb2d.MovePosition(new Vector2(nextX, mob.rgb2d.position.y));*/
        mob.transform.position = Vector2.MoveTowards(mob.transform.position, new Vector2(target.transform.position.x ,mob.transform.position.y), mob.stats.moveSpeed * Time.fixedDeltaTime);
    }

    public override void ExitState()
    {
        mob.anim.SetFloat("HorizontalInput", 0);
        target = null;
        canMove = false;
        mob.rgb2d.linearVelocity = Vector2.zero;
    }
}
