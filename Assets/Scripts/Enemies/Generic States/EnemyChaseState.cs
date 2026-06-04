using HSM;
using UnityEngine;

namespace Enemies
{
    public class EnemyChaseState : State
    {
        private EnemyController enemy;

        public EnemyChaseState(Entity entity) : base(entity)
        {
            enemy = (EnemyController)entity;
            stateName = "EnemyChase";
        }

        public override void Enter()
        {
        
        }

        public override void Do()
        {
            if (enemy.playerTransform is null) return;

            var distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.playerTransform.position);

            // Check if player escaped detection range
            if (distanceToPlayer > enemy.detectionRadius)
            {
                enemy.stateMachine.ChangeStateTo<EnemyIdleState>();
                return;
            }

            // Stop moving if close enough to attack
            if (distanceToPlayer <= enemy.attackRange)
            {
                enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
                // In the future, trigger stateMachine.ChangeStateTo<EnemyAttackState>() here
                return;
            }

            // Move towards player
            var direction = Mathf.Sign(enemy.playerTransform.position.x - enemy.transform.position.x);
            enemy.body.linearVelocity = new Vector2(direction * enemy.moveSpeed, enemy.body.linearVelocityY);

            enemy.transform.localRotation = direction switch
            {
                // Flip sprite facing direction
                > 0 => new Quaternion(0, 0, 0, 1),
                < 0 => new Quaternion(0, 180, 0, 1),
                _ => enemy.transform.localRotation
            };
        }

        public override void Exit()
        {
        
        }
    }
}