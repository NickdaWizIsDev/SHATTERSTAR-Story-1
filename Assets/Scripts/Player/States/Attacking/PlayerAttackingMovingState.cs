using HSM;

namespace Player
{
    internal class PlayerAttackingMovingState : AttackState
    {
        private PlayerController player;
        public PlayerAttackingMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "MovingAttack";
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