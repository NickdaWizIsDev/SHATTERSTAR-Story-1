using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerDashingState : State
    {
        private PlayerController player;
        private float dashTimer;
        private float dashDirection;

        public PlayerDashingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Dashing";
        }

        public override void Enter()
        {
            dashTimer = player.animations.DashAnimation?.length ?? 0.35f;       
            if (Mathf.Abs(player.movement.movementVector.x) > 0.1f)
            {
                dashDirection = Mathf.Sign(player.movement.movementVector.x);
            }
            else
            {
                dashDirection = player.transform.localScale.x > 0 ? 1 : -1;
            }

            player.movement.body.gravityScale = 0f;
            player.movement.body.linearVelocityY = 0f;
            player.hurtbox.enabled = false;

            if (player.animations.DashAnimation is not null)
            {
                player.animationManager.PlayAnimation(player.animations.DashAnimation);
            }
            
            player.dashCdTimer = player.dashCooldown;
        }

        public override void Do()
        {
            dashTimer -= Time.deltaTime;

            player.movement.body.linearVelocityX = dashDirection * player.movement.dashSpeed;

            if (dashTimer > 0) return;
            if (Mathf.Abs(player.movement.movementVector.x) > 0.1f)
            {
                player.stateMachine.ChangeStateTo<PlayerMovingState>();
            }
            else
            {
                player.stateMachine.ChangeStateTo<PlayerIdleState>();
            }
        }

        public override void Exit()
        {
            player.movement.body.gravityScale = player.movement.fallGravityScale;
            player.hurtbox.enabled = true;

            var isHoldingDashDirection = Mathf.Abs(player.movement.movementVector.x) > 0.1f && 
                                         // ReSharper disable once CompareOfFloatsByEqualityOperator
                                         Mathf.Sign(player.movement.movementVector.x) == Mathf.Sign(dashDirection);

            if (!isHoldingDashDirection)
            {
                player.movement.body.linearVelocityX = 0f;
            }
        }
    }
}