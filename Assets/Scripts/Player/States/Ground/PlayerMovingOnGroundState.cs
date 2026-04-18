using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerMovingOnGroundState : State
    {
        PlayerController player;
        public PlayerMovingOnGroundState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnGround";
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