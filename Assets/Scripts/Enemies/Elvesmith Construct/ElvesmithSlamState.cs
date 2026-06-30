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
            boss.TriggerTelegraphGlow();
            // boss.animationManager.PlayAnimation(boss.animations.SlamAnimation);
            animationTimer = 1.2f; // Replace with actual animation length
        }

        public override void Do()
        {
            animationTimer -= Time.deltaTime;
            if (animationTimer <= 0)
            {
                boss.attackCounter++;
                boss.nextAttackTime = Time.time + boss.slamRecoveryTime;
                boss.stateMachine.ChangeStateTo<ElvesmithChaseState>();
            }
        }

        public override void Exit()
        {
            
        }
    }
}