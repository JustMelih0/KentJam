using UnityEngine;

public class Bear_StateMachine : MobStateMachine
{
    public Bear_SleepState bear_SleepStateTemplate;
    public Bear_AttackState bear_AttackStateTemplate;
    [HideInInspector] public Bear_AttackState bear_AttackState;
    [HideInInspector] public Bear_SleepState bear_SleepState;
    public override void InitStates()
    {
        base.InitStates();
        InitStateFromBase(bear_SleepStateTemplate, out bear_SleepState);
        InitStateFromBase(bear_AttackStateTemplate, out bear_AttackState);
    }
    public override void Start()
    {
        base.Start();
        ChangeState(bear_SleepState);
        bear_SleepState.Sleep();
    }
    public override void Brain(string req = "")
    {
        base.Brain(req);
        switch (currentState)
        {
            case Bear_SleepState:
                if(req == "Wake")
                bear_SleepState.Wake();
                if(req == "NearTarget")
                    ChangeState(bear_AttackState);
            break;

            case Bear_AttackState:
                if(req == "AttackEnd")
                {
                    ChangeState(bear_SleepState);
                }
            break;
        }
    }
}
