using System;
using UnityEngine;
using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    public class PlayerController : Entity
    {
        [SerializeField] internal PlayerMovement movement;
        [SerializeField] internal PlayerAnimations animations;
        PlayerStates playerStates;

        void Awake()
        {
            playerStates = new PlayerStates(this);
        }

        void Start()
        {
            // stateMachine.ChangeState<PlayerIdleState>();
        }
        void FixedUpdate()
        {
            // Calling movement functions here, and not on the movement script, so I can later make it depend on the player's state
            if(movement.movementVector != Vector2.zero)
            {
                movement.Move();
            }
            else movement.Stop();
        }

        void Update()
        {
            CurrentState.RecursiveDo();

            // PENDING: show the current state (if it has a substate, add "." and the substate's name)

            // --- STATE MANAGEMENT --- //
            PickState();
        }

        private void PickState()
        {
            Debug.Log("Current State: " + stateMachine.currentState.RecursiveStateString());
            if(movement.body.linearVelocity != Vector2.zero)
            {
                // Change to Moving States
            }
            else
            {
                // Change to Idle State
            }
        }
    }

    [System.Serializable]
    internal class PlayerAnimations
    {
        // This is meant to be used as a reference for the player's animations. I don't wanna have to go into the animator every time 
        // I wanna check the name of an animation clip.
        public AnimationClip IdleAnimation;
        public AnimationClip RunAnimation;
        public AnimationClip JumpAnimation;
        public AnimationClip AirHangAnimation;
        public AnimationClip FallAnimation;
        public AnimationClip FallLoopAnimation;
        public AnimationClip GroundAttackAnimation_1;
        public AnimationClip GroundAttackAnimation_2;
        public AnimationClip AirAttackAnimation;        // Not Animated Yet
        public AnimationClip MovingAttackAnimation;     // Not Animated Yet
        public AnimationClip DashAnimation;
    }
}
