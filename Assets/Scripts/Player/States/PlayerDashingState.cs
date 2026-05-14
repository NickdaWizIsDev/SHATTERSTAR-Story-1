using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerDashingState : State
    {
        PlayerController player;
        float dashTimer;
        float dashDirection;

        public PlayerDashingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Dashing";
        }

        public override void Enter()
        {
            dashTimer = player.animations.DashAnimation != null ? player.animations.DashAnimation.length : 0.35f;       
            if (Mathf.Abs(player.movement.movementVector.x) > 0.1f)
            {
                dashDirection = Mathf.Sign(player.movement.movementVector.x);
            }
            else
            {
                dashDirection = player.transform.localRotation.y == 0 ? 1 : -1;
            }

            player.movement.body.gravityScale = 0f;
            player.movement.body.linearVelocityY = 0f;

            if (player.animations.DashAnimation != null)
            {
                player.animationManager.PlayAnimation(player.animations.DashAnimation);
            }
        }

        public override void Do()
        {
            dashTimer -= Time.deltaTime;

            player.movement.body.linearVelocityX = dashDirection * player.movement.dashSpeed;

            if (dashTimer <= 0)
            {
                if (Mathf.Abs(player.movement.movementVector.x) > 0.1f)
                {
                    player.stateMachine.ChangeStateTo<PlayerMovingState>();
                }
                else
                {
                    player.stateMachine.ChangeStateTo<PlayerIdleState>();
                }
            }
        }

        public override void Exit()
        {
            player.movement.body.gravityScale = player.movement.fallGravityScale;
        }
    }
}