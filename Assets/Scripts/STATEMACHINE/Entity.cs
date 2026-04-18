using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Statemachine;
public class Entity : MonoBehaviour
{
    public AnimationManager animationManager;
    internal StateMachine stateMachine;
    protected State CurrentState => stateMachine.currentState;

    [SerializeField] private List<State> availableStates = new List<State>();
}