using System;
using HSM;
using UnityEngine;

namespace Enemies
{
    public class EnemyController : Entity, IDamageable
    {
        [Header("Enemy Stats")]
        public int health = 30;
        public float moveSpeed = 4f;
        public float detectionRadius = 8f;
        public float attackRange = 1.5f;

        [Header("References")]
        public Rigidbody2D body;
        public Transform playerTransform; // Assign in inspector or find via tag in Start

        private void Awake()
        {
            stateMachine = new StateMachine();
        
            // Add enemy states
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyChaseState(this)
            );
        }

        private void Start()
        {
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            stateMachine.ChangeStateTo<EnemyIdleState>();
        }

        private void Update()
        {
            CurrentState?.RecursiveDo();
        }

        public void DamageThis(int damage)
        {
            health -= damage;
        
            // Optional: Trigger a hit animation or hit-stop logic here

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Handle death (play animation, drop particles, destroy object)
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}