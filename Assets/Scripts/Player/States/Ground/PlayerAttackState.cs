using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerAttackState : State
    {
        PlayerController player;
        int attackCounter; // This is meant to be used for the basic attack combo, to know which attack animation to play.
        int comboTimer; // This is meant to be used for the basic attack combo, to know when to reset the attackCounter.
        public PlayerAttackState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Attacking";
            subStateMachine = new StateMachine();
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
                        break;
                    case 1:
                        entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_2);
                        attackCounter = 0;
                        break;
                }
            }
            else 
            {
                entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_1);
                attackCounter = 1;
            }
        }
        public override void Do()
        {
            if(comboTimer > 0)
            {
                comboTimer--;
            }
        }
        public override void Exit()
        {
            
        }
    }
}