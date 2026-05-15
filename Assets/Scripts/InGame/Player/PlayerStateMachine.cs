using UnityEngine;

public class PlayerStateMachine : MobStateMachine
{
    public Player_LocomotionState player_LocomotionStateTemplate;
    [HideInInspector] public Player_LocomotionState player_LocomotionState;
     public override void InitStates()
    {
        base.InitStates();
        InitStateFromBase(player_LocomotionStateTemplate, out player_LocomotionState);
        ChangeState(player_LocomotionState);
    }
    public override void Start()
    {
        base.Start();
        ChangeState(player_LocomotionState);
    }
    public override void Brain(string req = "")
    {
        base.Brain(req);
    }
    public virtual void InputRequest(string req = "")
    {
    }
    public override void ChangeState(State newState, bool force = false)
    {
        base.ChangeState(newState, force);
    }
    
}
