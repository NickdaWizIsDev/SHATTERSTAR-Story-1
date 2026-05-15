using System.Collections;
using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerMovingOnAirState : State
    {
        PlayerController player;
        PlayerJumpState jumpState;
        PlayerAirHangState airHangState;
        PlayerFallState fallState;
        public PlayerMovingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnAir";
            subStateMachine = new StateMachine();

            jumpState = new PlayerJumpState(player);
            airHangState = new PlayerAirHangState(player);
            fallState = new PlayerFallState(player);
            subStateMachine.AddStates(jumpState, airHangState, fallState);
        }

        public override void Enter()
        {
            if(player.movement.CurrentVelocity.y > 0.5f) 
            {
                subStateMachine.ChangeStateTo<PlayerJumpState>();                
                player.StartCoroutine(ChangeToHangState());
                return;
            }
            else if(player.movement.CurrentVelocity.y < -0.5f) subStateMachine.ChangeStateTo<PlayerFallState>();
            else subStateMachine.ChangeStateTo<PlayerAirHangState>();            
            player.StopCoroutine(ChangeToHangState());
        }
        public override void Do()
        {
            switch (subStateMachine.currentState)
            {
                case PlayerJumpState:
                    if (player.movement.CurrentVelocity.y <= 0.5f) subStateMachine.ChangeStateTo<PlayerAirHangState>();
                    break;
                case PlayerAirHangState:
                    if (player.movement.CurrentVelocity.y < -1f) subStateMachine.ChangeStateTo<PlayerFallState>();
                    break;
                case PlayerFallState:
                    if (player.movement.CurrentVelocity.y >= -0.5f && player.movement.CurrentVelocity.y <= 0.5f) 
                        subStateMachine.ChangeStateTo<PlayerAirHangState>();
                    break;
            }
        }
        public override void Exit()
        {
            player.StopCoroutine(ChangeToHangState());
        }

        private IEnumerator ChangeToHangState()
        {
            yield return new WaitForSeconds(player.animations.JumpAnimation.length);
            subStateMachine.ChangeStateTo<PlayerAirHangState>();
        }
    }    
}