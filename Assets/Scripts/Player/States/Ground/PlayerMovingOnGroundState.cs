using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerMovingOnGroundState : State
    {
        private PlayerController player;
        private bool wasRunning;

        public PlayerMovingOnGroundState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnGround";
        }

        public override void Enter()
        {
            wasRunning = player.isRunning;
            //TODO: get a proper walk animation in here if we're not running
            player.animationManager.PlayAnimation(wasRunning ? player.animations.RunAnimation : player.animations.RunAnimation);
        }
        public override void Do()
        {
            if (wasRunning != player.isRunning)
            {
                wasRunning = player.isRunning;
                player.animationManager.PlayAnimation(wasRunning ? player.animations.RunAnimation : player.animations.WalkAnimation);
            }

            var horizontalSpeed = Mathf.Abs(player.movement.CurrentVelocity.x);
            var activeVelocity = wasRunning ? player.movement.runVelocity : player.movement.walkVelocity;
            player.animationManager.SetAnimationSpeed(Map(horizontalSpeed, 0, activeVelocity, 0.25f, 1));
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