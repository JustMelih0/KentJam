using UnityEngine;

public abstract class State : ScriptableObject
{
    protected StateMachine machine;
    protected Mob mob;
    float tickTimer;
    protected virtual float TickInterval => 0.25f;
    protected bool isStateActive = false;
    public virtual void InitState(StateMachine stateMachine, Mob defaultMob)
    {
        machine = stateMachine;
        mob = defaultMob;
    }
    public virtual void EnterState()
    {
        isStateActive = true;
    }
    public virtual void ExitState()
    {
        isStateActive = false;
    }
    public virtual void Execute()
    {
        TickUpdate();
    }
    protected virtual void OnTick() { }
    protected void TickUpdate()
    {
        tickTimer -= Time.deltaTime;
        if (tickTimer <= 0f)
        {
            tickTimer = TickInterval;
            OnTick();
        }
    }
    protected void ResetTick(bool runImmediately = false)
    {
        tickTimer = runImmediately ? 0f : TickInterval;
    }
    public abstract void PhysicExecute();
    public virtual void DebugGizmos(){}
    public virtual void AnimationEvent(string actionName){}
    public virtual void Enable(){}
    public virtual void Disable(){}
    public virtual void CollisionEnter(Collision2D other){}
    public virtual void CollisionExit(Collision2D other){}
    public virtual void TriggerEnter(Collider2D other){}
}
