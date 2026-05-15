using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public bool canTransationState = true;
    public bool thinkJustRequest = false;

    protected List<State> allStates = new();

    public State currentState;

    protected virtual void Awake()
    {

    }
    public virtual void Start(){ InitStates();}
    public virtual void InitStates(){}
    public abstract void InitStateFromBase<T>(T stateTemplate, out T state) where T : State;
    public virtual void OnEnable()
    {
        Invoke(nameof(StateEnabledWithDelay), 0.1f);
    }
    public void StateEnabledWithDelay()
    {
        foreach (State item in allStates)
        {
            item.Enable();
        }
    }
    public virtual void OnDisable()
    {
        CancelInvoke(nameof(StateEnabledWithDelay));
        foreach (State item in allStates)
        {
            item.Disable();
        }
    }

    protected virtual void Update()
    {
        currentState?.Execute();

        if(!thinkJustRequest)
            Brain();
    }
    public virtual void Brain(string req = "")
    {

    }
    public void StateRequest(string req)
    {
        Brain(req);
    }
    void FixedUpdate()
    {
        currentState?.PhysicExecute();
    }
    public virtual void ChangeState(State newState, bool force = false)
    {
        if (canTransationState == false && force == false) return;
         
        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }

    void OnDrawGizmos()
    {
        currentState?.DebugGizmos();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        currentState?.TriggerEnter(other);
    }


}
