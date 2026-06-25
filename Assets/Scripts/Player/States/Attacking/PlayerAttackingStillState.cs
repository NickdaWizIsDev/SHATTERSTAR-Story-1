using HSM;
using UnityEngine;

namespace Player
{
    internal class PlayerAttackingStillState : AttackState
    {
        private PlayerController player;
        private int attackCounter; // This is meant to be used for the basic attack combo, to know which attack animation to play.
        private float comboTimer; // This is to know when to reset the attackCounter.
        private float comboWindow = 0.2f; // The time window in which the player can perform the next attack in the combo.
        private float animationTimer;
        private AnimationClip currentClip;

        public PlayerAttackingStillState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Still";
        }

        public override void Enter()
        {
            player.CanMove = false;
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
            }
        }
        public override void Exit()
        {
            player.CanMove = true;
            player.combat.CloseHitbox();
        }

        public override void StartAttack()
        {
            player.combat.InitializeAttack(AttackType.Sword);
            var clipToPlay = player.animations.GroundAttackAnimation_1;
    
            // Ignore the input if the current swing hasn't completed to a certain point.
            if (animationTimer / (currentClip ? currentClip.length : clipToPlay.length) > 0.15f) return;

            // We use > 0.5f to account for joystick dead zones, preventing accidental up-attacks.
            if (player.movement.movementVector.y > 0.5f)
            {
                clipToPlay = player.animations.UpAttackAnimation;
        
                // Optional: Reset the basic combo counter so returning to normal attacks feels fresh.
                attackCounter = 0; 
            }
            else
            {
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