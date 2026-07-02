using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerMovingState : State
    {
        private PlayerController player;

        public PlayerMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Moving";
            subStateMachine = new StateMachine();
            var onGround1 = new PlayerMovingOnGroundState(player);
            var onAir1 = new PlayerMovingOnAirState(player);

            subStateMachine.AddStates(onGround1, onAir1);
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
                        player.PlaySFX(player.landAudio);
                        subStateMachine.ChangeStateTo<PlayerMovingOnGroundState>();
                    }
                    break;
            }


            if (!(Mathf.Abs(player.movement.CurrentVelocity.magnitude) < 0.15f)) return;
            player.stateMachine.ChangeStateTo<PlayerIdleState>();
        }
        public override void Exit()
        {
            player.animationManager.SetAnimationSpeed(1);
        }
    }
}