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
            player.animationManager.PlayAnimation(player.animations.IdleAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}