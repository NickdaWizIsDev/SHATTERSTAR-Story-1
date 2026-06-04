using HSM;

namespace Player
{
    internal class PlayerJumpState : State
    {
        private PlayerController player;
        public PlayerJumpState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Jump";
        }

        public override void Enter()
        {
            player.animationManager.PlayAnimation(player.animations.JumpAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
    
}