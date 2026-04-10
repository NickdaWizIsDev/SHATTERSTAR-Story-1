using UnityEditor.Experimental.Licensing;
using UnityEngine;

namespace Player
{
    internal class PlayerStates
    {
        internal PlayerStates(PlayerController player)
        {
            // The constructor passes the player down to the states, so they can access it and its components.

            // Create our base StateMachine.
            player.stateMachine = new StateMachine();

            // Create the base States.
            IdleState = new PlayerIdleState(player);
            MovingState = new PlayerMovingState(player);
            AttackState = new PlayerAttackState(player);
            DashingState = new PlayerDashingState(player);

            // Assign them to the base StateMachine.
            player.stateMachine.availableStates = new State[]
            {
                IdleState, MovingState, AttackState, DashingState
            };

            // Create substates and assign them to their respective StateMachines.
            MovingOnGroundState = new PlayerMovingOnGroundState(player);
            MovingOnAirState = new PlayerMovingOnAirState(player);
            MovingState.subStateMachine.availableStates = new State[]
            {
                MovingOnGroundState, MovingOnAirState
            };
            AttackingStillState = new PlayerAttackingStillState(player);
            AttackingMovingState = new PlayerAttackingMovingState(player);
            AttackingOnAirState = new PlayerAttackingOnAirState(player);
            AttackState.subStateMachine.availableStates = new State[]
            {
                AttackingStillState, AttackingMovingState, AttackingOnAirState
            };
        }        
        
        internal State IdleState;
        internal State MovingState;
        internal State AttackState;
        internal State DashingState;

        internal State MovingOnGroundState;
        internal State MovingOnAirState;

        internal State AttackingStillState;
        internal State AttackingMovingState;
        internal State AttackingOnAirState;
    }

    // As you can see, you create States in here.
    internal class PlayerIdleState : State
    {
        PlayerController player;
        public PlayerIdleState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Idle";
        }

        public override void Enter()
        {
            // This animation doesn't exist yet. Anyways this serves as example of how to play animations when entering a state.
            //entity.animationManager.PlayAnimation(player.animations.IdleAnimation);
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerMovingState : State
    {
        PlayerController player;
        public PlayerMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Moving";
            subStateMachine = new StateMachine();
        }

        public override void Enter()
        {
            entity.animationManager.PlayAnimation(player.animations.RunAnimation);
        }
        public override void Do()
        {
            // Evaluate the possibility of updating the animation speed depending on the player's velocity.

            // Transition between the different movement states.
        }
        public override void Exit()
        {
            
        }
    }

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
                        attackCounter++;
                        break;
                    case 2:
                        entity.animationManager.PlayAnimation(player.animations.GroundAttackAnimation_3);
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

    internal class PlayerDashingState : State
    {
        PlayerController player;
        public PlayerDashingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "Dashing";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerMovingOnGroundState : State
    {
        PlayerController player;
        public PlayerMovingOnGroundState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnGround";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerMovingOnAirState : State
    {
        PlayerController player;
        public PlayerMovingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "OnAir";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerAttackingStillState : State
    {
        PlayerController player;
        public PlayerAttackingStillState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "BasicAttack";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerAttackingMovingState : State
    {
        PlayerController player;
        public PlayerAttackingMovingState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "MovingAttack";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }

    internal class PlayerAttackingOnAirState : State
    {
        PlayerController player;
        public PlayerAttackingOnAirState(PlayerController entity) : base(entity)
        {
            this.entity = entity;
            player = entity;
            stateName = "AirAttack";
        }

        public override void Enter()
        {
            
        }
        public override void Do()
        {
            
        }
        public override void Exit()
        {
            
        }
    }
}