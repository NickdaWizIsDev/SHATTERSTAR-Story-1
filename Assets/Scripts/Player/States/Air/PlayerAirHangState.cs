using HSM;

namespace Player
{
    internal class PlayerAirHangState : State
    {
        private PlayerController player;
        public PlayerAirHangState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Hang";
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