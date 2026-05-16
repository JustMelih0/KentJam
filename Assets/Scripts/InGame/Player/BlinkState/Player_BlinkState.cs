using UnityEngine;

[CreateAssetMenu(fileName = "Player_BlinkState", menuName = "SO/States/Player/BlinkState", order = 0)]
public class Player_BlinkState : PlayerState
{
    public Projectile projectilePrefab;
    public float projectileSpeed = 10f;

    public override void EnterState()
    {
        base.EnterState();
        mob.ResetAnimatorTriggers();
        machine.canTransationState = false;
        mob.anim.SetTrigger("BlinkState");
    }
    public override void PhysicExecute()
    {
        playerStateMachine.player_LocomotionState.PhysicExecute();
    }

    public void BlinkStart()
    {
        
        Vector2 spawnPosition = player.attackPoint != null
            ? player.attackPoint.position
            : player.transform.position;

        Vector2 targetPosition = playerStateMachine.hasPointerWorldPosition
            ? playerStateMachine.lastPointerWorldPosition
            : spawnPosition + Vector2.right * player.facingRight;

        player.FaceToTarget(targetPosition);
    }
    public void Blink()
    {
        if (projectilePrefab == null) return;

        Vector2 spawnPosition = player.attackPoint != null
            ? player.attackPoint.position
            : player.transform.position;

        Vector2 targetPosition = playerStateMachine.hasPointerWorldPosition
            ? playerStateMachine.lastPointerWorldPosition
            : spawnPosition + Vector2.right * player.facingRight;

        Vector2 direction = targetPosition - spawnPosition;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
            direction = Vector2.right * player.facingRight;

        player.FaceToTarget(targetPosition);

        Projectile projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.SetupProjectile(direction, projectileSpeed);
    }
    public void BlinkEnd()
    {
        machine.canTransationState = true;
        machine.StateRequest("BlinkEnd");
    }
    public override void AnimationEvent(string actionName)
    {
        base.AnimationEvent(actionName);
        if (actionName == "Blink") Blink();
        else if(actionName == "BlinkEnd") BlinkEnd();
        else if(actionName == "BlinkStart") BlinkStart();
    }
}
