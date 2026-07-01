using System.Dynamic;
using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithDashState : State
    {
        private ElvesmithController boss;
        private float animationTimer;
        private bool isDashing;

        public ElvesmithDashState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "Dash";
        }

        public override void Enter()
        {
            boss.TriggerTelegraphGlow();
            boss.animationManager.PlayAnimation(boss.animations.DashAnimation);
            isDashing = false;

            _ = Dash();
        }
        public override void Do()
        {
            if(!isDashing) return;
            animationTimer -= Time.deltaTime;
            
            boss.body.linearVelocity = Vector2.Lerp(boss.body.linearVelocity, new Vector2(0, boss.body.linearVelocityY), 5f * Time.deltaTime);

            if (animationTimer > 0) return;
            
            boss.attackCounter++;
            
            boss.currentDashAttempts--;

            if (boss.currentDashAttempts <= 0)
            {
                boss.nextDashTime = Time.time + boss.dashRefreshCooldown; 
                boss.currentDashAttempts = boss.maxDashAttempts; 
            }
            else
            {
                boss.nextDashTime = Time.time + boss.dashRecoveryTime; 
            }

            boss.animationManager.PlayAnimation(boss.animations.IdleAnimation);
            boss.nextAttackTime = Time.time + boss.dashRecoveryTime;
            boss.stateMachine.ChangeStateTo<ElvesmithIdleState>();
        }

        public override void Exit()
        {
            
        }

        private async Awaitable Dash()
        {
            animationTimer = boss.animations.DashAnimation.length - 0.5f;
            // Wait for 0.5 seconds to telegraph the attack
            await Awaitable.WaitForSecondsAsync(0.5f);

            // Check if the boss didn't die in the meantime (or other checks)
            if (boss is null || !boss.gameObject.activeInHierarchy || boss.stateMachine.currentState != this) return;

            // Lunge forward
            var direction = Mathf.Sign(boss.transform.localScale.x);
            boss.body.AddForce(new Vector2(direction * 20f, 0), ForceMode2D.Impulse);

            isDashing = true;
        }
    }
}