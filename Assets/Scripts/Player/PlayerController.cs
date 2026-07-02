using System;
using System.Collections;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : Entity, IDamageable
    {
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        [Header("Debug Stuff")] 
        [SerializeField] private TextMeshProUGUI debugStateText;

        [Header("References")]
        [SerializeField] internal PlayerMovement movement;
        [SerializeField] internal PlayerAnimations animations;
        [SerializeField] internal PlayerCombat combat;
        [SerializeField] internal Collider2D hurtbox;
        [SerializeField] internal SpriteRenderer[] spriteRenderers;

        [Header("Variables")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int health = 100;
        public float CurrentHealthPct => (float)health / maxHealth;

        [SerializeField] private float iFrameDuration = 1.5f;
        [SerializeField] private float flashInterval = 0.1f;
        [SerializeField] internal float dashCooldown = 0.75f;
        internal float dashCdTimer;
        internal bool isRunning;
        
        [Header("Knockback Settings")]
        [SerializeField] private float knockbackForce = 12f;
        [SerializeField] private float knockbackDuration = 0.2f;
        private bool isKnockedBack;
        
        [Header("Audio")]
        [SerializeField] internal AudioResource landAudio;
        [SerializeField] internal AudioResource hitAudio;
        
        [Header("Input Buffering")]
        internal float attackBufferTimer;
        [SerializeField] private float attackBufferWindow = 0.2f;
        
        private bool isInvincible;

        public event Action<float> OnHealthPctChanged = _ => {};
        
        [UsedImplicitly] private PlayerStates playerStates;
        private PlayerInputActions inputActions;
        public bool CanMove { get; set; }
        private MaterialPropertyBlock mpb;

        private void Awake()
        {
            GameManager.Instance.InitializePlayer(this);
            
            playerStates = new PlayerStates(this);
            
            inputActions = new PlayerInputActions();

            inputActions.Gameplay.Jump.started += movement.OnJump;
            inputActions.Gameplay.Jump.canceled += movement.OnJump;

            inputActions.Gameplay.Move.performed += movement.OnMove;
            inputActions.Gameplay.Move.canceled += movement.OnMove;

            inputActions.Gameplay.Run.performed += _ => isRunning = true;
            inputActions.Gameplay.Run.canceled += _ => isRunning = false;

            inputActions.Gameplay.Dash.started += OnDash;

            inputActions.Gameplay.Attack.started += OnAttack;

            inputActions.Interactions.Interact.started += OnInteract;

            movement ??= GetComponent<PlayerMovement>();
            animations ??= GetComponent<PlayerAnimations>();
            combat ??= GetComponent<PlayerCombat>();
            spriteRenderers ??= GetComponentsInChildren<SpriteRenderer>();
            mpb = new MaterialPropertyBlock();
        }

        public void EnableInput()
        {
            inputActions.Gameplay.Enable();
        }

        public void DisableInput()
        {
            // Disables all movement, jumping, dashing, and attacking inputs.
            inputActions.Gameplay.Disable(); 
    
            movement.movementVector = Vector2.zero; 
            isRunning = false;
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
            CanMove = true;
            stateMachine.ChangeStateTo<PlayerIdleState>();
            movement.Controller = this;
            health = maxHealth;
            OnHealthPctChanged?.Invoke((float)health / maxHealth);
        }

        private void FixedUpdate()
        {
            if (isKnockedBack) return;

            if (movement.movementVector == Vector2.zero)
                movement.Stop();
            else 
                movement.Move();
        }

        private void Update()
        {
            if (isDead) return;
            if (attackBufferTimer > 0)
            {
                attackBufferTimer -= Time.deltaTime;
            }

            if (attackBufferTimer > 0 && stateMachine.currentState is not PlayerAttackState && stateMachine.currentState is not PlayerDashingState)
            {
                attackBufferTimer = 0f; // Consume the buffer
                stateMachine.ChangeStateTo<PlayerAttackState>();
                (stateMachine.currentState as PlayerAttackState)?.StartAttack();
            }
            
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
            
            OnHealthPctChanged.Invoke((float)health / maxHealth);
            
            if(health <= 0)
            {
                stateMachine.ChangeStateTo<PlayerDeathState>();
            }
            else
            {
                StartCoroutine(IFrameRoutine());
                StartCoroutine(KnockbackRoutine(damageSourcePos));
                GameManager.Instance.TriggerHitStop(0.08f);
                CameraEffects.Instance.Shake(0.1f, 1f);
                if(hitAudio) PlaySFX(hitAudio);
            }
        }
        
        private IEnumerator KnockbackRoutine(Vector2 damageSourcePos)
        {
            isKnockedBack = true;
            DisableInput();

            var knockbackDir =
                // Push away from the damage source
                damageSourcePos != default ? ((Vector2)transform.position - damageSourcePos).normalized :
                // Failsafe: If no source is provided (like environmental hazards), push them backwards based on where they are facing
                new Vector2(-Mathf.Sign(transform.localScale.x), 0);

            knockbackDir.y = 0.5f; // A slight upward "pop" to juggle the player

            if (movement.body)
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
            var elapsed = 0f;
            var isVisible = true;

            while (elapsed < iFrameDuration)
            {
                isVisible = !isVisible;
                var alpha = isVisible ? 1f : 0f;
                
                foreach (var sr in spriteRenderers)
                {
                    if (sr != null)
                    {
                        sr.GetPropertyBlock(mpb);
                        mpb.SetFloat(Alpha, alpha);
                        sr.SetPropertyBlock(mpb);
                    }
                }
                
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            foreach (var sr in spriteRenderers)
            {
                if (sr != null)
                {
                    sr.GetPropertyBlock(mpb);
                    mpb.SetFloat(Alpha, 1f);
                    sr.SetPropertyBlock(mpb);
                }
            }
            isInvincible = false;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (stateMachine.currentState is PlayerDashingState || isDead) return;
            if (DialogueManager.Instance.IsPlaying) return;
            
            attackBufferTimer = attackBufferWindow;
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (stateMachine.currentState is PlayerDashingState || isDead) return;
            if (DialogueManager.Instance.IsPlaying) return;
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
        public AnimationClip DeathAnimation;
    }
}