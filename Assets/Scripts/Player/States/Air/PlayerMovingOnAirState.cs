using Assets.Scripts.Statemachine;

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
            if(player.movement.CurrentVelocity.y > 0) subStateMachine.ChangeState<PlayerJumpState>();
            else if(player.movement.CurrentVelocity.y < 0) subStateMachine.ChangeState<PlayerFallState>();
            else subStateMachine.ChangeState<PlayerAirHangState>();
        }
        public override void Do()
        {
            // switch (subStateMachine.AAAAAAAAAAAAAA)
            // {
            //     case PlayerJumpState:
            //         if (player.movement.CurrentVelocity.y <= 0) subStateMachine.ChangeState<PlayerFallState>();
            //         break;
            //     case PlayerAirHangState:
            //         if (player.movement.CurrentVelocity.y > 0) subStateMachine.ChangeState<PlayerJumpState>();
            //         else if (player.movement.CurrentVelocity.y < 0) subStateMachine.ChangeState<PlayerFallState>();
            //         break;
            //     case PlayerFallState:
            //         if (player.movement.CurrentVelocity.y > 0) subStateMachine.ChangeState<PlayerJumpState>();
            //         else if (player.movement.CurrentVelocity.y == 0) subStateMachine.ChangeState<PlayerAirHangState>();
            //         break;
            // }
        }
        public override void Exit()
        {
            
        }
    }    
}