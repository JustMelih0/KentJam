using UnityEngine;

[CreateAssetMenu(fileName = "Player_StoneState", menuName = "SO/States/Player/StoneState", order = 0)]
public class Player_StoneState : PlayerState
{
    public Projectile stoneProjectile;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float waitBeforeWalk = 1f;
    [SerializeField] private float stopDistance = 0.05f;

    protected Statue statue;
    private bool isWalkingToStatuePoint;
    private bool stoneOutStarted;

    public override void EnterState()
    {
        base.EnterState();
        isWalkingToStatuePoint = false;
        stoneOutStarted = false;
        mob.ResetAnimatorTriggers();
        mob.anim.SetTrigger("StoneState");

        statue = GameManager.Instance.statue;
        if (statue != null && statue.statueStonePoint != null)
        {
            mob.transform.position = statue.statueStonePoint.position;
        }

        mob.rgb2d.linearVelocity = Vector2.zero;
        mob.rgb2d.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    public override void AnimationEvent(string actionName)
    {
        base.AnimationEvent(actionName);

        if (actionName == "StoneOut")
        {
            if (stoneOutStarted) return;

            stoneOutStarted = true;
            machine.StartCoroutine(StoneOutRoutine());
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        isWalkingToStatuePoint = false;
        mob.rgb2d.linearVelocityX = 0f;
        mob.rgb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void PhysicExecute()
    {
        if (!isWalkingToStatuePoint || statue == null || statue.statueWalkPoint == null) return;

        Vector2 targetPosition = statue.statueWalkPoint.position;
        float distanceX = targetPosition.x - mob.rgb2d.position.x;

        mob.FaceToTarget(targetPosition);

        if (Mathf.Abs(distanceX) <= stopDistance)
        {
            isWalkingToStatuePoint = false;
            mob.rgb2d.linearVelocityX = 0f;
            mob.anim.SetFloat("HorizontalInput", 0f);
            machine.StateRequest("StoneEnd");
            GameManager.Instance.NextScene();
            return;
        }

        mob.rgb2d.linearVelocityX = Mathf.Sign(distanceX) * mob.stats.moveSpeed;
    }

    private System.Collections.IEnumerator StoneOutRoutine()
    {
        if (statue == null) yield break;
        Projectile projectile = null;
        if (stoneProjectile != null && statue.projectilePoint != null)
        {
            Vector3 spawnPosition = mob.attackPoint != null ? mob.attackPoint.position : mob.transform.position;
            projectile = Instantiate(stoneProjectile, spawnPosition, Quaternion.identity);

            yield return MoveProjectileToPoint(projectile.transform, statue.projectilePoint.position);
        }


        Destroy(projectile.gameObject);
        statue.ActivateStatue();

        yield return new WaitForSeconds(waitBeforeWalk);

        mob.rgb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        mob.anim.SetTrigger("LocomotionState");
        mob.anim.SetFloat("HorizontalInput", 1f);
        isWalkingToStatuePoint = true;
    }

    private System.Collections.IEnumerator MoveProjectileToPoint(Transform projectileTransform, Vector3 targetPosition)
    {
        while (projectileTransform != null && Vector2.Distance(projectileTransform.position, targetPosition) > stopDistance)
        {
            Vector2 direction = targetPosition - projectileTransform.position;
            projectileTransform.right = direction;
            projectileTransform.position = Vector2.MoveTowards(
                projectileTransform.position,
                targetPosition,
                projectileSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (projectileTransform != null)
        {
            projectileTransform.position = targetPosition;
        }
    }
}
