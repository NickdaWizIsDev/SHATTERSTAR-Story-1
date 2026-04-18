using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackingStillState : State
    {
        PlayerController player;
        public PlayerAttackingStillState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "BasicAttack";
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