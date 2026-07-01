using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithIdleState : State
    {
        private ElvesmithController boss;
        public ElvesmithIdleState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "idle";
        }

        public override void Enter()
        {
            boss.animationManager.PlayAnimation(boss.animations.IdleAnimation);
        }

        public override void Do()
        {
            boss.FacePlayer();

            // 2. Apply "friction" so he gracefully slides to a halt if knocked back
            boss.body.linearVelocity = Vector2.Lerp(boss.body.linearVelocity, new Vector2(0, boss.body.linearVelocityY), 5f * Time.deltaTime);

            if(Time.time < boss.nextAttackTime) return;
            
            boss.stateMachine.ChangeStateTo<ElvesmithChaseState>();
        }

        public override void Exit()
        {
        }
    }
}