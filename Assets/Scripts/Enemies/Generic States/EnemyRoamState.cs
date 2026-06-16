using HSM;
using UnityEngine;

namespace Enemies
{
    public class EnemyRoamState : State
    {
        private EnemyController enemy;
        private float startX;
        private float roamDirection;
        private float currentRoamDistance;

        public EnemyRoamState(Entity entity, float direction) : base(entity)
        {
            enemy = (EnemyController)entity;
            stateName = "Roaming";
            roamDirection = direction;
        }

        public override void Enter()
        {
            enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
            enemy.animationManager.PlayAnimation(enemy.animations.MoveAnimation);
            
            startX = enemy.transform.position.x;
            currentRoamDistance = RandomRoamDistance();
            UpdateRotation();
        }

        public override void Do()
        {
            enemy.body.linearVelocity = new Vector2(roamDirection * enemy.moveSpeed, enemy.body.linearVelocityY);

            Vector2 pos = enemy.transform.position;
            float boundsOffset = 1f;

            RaycastHit2D wallHit = Physics2D.Raycast(pos + new Vector2(roamDirection * boundsOffset, 0), Vector2.right * roamDirection, 0.1f);
            RaycastHit2D groundHit = Physics2D.Raycast(pos + new Vector2(roamDirection * boundsOffset, 0), Vector2.down, 2f);

            bool hitWall = wallHit.collider && !wallHit.collider.isTrigger;
            bool noGround = !groundHit.collider;

            if (hitWall || noGround)
            {
                roamDirection *= -1f;
                startX = enemy.transform.position.x;
                currentRoamDistance = RandomRoamDistance();
                UpdateRotation();
            }
            else if (Mathf.Abs(enemy.transform.position.x - startX) >= currentRoamDistance)
            {
                enemy.stateMachine.ChangeStateTo<EnemyIdleState>();
            }
        }

        public override void Exit()
        {
            enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
        }

        private static float RandomRoamDistance()
        {
            return Random.Range(1f, 10f);
        }

        private void UpdateRotation()
        {
            enemy.transform.localRotation = roamDirection switch
            {
                > 0 => new Quaternion(0, 0, 0, 1),
                < 0 => new Quaternion(0, 180, 0, 1),
                _ => enemy.transform.localRotation
            };
        }
    }
}