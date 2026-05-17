using UnityEngine;

[CreateAssetMenu(fileName = "Bear_AttackState", menuName = "SO/States/Mob/Bear/AttackState", order = 0)]
public class Bear_AttackState : MobState
{
    public GameObject woodBreakParticle;
    public float playerLaunchForce = 6f;
    public Vector2 playerLaunchBoxSize = new Vector2(2f, 0.6f);
    public Vector2 playerLaunchBoxOffset = new Vector2(0f, 0.8f);

    public override void EnterState()
    {
        base.EnterState();
        mob.ResetAnimatorTriggers();
        mob.anim.SetTrigger("AttackState");
        LaunchPlayerOnTop();
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
            CameraController.Instance.Shake(0.1f, 0.3f);
            Destroy(col.gameObject);
        }
    }

    private void LaunchPlayerOnTop()
    {
        Vector2 center = (Vector2)mob.transform.position + playerLaunchBoxOffset;
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, playerLaunchBoxSize, 0f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].CompareTag("Player") || !hits[i].TryGetComponent(out Rigidbody2D rb))
                continue;

            rb.linearVelocityY = 0f;
            rb.AddForce(Vector2.up * playerLaunchForce, ForceMode2D.Impulse);
            return;
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

    public override void DebugGizmos()
    {
        base.DebugGizmos();

        if (mob == null) return;

        Gizmos.color = Color.yellow;
        Vector2 center = (Vector2)mob.transform.position + playerLaunchBoxOffset;
        Gizmos.DrawWireCube(center, playerLaunchBoxSize);
    }
}
