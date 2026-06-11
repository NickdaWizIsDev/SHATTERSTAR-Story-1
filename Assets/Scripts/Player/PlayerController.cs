using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : Entity, IDamageable
    {
        [Header("Debug Stuff")] 
        [SerializeField] private TextMeshProUGUI debugStateText;

        [Header("References")]
        [SerializeField] internal PlayerMovement movement;
        [SerializeField] internal PlayerAnimations animations;
        [SerializeField] internal PlayerCombat combat;
        [SerializeField] internal Collider2D hurtbox;
        [SerializeField] internal SpriteRenderer spriteRenderer;

        [Header("Variables")]
        [SerializeField] internal bool canMove = true;
        [SerializeField] private int health = 100;
        [SerializeField] private float iFrameDuration = 1.5f;
        [SerializeField] private float flashInterval = 0.1f;
        [SerializeField] internal float dashCooldown = 0.75f;
        [SerializeField] internal float dashCdTimer;
        private bool isInvincible;
        
        [UsedImplicitly] private PlayerStates playerStates;
        private PlayerInputActions inputActions;

        private void Awake()
        {
            GameManager.Instance.InitializePlayer(this);
            
            playerStates = new PlayerStates(this);
            
            inputActions = new PlayerInputActions();

            inputActions.Gameplay.Jump.started += movement.OnJump;
            inputActions.Gameplay.Jump.canceled += movement.OnJump;

            inputActions.Gameplay.Move.performed += movement.OnMove;
            inputActions.Gameplay.Move.canceled += movement.OnMove;

            inputActions.Gameplay.Dash.started += OnDash;

            inputActions.Gameplay.Attack.started += OnAttack;

            inputActions.Gameplay.Interact.started += OnInteract;

            movement ??= GetComponent<PlayerMovement>();
            animations ??= GetComponent<PlayerAnimations>();
            combat ??= GetComponent<PlayerCombat>();
            spriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
        }
        
        public void EnableInput() => inputActions.Gameplay.Enable();
        public void DisableInput() => inputActions.Gameplay.Disable();

        private void OnEnable() => EnableInput();
        private void OnDisable() => DisableInput();

        private void Start()
        {
            stateMachine.ChangeStateTo<PlayerIdleState>();
            movement.controller = this;
        }

        private void FixedUpdate()
        {
            // If we can't move, force a stop and exit early so we don't apply movement
            if (!canMove)
            {
                movement.Stop();
                return;
            }

            if (movement.movementVector == Vector2.zero)
                movement.Stop();
            else 
                movement.Move();
        }

        private void Update()
        {
            CurrentState.RecursiveDo();

            if (debugStateText is not null)
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

            if (dashCdTimer > 0)
            {
                dashCdTimer -= Time.deltaTime;
            }

            animationManager.SetBool("OnGround", movement.touching.Ground);
        }

        public void DamageThis(int damage)
        {
            if (isInvincible) return;

            health -= damage;
            
            if(health <= 0)
            {
                // Handle player death
            }
            else
            {
                StartCoroutine(IFrameRoutine());
            }
        }

        private IEnumerator IFrameRoutine()
        {
            isInvincible = true;
            float elapsed = 0f;
            bool isVisible = true;

            while (elapsed < iFrameDuration)
            {
                isVisible = !isVisible;
                if (spriteRenderer != null) 
                {
                    Color c = spriteRenderer.color;
                    c.a = isVisible ? 1f : 0f;
                    spriteRenderer.color = c;
                }
                
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            if (spriteRenderer != null) 
            {
                Color c = spriteRenderer.color;
                c.a = 0f;
                spriteRenderer.color = c;
            }
            isInvincible = false;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            // Attack cannot interrupt an ongoing dash
            if (stateMachine.currentState is PlayerDashingState) return;

            // If we are ALREADY attacking, pass the input down to the active attack state for combos
            if (stateMachine.currentState is PlayerAttackState)
            {
                var activeAttack = stateMachine.currentState as PlayerAttackState;
                activeAttack?.StartAttack();
                return;
            }

            // Otherwise, start a brand-new attack
            stateMachine.ChangeStateTo<PlayerAttackState>();
            var newAttack = stateMachine.currentState as PlayerAttackState;
            newAttack?.StartAttack();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            // Dash can interrupt ANYTHING (even attacks!) except itself
            if (stateMachine.currentState is PlayerDashingState) return;
            if (dashCdTimer > 0) return;
            
            stateMachine.ChangeStateTo<PlayerDashingState>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            OnInteractEvent?.Invoke();
        }

        public event Action OnInteractEvent;
    }

    [Serializable]
    internal class PlayerAnimations
    {
        // This is meant to be used as a reference for the player's animations. I don't wanna have to go into the
        // animator every time I wanna check the name of an animation clip.
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