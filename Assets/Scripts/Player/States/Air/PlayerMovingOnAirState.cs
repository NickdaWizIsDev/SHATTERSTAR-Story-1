using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerMovingOnAirState : State
    {
        PlayerController player;
        public PlayerMovingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnAir";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}