using HSM;
using UnityEngine;

namespace Player
{
    // As you can see, you create States in here.
    internal class PlayerIdleState : State
    {
        private PlayerController player;
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
            if (Mathf.Abs(player.movement.CurrentVelocity.magnitude) > 0.5f)
            {
                entity.stateMachine.ChangeStateTo<PlayerMovingState>();
            }
        }
        public override void Exit()
        {
            
        }
    }
}