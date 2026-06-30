using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvesmithChaseState : State
    {
        private ElvesmithController boss;

        public ElvesmithChaseState(ElvesmithController boss) : base(boss)
        {
            this.boss = boss;
            stateName = "Chase";
        }

        public override void Enter()
        {
            // play Move
        }

        public override void Do()
        {
            if (boss.playerTarget == null) return;

            boss.FacePlayer();
            var distanceToPlayer = Vector2.Distance(boss.transform.position, boss.playerTarget.position);

            if (Time.time >= boss.nextAttackTime)
            {
                if (boss.attackCounter >= boss.attacksBeforeStomp)
                {
                    boss.stateMachine.ChangeStateTo<ElvesmithStompState>();
                    return;
                }

                if (distanceToPlayer <= boss.slamAttackRange)
                {
                    boss.stateMachine.ChangeStateTo<ElvesmithSlamState>();
                    return;
                }
                if (distanceToPlayer <= boss.dashAttackRange)
                {
                    boss.stateMachine.ChangeStateTo<ElvesmithDashState>();
                    return;
                }
            }

            // Move towards player
            var targetVelocity = new Vector2(Mathf.Sign(boss.playerTarget.position.x - boss.transform.position.x) * boss.moveSpeed, boss.body.linearVelocityY);
            boss.body.linearVelocity = targetVelocity;
        }

        public override void Exit()
        {
            boss.body.linearVelocity = new Vector2(0, boss.body.linearVelocityY);
        }
    }
}