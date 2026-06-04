using HSM;
using UnityEngine;

namespace Enemies
{
    public class EnemyIdleState : State
    {
        private EnemyController enemy;

        public EnemyIdleState(Entity entity) : base(entity)
        {
            enemy = (EnemyController)entity;
            stateName = "EnemyIdle";
        }

        public override void Enter()
        {
            enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
        }

        public override void Do()
        {
            if (enemy.playerTransform is null) return;

            var distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.playerTransform.position);

            if (distanceToPlayer <= enemy.detectionRadius)
            {
                enemy.stateMachine.ChangeStateTo<EnemyChaseState>();
            }
        }

        public override void Exit()
        {
        
        }
    }
}