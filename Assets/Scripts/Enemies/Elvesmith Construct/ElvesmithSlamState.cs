using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithSlamState : State
    {
        private ElvesmithController boss;
        private float animationTimer;

        public ElvesmithSlamState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "Slam";
        }

        public override void Enter()
        {
            boss.body.linearVelocity = Vector2.zero;
            boss.animationManager.PlayAnimation(boss.animations.SlamAnimation);
            animationTimer = boss.animations.SlamAnimation.length;
            boss.TriggerTelegraphGlow(animationTimer/2);
        }

        public override void Do()
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer > 0) return;
            boss.attackCounter++;
            boss.nextAttackTime = Time.time + boss.slamRecoveryTime;
            boss.stateMachine.ChangeStateTo<ElvesmithIdleState>();
        }

        public override void Exit()
        {
            
        }
    }
}