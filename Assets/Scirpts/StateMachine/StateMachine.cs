using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }
    private bool canChangeState = true;

    public void Init(EntityState startState)
    {
        currentState = startState;
        currentState.Enter();
    }
    
    public void changeState(EntityState newState)
    {
        if (!canChangeState) return;
        
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }
    
    public void SwitchOffStateMachine() => canChangeState = false;
}
