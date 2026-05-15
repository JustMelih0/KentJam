using UnityEngine;

public class AttackableMobStateMachine : MobStateMachine
{
    public Mob_IdleState mob_IdleStateTemplate;
    public Mob_ChaseState mob_ChaseStateTemplate;
    public Mob_AttackState mob_AttackStateTemplate;

    [HideInInspector] public Mob_IdleState mob_IdleState;
    [HideInInspector] public Mob_ChaseState mob_ChaseState;
    [HideInInspector] public Mob_AttackState mob_AttackState;
    public override void InitStates()
    {
        base.InitStates();
        InitStateFromBase(mob_IdleStateTemplate, out mob_IdleState);
        InitStateFromBase(mob_ChaseStateTemplate, out mob_ChaseState);
        InitStateFromBase(mob_AttackStateTemplate, out mob_AttackState);
    }
    public override void Start()
    {
        base.Start();
        ChangeState(mob_IdleState);
    }
    public override void Brain(string req = "")
    {
        switch (currentState)
        {
            case Mob_IdleState:
                IdleControl(req);
            break;

            case Mob_ChaseState:
                ChaseControl(req);

            break;

            case Mob_AttackState:
                AttackControl(req);
            break;
        }
    }
    protected virtual void IdleControl(string req = "")
    {
        if (req == "FindTarget")
        {
            ChangeState(mob_ChaseState);
        }
        
    }
    protected virtual void AttackControl(string req = "")
    {
        if (req == "AttackEnd")
        {
            ChangeState(mob_IdleState);
        }
    }
    protected virtual void ChaseControl(string req = "")
    {
        if (req == "TargetLost")
        {
            ChangeState(mob_IdleState);
        }
        else if(req == "NearTheTarget" && mob_AttackState.canAttack)
        {
            ChangeState(mob_AttackState);
        }
    }
}
