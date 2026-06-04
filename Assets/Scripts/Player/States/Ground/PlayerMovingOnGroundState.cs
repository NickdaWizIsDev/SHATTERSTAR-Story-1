using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerMovingOnGroundState : State
    {
        private PlayerController player;
        public PlayerMovingOnGroundState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnGround";
        }

        public override void Enter()
        {
            player.animationManager.PlayAnimation(player.animations.RunAnimation);
        }
        public override void Do()
        {
            var horizontalSpeed = Mathf.Abs(player.movement.CurrentVelocity.x);
            player.animationManager.SetAnimationSpeed(Map(horizontalSpeed, 0, player.movement.runVelocity, 0.25f, 1));
        }
        public override void Exit()
        {
            player.animationManager.SetAnimationSpeed(1);
        }

        private static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}