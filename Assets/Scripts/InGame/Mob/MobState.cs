using UnityEngine;

public abstract class MobState : State
{
    protected DefaultMob defaultMob;
    public override void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        base.InitState(stateMachine, defaultMob);
        this.defaultMob = defaultMob as DefaultMob;
    }
}
