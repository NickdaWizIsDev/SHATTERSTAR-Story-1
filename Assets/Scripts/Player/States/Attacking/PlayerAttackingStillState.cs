using Assets.Scripts.Statemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackingStillState : State
    {
        PlayerController player;
        int attackCounter; // This is meant to be used for the basic attack combo, to know which attack animation to play.
        float comboTimer; // This is meant to be used for the basic attack combo, to know when to reset the attackCounter.
        float comboWindow = 0.5f; // The time window in which the player can perform the next attack in the combo.
        float animationTimer;

        public PlayerAttackingStillState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Still";
        }

        public override void Enter()
        {
            player.canMove = false;
            
            AnimationClip clipToPlay = player.animations.GroundAttackAnimation_1;

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

            entity.animationManager.PlayAnimation(clipToPlay);
            animationTimer = clipToPlay.length;
            
            comboTimer = Time.time + animationTimer + comboWindow;
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
            player.canMove = true;
        }
    }
}