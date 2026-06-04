using System.Collections.Generic;
using UnityEngine;
using HSM;
public class Entity : MonoBehaviour
{
    public AnimationManager animationManager;
    internal StateMachine stateMachine;
    protected State CurrentState => stateMachine.currentState;

    [SerializeField] private List<State> availableStates = new List<State>();
}