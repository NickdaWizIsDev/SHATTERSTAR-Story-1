using HSM;
using PrimeTween;
using UnityEngine;

namespace Enemies
{
    public class EnemyAttackState : State
    {
        private EnemyController enemy;
        private float telegraphTimer;
        private float lungeTimer;
        private int attackPhase; 
        private float attackDirection;

        public EnemyAttackState(Entity entity) : base(entity)
        {
            enemy = (EnemyController)entity;
            stateName = "Attacking";
        }

        public override void Enter()
        {
            enemy.body.linearVelocity = new Vector2(0, enemy.body.linearVelocityY);
            telegraphTimer = enemy.telegraphTime; 
            lungeTimer = enemy.attackRecoveryTime; 
            attackPhase = 0;

            if (!enemy.playerTransform) return;
            attackDirection = Mathf.Sign(enemy.playerTransform.position.x - enemy.transform.position.x);
            enemy.transform.localRotation = attackDirection > 0 
                ? new Quaternion(0, 0, 0, 1) 
                : new Quaternion(0, 180, 0, 1);

            enemy.animationManager.PlayAnimation(enemy.animations.TelegraphAnimation);
            BrightUp();
        }

        public override void Do()
        {
            switch (attackPhase)
            {
                case 0:
                {
                    telegraphTimer -= Time.deltaTime;
                    if (!(telegraphTimer <= 0)) return;
                    attackPhase = 1;
                    enemy.animationManager.PlayAnimation(enemy.animations.AttackAnimation);
                    enemy.body.linearVelocityX = attackDirection * enemy.attackLungeSpeed;
                    enemy.ResetAttackCD();
                    break;
                }
                case 1:
                {
                    lungeTimer -= Time.deltaTime;
                    if (lungeTimer <= 0)
                    {
                        enemy.stateMachine.ChangeStateTo<EnemyIdleState>();
                        BrightDown();
                    }

                    break;
                }
            }
        }

        public override void Exit()
        {
            enemy.body.linearVelocityX = 0;
        }


        private void BrightUp(float time = 0.6f)
        {
            Tween.Custom(Color.white, enemy.attackGlowColor, duration: time, onValueChange: color =>
            {
                if (enemy.spriteRenderer is null) return;
                enemy.spriteRenderer.GetPropertyBlock(enemy.propBlock);
                enemy.propBlock.SetColor("_Emission_Color", color);
                enemy.spriteRenderer.SetPropertyBlock(enemy.propBlock);
            });
        }

        private void BrightDown(float time = 0.8f)
        {
            Tween.Custom(enemy.attackGlowColor, Color.white, duration: time, onValueChange: color =>
            {
                if (enemy.spriteRenderer is null) return;
                enemy.spriteRenderer.GetPropertyBlock(enemy.propBlock);
                enemy.propBlock.SetColor("_Emission_Color", color);
                enemy.spriteRenderer.SetPropertyBlock(enemy.propBlock);
            });
        }
    }
}