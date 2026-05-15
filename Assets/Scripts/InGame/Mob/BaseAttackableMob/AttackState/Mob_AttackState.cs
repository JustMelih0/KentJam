using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob_AttackState", menuName = "SO/States/Mob/AttackState", order = 0)]
public class Mob_AttackState : MobState
{
    [System.Serializable]
    public struct AttackRangeData
    {
        public Vector2 attackBoxSize;
        public Vector2 attackBoxOffset;
    }

    int attackIndex = 0;
    public int attackCombo = 2;

    [Header("Attack Range")]
    [SerializeField] private AttackRangeData[] attackRanges;
    [SerializeField] protected LayerMask targetLayer;

    [SerializeField] private float attackCooldown = 1.5f;

    [HideInInspector] public bool canAttack = true;
    private Coroutine attackCoroutine;

    protected Collider2D target;


    public override void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        base.InitState(stateMachine, defaultMob);
        canAttack = true;
    }
    public override void EnterState()
    {
        machine.canTransationState = false;

        attackIndex = 0;
        target = FindTargetInAttackRange();

        mob.anim.SetInteger("attackIndex", attackIndex);
        mob.anim.SetTrigger("AttackState");
    }

    public override void ExitState()
    {
        target = null;
    }

    public override void PhysicExecute()
    {
    }

    public override void Execute()
    {

        if (target == null)
        {
            target = FindTargetInAttackRange();
        }
    }

    public override void AnimationEvent(string actionName)
    {
        base.AnimationEvent(actionName);

        if (actionName == "AttackStart")
        {
            if (target == null)
                target = defaultMob.IsTargetInViewRange();

            if (target != null)
                mob.FaceToTarget(target.transform.position);
        }

        if (actionName == "AttackEnd")
        {
            if (attackIndex < attackCombo)
            {
                attackIndex++;
                mob.anim.SetInteger("attackIndex", attackIndex);
            }
            else
            {
                machine.canTransationState = true;
                machine.StateRequest("AttackEnd");
                attackCoroutine ??= machine.StartCoroutine(AttackCooldownTimer());
            }
        }

        if (actionName == "Attack")
        {
            Attack();
        }
    }
    protected virtual void Attack()
    {
        Collider2D hitTarget = FindTargetInAttackRange();

        if (hitTarget != null && hitTarget.TryGetComponent(out IHitable hitable))
        {
            hitable.TakeDamage(mob.stats.attackPower, mob.transform.position);
        }
    }
    protected IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        attackCoroutine = null;
    }

    Collider2D FindTargetInAttackRange()
    {
        Vector2 center = GetAttackBoxCenter();
        Vector2 size = GetCurrentAttackBoxSize();

        return Physics2D.OverlapBox(center, size, 0f, targetLayer);
    }

    Vector2 GetAttackBoxCenter()
    {
        if (mob.attackPoint == null) return mob.transform.position;

        AttackRangeData rangeData = GetCurrentAttackRangeData();

        float dir = mob.facingRight >= 0 ? 1f : -1f;
        Vector2 origin = mob.attackPoint.position;

        Vector2 center = origin;
        center.x += dir * (rangeData.attackBoxSize.x * 0.5f + rangeData.attackBoxOffset.x);
        center.y += rangeData.attackBoxOffset.y;

        return center;
    }

    Vector2 GetCurrentAttackBoxSize()
    {
        return GetCurrentAttackRangeData().attackBoxSize;
    }

    AttackRangeData GetCurrentAttackRangeData()
    {
        int index = Mathf.Clamp(attackIndex, 0, attackRanges.Length - 1);
        return attackRanges[index];
    }

    public override void DebugGizmos()
    {
        base.DebugGizmos();

        if (mob == null || mob.attackPoint == null) return;

        Gizmos.color = Color.red;
        Vector2 center = GetAttackBoxCenter();
        Vector2 size = GetCurrentAttackBoxSize();
        Gizmos.DrawWireCube(center, size);
    }
}