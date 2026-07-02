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
             
                if (!(player.attackBufferTimer > 0) ||
                    !((animationTimer / (currentClip ? currentClip.length : 1f)) <= 0.15f)) return;
                player.attackBufferTimer = 0f; // Consume the buffer
                StartAttack();
            }
            else 
            {
                if (Mathf.Abs(player.movement.movementVector.x) > 0)
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
            player.combat.CloseHitbox();
        }

        public override void StartAttack()
        {
            var isPogo = false;
            var clipToPlay = player.animations.AirAttackAnimation;

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
            else
            {
                switch (currentClip == player.animations.AirAttackAnimation)
                {
                    case true:
                        // Normally, the alternative attack anim would be here, but there's none, and I'm
                        // using the exact same animation anyway
                        clipToPlay = player.animations.GroundAttackAnimation_2;
                        break;
                    case false:
                        // no need to do anything hahahahahahah imsotired
                        break;
                }
            }

            // We pass the pogo flag into the Combat script
            player.combat.InitializeAttack(AttackType.Sword, isPogo);

            entity.animationManager.PlayAnimation(clipToPlay);
            animationTimer = clipToPlay.length;
            currentClip = clipToPlay;
        }
    }
}