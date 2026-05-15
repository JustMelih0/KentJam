using UnityEngine;

[CreateAssetMenu(fileName = "Mob_IdleState", menuName = "SO/States/Mob/IdleState", order = 0)]
public class Mob_IdleState : MobState
{
    public override void EnterState()
    {
        mob.anim.SetTrigger("LocomotionState");
    }

    public override void Execute()
    {
        base.Execute();
        if (defaultMob.IsTargetInViewRange())
        {
            machine.StateRequest("FindTarget");
        }    
    }


    public override void ExitState()
    {
     
    }

    public override void PhysicExecute()
    {
    }
}
