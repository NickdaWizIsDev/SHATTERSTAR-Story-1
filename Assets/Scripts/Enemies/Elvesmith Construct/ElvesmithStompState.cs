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
            boss.TriggerTelegraphGlow(0.8f);
            
            // boss.animationManager.PlayAnimation(boss.animations.StompAnimation);
            animationTimer = 1.5f; 
        }

        public override void Do()
        {
            animationTimer -= Time.deltaTime;

            // Spawn shockwave at the exact moment the foot hits the ground (e.g., 0.5s before animation ends)
            if (animationTimer <= 0.5f && !shockwaveSpawned)
            {
                shockwaveSpawned = true;
                Object.Instantiate(boss.shockwavePrefab, boss.shockwaveSpawnPoint.position, Quaternion.identity);
                // CameraEffects.Instance.Shake(0.2f, 1.5f);
            }

            if (!(animationTimer <= 0)) return;
            boss.nextAttackTime = Time.time + boss.stompRecoveryTime;
            boss.stateMachine.ChangeStateTo<ElvesmithChaseState>();
        }

        public override void Exit()
        {
            
        }
    }
}