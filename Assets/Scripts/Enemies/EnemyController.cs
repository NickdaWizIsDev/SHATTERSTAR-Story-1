using System;
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
        
            // TODO: Trigger a hit frame and/or stun here

            if (health <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // TODO: Handle death (play animation, drop particles, destroy object)
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