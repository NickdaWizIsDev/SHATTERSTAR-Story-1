using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackingOnAirState : State
    {
        PlayerController player;
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
    }
}