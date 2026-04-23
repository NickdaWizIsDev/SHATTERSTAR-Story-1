using System;
using UnityEngine;
using Assets.Scripts.Statemachine;
using TMPro;

namespace Assets.Scripts.Player
{
    public class PlayerController : Entity, IDamageable
    {
        [Header("Debug Stuff")] 
        [SerializeField] private TextMeshProUGUI debugStateText;

        [Header("References")]
        [SerializeField] internal PlayerMovement movement;
        [SerializeField] internal PlayerAnimations animations;

        [Header("Stats")]
        [SerializeField] private int health = 100;
        PlayerStates playerStates;

        void Awake()
        {
            playerStates = new PlayerStates(this);
        }

        void Start()
        {
            stateMachine.ChangeState<PlayerIdleState>();
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

            if (debugStateText != null)
            {
                debugStateText.text = stateMachine.currentState.RecursiveStateString();
            }

            // --- STATE MANAGEMENT --- //
            PickState();
        }

        private void PickState()
        {
            switch (CurrentState)
            {
                case PlayerIdleState:
                    if (movement.CurrentVelocity != Vector2.zero)
                    {
                        stateMachine.ChangeState<PlayerMovingState>();
                    }
                    break;
                case PlayerMovingState:
                    if (movement.CurrentVelocity == Vector2.zero)
                    {
                        stateMachine.ChangeState<PlayerIdleState>();
                    }
                    break;
            }
        }

        public void DamageThis(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                // Handle player death
            }
        }
    }

    [Serializable]
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
