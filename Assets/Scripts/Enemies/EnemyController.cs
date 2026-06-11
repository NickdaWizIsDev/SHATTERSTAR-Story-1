using System;
using HSM;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : Entity, IDamageable
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

        private void Awake()
        {
            stateMachine = new StateMachine();
        
            // Add enemy states
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyChaseState(this),
                new EnemyAttackState(this)
            );
        }

        private void Start()
        {
            if (playerTransform == null)
            {
                var player = GameManager.Instance.Player;
                playerTransform = player.transform;
            }

            stateMachine.ChangeStateTo<EnemyIdleState>();
        }

        private void Update()
        {
            CurrentState?.RecursiveDo();

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }

        public void DamageThis(int damage)
        {
            health -= damage;
        
            // TODO: Trigger a hit frame and/or stun here

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // TODO: Handle death (play animation, drop particles, destroy object)
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        public void ResetAttackCD()
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