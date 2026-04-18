using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerDashingState : State
    {
        PlayerController player;
        public PlayerDashingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Dashing";
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