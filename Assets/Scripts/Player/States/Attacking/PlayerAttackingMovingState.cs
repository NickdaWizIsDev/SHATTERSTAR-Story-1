using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackingMovingState : State
    {
        PlayerController player;
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
    }
}