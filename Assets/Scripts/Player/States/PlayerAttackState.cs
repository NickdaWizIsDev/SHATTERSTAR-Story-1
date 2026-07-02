using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerAttackState : State
    {
        private PlayerController player;

        public PlayerAttackState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Attacking";
            subStateMachine = new StateMachine();

            var attackingStillState = new PlayerAttackingStillState(player);
            var attackingAirState = new PlayerAttackingOnAirState(player);
            var attackingMovingState = new PlayerAttackingMovingState(player);
            subStateMachine.AddStates(attackingStillState, attackingAirState, attackingMovingState);
        }

        public override void Enter()
        {
            if(player.movement.touching.Ground)
            {
                if(player.movement.movementVector.x == 0) subStateMachine.ChangeStateTo<PlayerAttackingStillState>();
                else
                {
                    subStateMachine.ChangeStateTo<PlayerAttackingMovingState>();
                }
            }
            else
            {
                subStateMachine.ChangeStateTo<PlayerAttackingOnAirState>();
            }
        }
        public override void Do()
        {
            if (subStateMachine.currentState is null) player.stateMachine.ChangeStateTo<PlayerIdleState>();
        }
        public override void Exit()
        {
            player.CanMove = true;
        }

        public void StartAttack()
        {
            var atk = subStateMachine.currentState as AttackState;
            atk?.StartAttack();
        }
    }
}