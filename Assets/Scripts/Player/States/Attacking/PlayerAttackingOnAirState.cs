using HSM;

namespace Player
{
    internal class PlayerAttackingOnAirState : AttackState
    {
        private PlayerController player;
        public PlayerAttackingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "AirAttack";
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

        public override void StartAttack()
        {
            
        }
    }
}