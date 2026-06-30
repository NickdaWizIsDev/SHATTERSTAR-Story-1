using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithDashState : State
    {
        private ElvesmithController boss;
        private float animationTimer;

        public ElvesmithDashState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "Dash";
        }

        public override void Enter()
        {
            boss.TriggerTelegraphGlow(0.3f);
            // boss.animationManager.PlayAnimation(boss.animations.DashAnimation);
            
            // Lunge forward
            var direction = Mathf.Sign(boss.transform.localScale.x);
            boss.body.AddForce(new Vector2(direction * 15f, 0), ForceMode2D.Impulse);
            
            animationTimer = 0.8f; 
        }

        public override void Do()
        {
            animationTimer -= Time.deltaTime;
            
            // Apply friction so he doesn't slide forever
            boss.body.linearVelocity = Vector2.Lerp(boss.body.linearVelocity, new Vector2(0, boss.body.linearVelocityY), 5f * Time.deltaTime);

            if (animationTimer > 0) return;
            boss.attackCounter++;
            boss.nextAttackTime = Time.time + boss.dashRecoveryTime;
            boss.stateMachine.ChangeStateTo<ElvesmithChaseState>();
        }

        public override void Exit()
        {
            
        }
    }
}