using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    // As you can see, you create States in here.
    internal class PlayerIdleState : State
    {
        PlayerController player;
        public PlayerIdleState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Idle";
        }

        public override void Enter()
        {
            // This animation doesn't exist yet. Anyways this serves as example of how to play animations when entering a state.
            //entity.animationManager.PlayAnimation(player.animations.IdleAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}