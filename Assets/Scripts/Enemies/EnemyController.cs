using System;
using HSM;
using UnityEngine;
using PrimeTween;
using UnityEngine.Audio;

namespace Enemies
{
    public abstract class EnemyController : Entity, IDamageable
    {
        [Header("Enemy Stats")]
        [SerializeField] internal int health = 30;
        [SerializeField] internal float moveSpeed = 4f;
        [SerializeField] internal float detectionRadius = 8f;
        [SerializeField] internal float attackRange = 1.5f;
        [SerializeField] internal float attackLungeSpeed = 10f;
        [SerializeField] internal int attackDamage = 10;
        [SerializeField] internal float telegraphTime = 0.5f;
        [SerializeField] internal float attackRecoveryTime = 0.5f;
        [SerializeField] internal float attackCooldown;

        internal float attackTimer;

        [Header("References")] 
        [SerializeField] internal Rigidbody2D body;
        [SerializeField] internal EnemyAnimations animations;
        [SerializeField] internal LayerMask obstacleLayer;
        [SerializeField] private GameObject deathFXPrefab;
        
        // GAME JUICE (FEEDBACK)
        [Header("Juice")]
        [SerializeField] internal SpriteRenderer spriteRenderer;
        [SerializeField] private float knockbackForce = 6f;
        [SerializeField] private Color hitFlashColor = Color.red; 
        [SerializeField] private float hitFlashDuration = 0.1f;
        [SerializeField] internal bool isInterruptable = true;
        [ColorUsage(true, true)][SerializeField] internal Color attackGlowColor = Color.white;
        [SerializeField] private AudioResource hitAudio;
        
        private Color originalColor;
        internal Transform playerTransform;

        // Caching the MaterialPropertyBlock to prevent memory leaks
        internal MaterialPropertyBlock propBlock;
        private Tween flashTween;
        public bool doesChase = false;

        protected virtual void Awake()
        {
            stateMachine = new StateMachine();
            InitializeStateMachine();
            propBlock = new MaterialPropertyBlock();
        }

        protected abstract void InitializeStateMachine();

        protected virtual void Start()
        {
            if (playerTransform == null)
            {
                var player = GameManager.Instance.Player;
                if (player != null) playerTransform = player.transform;
            }

            if (spriteRenderer != null) 
            {
                originalColor = spriteRenderer.sharedMaterial.HasProperty("m_Color") 
                    ? spriteRenderer.sharedMaterial.GetColor("m_Color") 
                    : Color.white;
            }
        }

        protected virtual void Update()
        {
            CurrentState?.RecursiveDo();

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }

        // Updated Signature to match the new IDamageable!
        public virtual void DamageThis(int damage, Vector2 damageSourcePos = default)
        {
            health -= damage;
        
            var knockbackDir = Vector2.right;
            
            // Prioritize the actual attack location if provided
            if (damageSourcePos != default)
            {
                knockbackDir = ((Vector2)transform.position - damageSourcePos).normalized;
            }
            // Fallback to the player's general position
            else if (playerTransform is not null)
            {
                knockbackDir = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;
            }

            knockbackDir.y = 0.1f; // Slight aerial push
            
            if (body is not null)
            {
                body.linearVelocity = Vector2.zero; // Reset velocity so the knockback is always consistent
                body.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }

            // Hit flash
            if (spriteRenderer is not null && gameObject.activeInHierarchy)
            {
                flashTween.Stop(); // Stop any existing flash if they get hit rapidly

                // Snap immediately to the flash color
                spriteRenderer.GetPropertyBlock(propBlock);
                propBlock.SetColor("m_Color", hitFlashColor);
                spriteRenderer.SetPropertyBlock(propBlock);

                // Tween smoothly back to the original color
                flashTween = Tween.Custom(hitFlashColor, originalColor, duration: hitFlashDuration, onValueChange: color =>
                {
                    if (spriteRenderer == null) return;
                    spriteRenderer.GetPropertyBlock(propBlock);
                    propBlock.SetColor("m_Color", color);
                    spriteRenderer.SetPropertyBlock(propBlock);
                });
            }
            
            if(hitAudio) PlaySFX(hitAudio);

            if (stateMachine.currentState is not EnemyIdleState && isInterruptable)
            {
                stateMachine.ChangeStateTo<EnemyIdleState>();
            }

            if (health <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Tween.StopAll(spriteRenderer);
            OnDeath?.Invoke(this);
            gameObject.SetActive(false);
            Instantiate(deathFXPrefab, transform.position, Quaternion.identity);  
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public virtual void ResetAttackCD()
        {
            attackTimer = attackCooldown;
        }
    }

    [Serializable]
    public class EnemyAnimations
    {
        public AnimationClip IdleAnimation;
        public AnimationClip MoveAnimation;
        public AnimationClip TelegraphAnimation;
        public AnimationClip AttackAnimation;
    }
}