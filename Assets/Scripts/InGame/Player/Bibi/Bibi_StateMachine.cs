using UnityEngine;

public class Bibi_StateMachine : PlayerStateMachine
{
    public Player_BlinkState player_BlinkStateTemplate;
    public Player_StoneState player_StoneStateTemplate;
    [HideInInspector] public Player_BlinkState player_BlinkState;
    [HideInInspector] public Player_StoneState player_StoneState;

    public override void InitStates()
    {
        base.InitStates();
        InitStateFromBase(player_BlinkStateTemplate, out player_BlinkState);
        InitStateFromBase(player_StoneStateTemplate, out player_StoneState);
    }
    public override void Brain(string req = "")
    {
        base.Brain(req);
        switch (currentState)
        {
            case Player_BlinkState:
                if (req == "BlinkEnd") ChangeState(player_LocomotionState);
            break;
            case Player_StoneState:
                if (req == "StoneEnd") ChangeState(player_LocomotionState);
            break;
        }

    }
    public override void InputRequest(string req = "")
    {
        base.InputRequest(req);
        if(req == "BlinkState") ChangeState(player_BlinkState);
        else if(req == "StoneState") ChangeState(player_StoneState, true);

    }
    
    
}
