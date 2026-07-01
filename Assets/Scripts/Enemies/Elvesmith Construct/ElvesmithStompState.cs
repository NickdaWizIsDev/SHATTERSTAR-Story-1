using Gameplay;
using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithStompState : State
    {
        private ElvesmithController boss;
        private float animationTimer;
        private bool shockwaveSpawned;

        public ElvesmithStompState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "Stomp";
        }

        public override void Enter()
        {
            boss.body.linearVelocity = Vector2.zero;
            boss.attackCounter = 0; 
            shockwaveSpawned = false;
            
            boss.animationManager.PlayAnimation(boss.animations.StompAnimation);
            animationTimer = boss.animations.StompAnimation.length;
            boss.TriggerTelegraphGlow(animationTimer/2);
        }

        public override void Do()
        {
            animationTimer -= Time.deltaTime;

            if (animationTimer <= 1f && !shockwaveSpawned)
            {
                shockwaveSpawned = true;
                var sw = Object.Instantiate(boss.shockwavePrefab, boss.shockwaveSpawnPoint.position, Quaternion.identity);
                sw.MoveDirection = new Vector2(boss.transform.localScale.x, 0f);
                CameraEffects.Instance.Shake(0.5f, 1.5f);
            }

            if (!(animationTimer <= 0)) return;
            boss.nextAttackTime = Time.time + boss.stompRecoveryTime;
            boss.stateMachine.ChangeStateTo<ElvesmithIdleState>();
        }

        public override void Exit()
        {
        }
    }
}