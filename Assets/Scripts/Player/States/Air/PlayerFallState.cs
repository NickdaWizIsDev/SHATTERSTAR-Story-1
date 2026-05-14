using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerFallState : State
    {
        PlayerController player;
        public PlayerFallState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Falling";
        }

        public override void Enter()
        {
            player.animationManager.PlayAnimation(player.animations.FallAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
    
}