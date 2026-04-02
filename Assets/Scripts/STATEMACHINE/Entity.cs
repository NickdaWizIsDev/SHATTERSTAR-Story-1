using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public AnimationManager animations;
    internal StateMachine stateMachine;
    protected State CurrentState => stateMachine.currentState;

    [SerializeField] private List<State> availableStates = new List<State>();
}