using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player_LocomotionState", menuName = "SO/States/Player/LocomotionState", order = 0)]
public class Player_LocomotionState : PlayerState
{
    public float jumpForce = 5f;
    [Range(0, 5)] public float gravityMultiplier = 1;

    [Header("Dust Effect")]
    public float dustCooldown = 0.2f;

    private float defaultGravity = 1;

    public override void EnterState()
    {
        base.EnterState();
        mob.ResetAnimatorTriggers();
        mob.anim.SetTrigger("LocomotionState");
    }

    public override void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        base.InitState(stateMachine, defaultMob);
        defaultGravity = player.rgb2d.gravityScale;
    }

    public override void Execute()
    {
        base.Execute();
        player.FaceToInput();
        player.anim.SetFloat("VerticalSpeed", player.rgb2d.linearVelocityY);
    }

    public override void ExitState()
    {
        base.EnterState();
        player.rgb2d.linearVelocityX = 0;
    }

    public override void PhysicExecute()
    {
        if (mob?.mob_HealthBase?.isHitting == true)
        {
            GravityControl();
            return;
        }

        player.rgb2d.linearVelocityX = player.horizontalInput * player.stats.moveSpeed;

        if (player.transform.position.y < -15f)
            mob.mob_HealthBase.TakeDamage(1000, mob.transform.position);

        GravityControl();
    }

    protected virtual void GravityControl()
    {
        if (player.rgb2d.linearVelocityY < 0 && player.rgb2d.gravityScale != (defaultGravity * gravityMultiplier))
            player.rgb2d.gravityScale = (defaultGravity * gravityMultiplier);
        else if (player.rgb2d.gravityScale != defaultGravity)
            player.rgb2d.gravityScale = defaultGravity;
    }

    public override void InputRequest(PlayerInputController.InputType inputType, InputAction.CallbackContext context)
    {
        base.InputRequest(inputType, context);

        if (isStateActive)
        {
            if (inputType == PlayerInputController.InputType.JumpInput && context.started)
            {
                if (player.IsGrounded() || player.CanJumpWithCoyoteTime())
                {
                    player.rgb2d.linearVelocityY = jumpForce;
                    player.ConsumeCoyoteTime();
                }
            }
        }
    }
}
