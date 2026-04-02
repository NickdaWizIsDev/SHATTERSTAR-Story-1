using System;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public State[] availableStates;

    public void ChangeState(State newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }
}