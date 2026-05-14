using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerAirHangState : State
    {
        PlayerController player;
        public PlayerAirHangState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Hang";
        }

        public override void Enter()
        {
            player.animationManager.PlayAnimation(player.animations.AirHangAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
    
}