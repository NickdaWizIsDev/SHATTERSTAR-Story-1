using System;
using UnityEngine;
using Assets.Scripts.Statemachine;
using TMPro;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerController : Entity, IDamageable
    {
        [Header("Debug Stuff")] 
        [SerializeField] private TextMeshProUGUI debugStateText;

        [Header("References")]
        [SerializeField] internal PlayerMovement movement;
        [SerializeField] internal PlayerAnimations animations;

        private int health = 100;
        private PlayerStates playerStates;
        private PlayerInputActions inputActions;

        void Awake()
        {
            playerStates = new PlayerStates(this);
            
            inputActions = new PlayerInputActions();

            inputActions.Gameplay.Jump.started += movement.OnJump;
            inputActions.Gameplay.Jump.canceled += movement.OnJump;

            inputActions.Gameplay.Move.performed += movement.OnMove;
            inputActions.Gameplay.Move.canceled += movement.OnMove;

            inputActions.Gameplay.Dash.started += movement.OnDash;

            inputActions.Gameplay.Attack.started += OnAttack;
        }

        private void OnEnable() => inputActions.Gameplay.Enable();
        private void OnDisable() => inputActions.Gameplay.Disable();

        void Start()
        {
            stateMachine.ChangeStateTo<PlayerIdleState>();
            movement.controller = this;
        }
        void FixedUpdate()
        {
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

            if(movement.movementVector.x > 0)
            {
                transform.localRotation = new Quaternion(0, 0, 0, 1);
            }
            else if(movement.movementVector.x < 0)
            {
                transform.localRotation = new Quaternion(0, 180, 0, 1);
            }

            animationManager.SetBool("OnGround", movement.touching.Ground);
        }

        public void DamageThis(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                // Handle player death
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {            
            stateMachine.ChangeStateTo<PlayerAttackState>();
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
