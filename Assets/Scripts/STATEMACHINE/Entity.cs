using System;
using System.Collections.Generic;
using UnityEngine;
using HSM;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public AnimationManager animationManager;
    internal StateMachine stateMachine;
    protected State CurrentState => stateMachine.currentState;
    public Action<Entity> OnDeath { get; set; }

    [SerializeField] private List<State> availableStates = new List<State>();
}