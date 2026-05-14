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
        public PlayerAttackingStillState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnGround";
        }

        public override void Enter()
        {
            if(comboTimer > 0)
            {
                switch(attackCounter)
                {
                    case 0:
                        entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_1);
                        attackCounter++;
                        comboTimer = comboWindow;
                        break;
                    case 1:
                        entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_2);
                        attackCounter = 0;
                        comboTimer = comboWindow;
                        break;
                }
            }
            else 
            {
                entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_1);
                attackCounter = 1;
                comboTimer = comboWindow;
            }
        }
        public override void Do()
        {            
            if(comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
            }
            else player.stateMachine.ChangeStateTo<PlayerIdleState>();
        }
        public override void Exit()
        {
            
        }
    }
}