using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerAttackingOnAirState : AttackState
    {
        private PlayerController player;
        private float animationTimer;
        private AnimationClip currentClip;

        public PlayerAttackingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "AirAttack";
        }

        public override void Enter()
        {
            player.animationManager.SetAnimationSpeed(1);
        }

        public override void Do()
        {
            if(animationTimer > 0)
            {
                animationTimer -= Time.deltaTime;
            }
            else 
            {
                player.stateMachine.ChangeStateTo<PlayerIdleState>(); 
                // Note: You might want to change this to a PlayerFallState depending on your HSM setup.
            }
        }

        public override void Exit()
        {
            player.combat.CloseHitbox();
        }

        public override void StartAttack()
        {
            bool isPogo = false;
            var clipToPlay = player.animations.AirAttackAnimation;

            if (animationTimer / (currentClip ? currentClip.length : clipToPlay.length) > 0.15f) return;

            // Directional Checks
            if (player.movement.movementVector.y > 0.5f)
            {
                clipToPlay = player.animations.UpAttackAnimation;
            }
            else if (player.movement.movementVector.y < -0.5f)
            {
                clipToPlay = player.animations.DownAttackAnimation;
                isPogo = true; // Toggle the pogo flag!
            }

            // We pass the pogo flag into the Combat script
            player.combat.InitializeAttack(AttackType.Sword, isPogo);

            entity.animationManager.PlayAnimation(clipToPlay);
            animationTimer = clipToPlay.length;
            currentClip = clipToPlay;
        }
    }
}