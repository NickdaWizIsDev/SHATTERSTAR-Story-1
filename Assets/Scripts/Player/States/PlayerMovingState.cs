using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerMovingState : State
    {
        PlayerController player;
        PlayerMovingOnGroundState onGround;
        PlayerMovingOnAirState onAir;
        public PlayerMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Moving";
            subStateMachine = new StateMachine();
            onGround = new PlayerMovingOnGroundState(player);
            onAir = new PlayerMovingOnAirState(player);

            subStateMachine.AddStates(onGround, onAir);
        }

        public override void Enter()
        {
            if (player.movement.touching.Ground)
            {
                subStateMachine.ChangeStateTo<PlayerMovingOnGroundState>();
            }
            else
            {
                subStateMachine.ChangeStateTo<PlayerMovingOnAirState>();
            }
        }
        public override void Do()
        {
            switch (subStateMachine.currentState)
            {
                case PlayerMovingOnGroundState:
                    if (!player.movement.touching.Ground)
                    {
                        subStateMachine.ChangeStateTo<PlayerMovingOnAirState>();
                    }
                    break;
                case PlayerMovingOnAirState:
                    if (player.movement.touching.Ground)
                    {
                        subStateMachine.ChangeStateTo<PlayerMovingOnGroundState>();
                    }
                    break;
            }

            
            if (Mathf.Abs(player.movement.CurrentVelocity.magnitude) < 0.5f)
            {
                player.stateMachine.ChangeStateTo<PlayerIdleState>();
            }
        }
        public override void Exit()
        {
            
        }
    }
}