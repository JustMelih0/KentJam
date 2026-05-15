using UnityEngine;

public class Bibi_StateMachine : PlayerStateMachine
{
    public Player_BlinkState player_BlinkStateTemplate;
    [HideInInspector] public Player_BlinkState player_BlinkState;

    public override void InitStates()
    {
        base.InitStates();
        InitStateFromBase(player_BlinkStateTemplate, out player_BlinkState);
    }
    public override void Brain(string req = "")
    {
        base.Brain(req);
        switch (currentState)
        {
            case Player_BlinkState:
                if (req == "BlinkEnd") ChangeState(player_LocomotionState);
            break;
        }

    }
    public override void InputRequest(string req = "")
    {
        base.InputRequest(req);
        if(req == "BlinkState") ChangeState(player_BlinkState);

    }
    
    
}
