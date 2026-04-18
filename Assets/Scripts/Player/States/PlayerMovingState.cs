using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerMovingState : State
    {
        PlayerController player;
        public PlayerMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Moving";
            subStateMachine = new StateMachine();
        }

        public override void Enter()
        {
            entity.animationManager.PlayAnimation(player.animations.RunAnimation);
        }
        public override void Do()
        {
            // Evaluate the possibility of updating the animation speed depending on the player's velocity.

            // Transition between the different movement states.
        }
        public override void Exit()
        {
            
        }
    }
}