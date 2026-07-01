using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerAttackingMovingState : AttackState
    {
        private PlayerController player;
        private int attackCounter; 
        private float comboTimer; 
        private float comboWindow = 0.2f; 
        private float animationTimer;
        private AnimationClip currentClip;

        public PlayerAttackingMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "MovingAttack";
        }

        public override void Enter()
        {
            // We intentionally DO NOT disable movement here!
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
                player.stateMachine.ChangeStateTo<PlayerIdleState>();
            }
        }

        public override void Exit()
        {
            player.combat.CloseHitbox();
        }

        public override void StartAttack()
        {
            player.combat.InitializeAttack(AttackType.Sword);
            var clipToPlay = player.animations.MovingAttackAnimation; // Assuming you use MovingAttack as the base

            // Directional Checks
            if (player.movement.movementVector.y > 0.5f)
            {
                clipToPlay = player.animations.UpAttackAnimation;
                attackCounter = 0; 
            }
            else
            {
                // Basic Attack Combo Logic
                if(Time.time <= comboTimer)
                {
                    switch(attackCounter)
                    {
                        case 0:
                            clipToPlay = player.animations.GroundAttackAnimation_1;
                            attackCounter++;
                            break;
                        case 1:
                            clipToPlay = player.animations.GroundAttackAnimation_2;
                            attackCounter = 0;
                            break;
                    }
                }
                else 
                {
                    clipToPlay = player.animations.GroundAttackAnimation_1;
                    attackCounter = 1;
                }
            }

            entity.animationManager.PlayAnimation(clipToPlay);
            animationTimer = clipToPlay.length;
            currentClip = clipToPlay;
    
            comboTimer = Time.time + animationTimer + comboWindow;
        }
    }
}