using System;
using System.Collections; // <--- ADD THIS
using HSM;
using UnityEngine;

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
        
        // --- NEW JUICE REFERENCES ---
        [Header("Juice")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float knockbackForce = 6f;
        [SerializeField] private Color hitFlashColor = Color.red; // Pure white or red work best!
        [SerializeField] private float hitFlashDuration = 0.1f;
        
        private Color originalColor;
        internal Transform playerTransform;

        protected virtual void Awake()
        {
            stateMachine = new StateMachine();
            InitializeStateMachine();
        }

        protected abstract void InitializeStateMachine();

        protected virtual void Start()
        {
            if (playerTransform == null)
            {
                var player = GameManager.Instance.Player;
                playerTransform = player.transform;
            }

            // Cache the enemy's original color so we can revert back to it after flashing
            if (spriteRenderer != null) 
            {
                originalColor = spriteRenderer.color;
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

        public virtual void DamageThis(int damage)
        {
            health -= damage;
        
            // 1. JUICE: KNOCKBACK
            // Calculate the direction away from Nick, add a little upward "pop", and apply force.
            if (body != null && playerTransform != null)
            {
                Vector2 knockbackDir = (transform.position - playerTransform.position).normalized;
                knockbackDir.y = 0.5f; // Gives them a slight aerial juggle effect!
                
                body.linearVelocity = Vector2.zero; // Reset velocity so the knockback is always consistent
                body.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }

            // 2. JUICE: HIT FLASH
            // Blink the sprite a bright color for a fraction of a second.
            if (spriteRenderer != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(HitFlashRoutine());
            }

            // 3. JUICE: INTERRUPT / STUN
            // Force the enemy out of their Roam or Attack state so hitting them feels impactful.
            if (stateMachine.currentState is not EnemyIdleState)
            {
                stateMachine.ChangeStateTo<EnemyIdleState>();
            }

            if (health <= 0)
            {
                Die();
            }
        }

        // The Coroutine that handles the flashing visual
        private IEnumerator HitFlashRoutine()
        {
            spriteRenderer.color = hitFlashColor;
            yield return new WaitForSeconds(hitFlashDuration);
            spriteRenderer.color = originalColor;
        }

        protected virtual void Die()
        {
            // TODO: Handle death (play animation, drop particles, destroy object)
            OnDeath?.Invoke(this);
            Destroy(gameObject);
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

        public event Action<EnemyController> OnDeath;
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