using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackState : State
    {
        PlayerController player;
        PlayerAttackingStillState attackingStillState;
        PlayerAttackingOnAirState attackingAirState;
        PlayerAttackingMovingState attackingMovingState;

        public PlayerAttackState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Attacking";
            subStateMachine = new StateMachine();

            attackingStillState = new PlayerAttackingStillState(player);
            attackingAirState = new PlayerAttackingOnAirState(player);
            attackingMovingState = new PlayerAttackingMovingState(player);
            subStateMachine.AddStates(attackingStillState, attackingAirState, attackingMovingState);
        }

        public override void Enter()
        {
            if(player.movement.touching.Ground)
            {
                if(player.movement.movementVector == Vector2.zero) subStateMachine.ChangeStateTo<PlayerAttackingStillState>();
                else subStateMachine.ChangeStateTo<PlayerAttackingMovingState>();
            }
            else subStateMachine.ChangeStateTo<PlayerAttackingOnAirState>();
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}