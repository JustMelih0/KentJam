
using System;
using Unity.VisualScripting;
using UnityEngine;

public class MobStateMachine : StateMachine
{
    [HideInInspector]public Mob mob;
    protected bool mobDead = false;

    public void AnimationEvent(string eventName)
    {
        currentState?.AnimationEvent(eventName);
    }
    

    protected override void Awake()
    {
        mob = GetComponent<Mob>();
        base.Awake();

    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void OnDisable()
    {
        base.OnDisable();
    }
    protected virtual void MobDead()
    {
        mobDead = true;
    }
    public override void InitStateFromBase<T>(T stateTemplate, out T state)
    {
        state = Instantiate(stateTemplate);
        state.InitState(this, mob);
        allStates.Add(state);
    }

}
