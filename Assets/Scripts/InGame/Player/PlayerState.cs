using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerState : State
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    public override void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        base.InitState(stateMachine, defaultMob);
        player = defaultMob as Player;
        playerStateMachine = stateMachine as PlayerStateMachine;
    }
    public virtual void InputRequest(PlayerInputController.InputType inputType,InputAction.CallbackContext context){}
}
