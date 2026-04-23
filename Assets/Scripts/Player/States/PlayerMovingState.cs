using Assets.Scripts.Statemachine;

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
                subStateMachine.ChangeState<PlayerMovingOnGroundState>();
            }
            else
            {
                subStateMachine.ChangeState<PlayerMovingOnAirState>();
            }
        }
        public override void Do()
        {
            switch (subStateMachine.currentState)
            {
                case PlayerMovingOnGroundState:
                    if (!player.movement.touching.Ground)
                    {
                        subStateMachine.ChangeState<PlayerMovingOnAirState>();
                    }
                    break;
                case PlayerMovingOnAirState:
                    if (player.movement.touching.Ground)
                    {
                        subStateMachine.ChangeState<PlayerMovingOnGroundState>();
                    }
                    break;
            }
        }
        public override void Exit()
        {
            
        }
    }
}