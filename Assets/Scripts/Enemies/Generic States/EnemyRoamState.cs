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
        private LayerMask obstacleLayer;

        public EnemyRoamState(Entity entity, float direction, LayerMask obs) : base(entity)
        {
            enemy = (EnemyController)entity;
            stateName = "Roaming";
            roamDirection = direction;
            obstacleLayer = obs;
        }

        public override void Enter()
        {
            enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
            enemy.animationManager.PlayAnimation(enemy.animations.MoveAnimation);
            
            // Pick a random direction every time it starts moving
            roamDirection = Random.value > 0.5f ? 1f : -1f;
            
            startX = enemy.transform.position.x;
            currentRoamDistance = RandomRoamDistance();
            
            UpdateFacingDirection();
        }

        public override void Do()
        {
            enemy.body.linearVelocity = new Vector2(roamDirection * enemy.moveSpeed, enemy.body.linearVelocityY);

            Vector2 pos = enemy.transform.position;
            
            float boundsOffset = 0.6f; 
            float heightOffset = 0.5f; // Lifts the raycast origin up from the floor
            
            // Calculate the new origin slightly in front of and above the enemy's feet
            Vector2 raycastOrigin = pos + new Vector2(roamDirection * boundsOffset, heightOffset);

            // Wall check, horizontal raycast
            var wallHit = Physics2D.Raycast(raycastOrigin, Vector2.right * roamDirection, 0.8f, obstacleLayer);
            Debug.DrawRay(raycastOrigin, Vector2.right * roamDirection, Color.red);
            // Missing ground check, vertical raycast
            var groundHit = Physics2D.Raycast(raycastOrigin, Vector2.down, 2f, obstacleLayer);
            Debug.DrawRay(raycastOrigin, Vector2.down, Color.green);


            var hitWall = wallHit.collider && !wallHit.collider.isTrigger;
            var noGround = !groundHit.collider;

            if (hitWall || noGround)
            {
                Debug.Log(enemy.gameObject.name + " couldn't keep going this direction!");
                roamDirection *= -1f;
                startX = enemy.transform.position.x;
                currentRoamDistance = RandomRoamDistance();
                
                UpdateFacingDirection();
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

        private void UpdateFacingDirection()
        {
            enemy.transform.localScale = roamDirection switch
            {
                > 0 => new Vector3(1, 1, 1),
                < 0 => new Vector3(-1, 1, 1),
                _ => enemy.transform.localScale
            };
        }
    }
}