using System;
using System.Collections;
using JetBrains.Annotations;
using Managers;
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
        [SerializeField] private int health = 100;
        [SerializeField] private float iFrameDuration = 1.5f;
        [SerializeField] private float flashInterval = 0.1f;
        [SerializeField] internal float dashCooldown = 0.75f;
        [SerializeField] internal float dashCdTimer;
        [SerializeField] internal bool isRunning;
        
        [Header("Knockback Settings")]
        [SerializeField] private float knockbackForce = 12f;
        [SerializeField] private float knockbackDuration = 0.2f;
        private bool isKnockedBack;
        
        private bool isInvincible;
        
        [UsedImplicitly] private PlayerStates playerStates;
        private PlayerInputActions inputActions;
        internal bool CanMove = true;
        internal bool CanAttack = true;

        private void Awake()
        {
            GameManager.Instance.InitializePlayer(this);
            
            playerStates = new PlayerStates(this);
            
            inputActions = new PlayerInputActions();

            inputActions.Gameplay.Jump.started += movement.OnJump;
            inputActions.Gameplay.Jump.canceled += movement.OnJump;

            inputActions.Gameplay.Move.performed += movement.OnMove;
            inputActions.Gameplay.Move.canceled += movement.OnMove;

            inputActions.Gameplay.Run.performed += ctx => isRunning = true;
            inputActions.Gameplay.Run.canceled += ctx => isRunning = false;

            inputActions.Gameplay.Dash.started += OnDash;

            inputActions.Gameplay.Attack.started += OnAttack;

            inputActions.Interactions.Interact.started += OnInteract;

            movement ??= GetComponent<PlayerMovement>();
            animations ??= GetComponent<PlayerAnimations>();
            combat ??= GetComponent<PlayerCombat>();
            spriteRenderer ??= GetComponentInChildren<SpriteRenderer>();
        }

        public void EnableInput()
        {
            CanMove = true;
            CanAttack = true;
        }

        public void DisableInput()
        {
            CanMove = false;
            CanAttack = false;
        }

        private void OnEnable()
        {
            EnableInput();
            inputActions.Gameplay.Enable();
            inputActions.Interactions.Enable();
        }

        private void OnDisable()
        {
            DisableInput();
            inputActions.Gameplay.Disable();
            inputActions.Interactions.Disable();
        }

        private void Start()
        {
            stateMachine.ChangeStateTo<PlayerIdleState>();
            movement.Controller = this;
        }

        private void FixedUpdate()
        {
            if (!CanMove)
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

            #if UNITY_EDITOR
            if (debugStateText is not null)
            {
                debugStateText.text = stateMachine.currentState.RecursiveStateString();
            }
            #endif
            
            if(movement.movementVector.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(movement.movementVector.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (dashCdTimer > 0)
            {
                dashCdTimer -= Time.deltaTime;
            }

            animationManager.SetBool("OnGround", movement.touching.Ground);
        }

        public void DamageThis(int damage, Vector2 damageSourcePos = default)
        {
            if (isInvincible) return;

            health -= damage;
            
            if(health <= 0)
            {
            }
            else
            {
                StartCoroutine(IFrameRoutine());
                GameManager.Instance.TriggerHitStop(0.25f);
                CameraEffects.Instance.Shake(0.1f, 1f);
            }
        }
        
        private IEnumerator KnockbackRoutine(Vector2 damageSourcePos)
        {
            isKnockedBack = true;
            DisableInput();
            
            Vector2 knockbackDir;
            if (damageSourcePos != default)
            {
                // Push away from the damage source
                knockbackDir = ((Vector2)transform.position - damageSourcePos).normalized;
            }
            else
            {
                // Failsafe: If no source is provided (like environmental hazards), push them backwards based on where they are facing
                knockbackDir = new Vector2(-Mathf.Sign(transform.localScale.x), 0);
            }

            knockbackDir.y = 0.5f; // A slight upward "pop" to juggle the player

            if (movement.body != null)
            {
                movement.body.linearVelocity = Vector2.zero;
                movement.body.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(knockbackDuration);

            isKnockedBack = false;
            EnableInput(); 
        }

        private IEnumerator IFrameRoutine()
        {
            isInvincible = true;
            float elapsed = 0f;
            bool isVisible = true;
            var mat = Instantiate(spriteRenderer.material);
            spriteRenderer.material = mat;

            while (elapsed < iFrameDuration)
            {
                isVisible = !isVisible;
                if (spriteRenderer != null)
                {
                    var a = mat.GetFloat("_Alpha");
                    var alpha = isVisible ? 1f : 0f;
                    mat.SetFloat("_Alpha", alpha);
                }
                
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            if (spriteRenderer != null) 
            {
                var a = mat.GetFloat("_Alpha");
                mat.SetFloat("_Alpha", 1f);
            }
            isInvincible = false;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (stateMachine.currentState is PlayerDashingState) return;
            if (DialogueManager.Instance.IsPlaying || !CanAttack) return;

            if (stateMachine.currentState is PlayerAttackState)
            {
                var activeAttack = stateMachine.currentState as PlayerAttackState;
                activeAttack?.StartAttack();
                return;
            }

            stateMachine.ChangeStateTo<PlayerAttackState>();
            var newAttack = stateMachine.currentState as PlayerAttackState;
            newAttack?.StartAttack();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (stateMachine.currentState is PlayerDashingState) return;
            if (DialogueManager.Instance.IsPlaying || !CanMove) return;
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
        public AnimationClip IdleAnimation;
        public AnimationClip WalkAnimation;
        public AnimationClip RunAnimation;
        public AnimationClip JumpAnimation;
        public AnimationClip AirHangAnimation;
        public AnimationClip FallAnimation;
        public AnimationClip FallLoopAnimation;
        public AnimationClip GroundAttackAnimation_1;
        public AnimationClip GroundAttackAnimation_2;
        public AnimationClip UpAttackAnimation;
        public AnimationClip DownAttackAnimation;
        public AnimationClip AirAttackAnimation;        
        public AnimationClip MovingAttackAnimation;     
        public AnimationClip DashAnimation;
    }
}