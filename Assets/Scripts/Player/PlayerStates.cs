using UnityEditor.Experimental.Licensing;
using UnityEngine;

namespace Player
{
    internal class PlayerStates
    {
        internal PlayerStates(PlayerController player)
        {
            // The constructor passes the entity down to the states, so they can access it and its components.

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
        public PlayerIdleState(Entity entity) : base(entity)
        {
            this.entity = entity;
            stateName = "Idle";
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

    internal class PlayerMovingState : State
    {
        public PlayerMovingState(Entity entity) : base(entity)
        {
            this.entity = entity;
            stateName = "Moving";
            subStateMachine = new StateMachine();
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

    internal class PlayerAttackState : State
    {
        public PlayerAttackState(Entity entity) : base(entity)
        {
            this.entity = entity;
            stateName = "Attacking";
            subStateMachine = new StateMachine();
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

    internal class PlayerDashingState : State
    {
        public PlayerDashingState(Entity entity) : base(entity)
        {
            this.entity = entity;
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
        public PlayerMovingOnGroundState(Entity entity) : base(entity)
        {
            this.entity = entity;
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
        public PlayerMovingOnAirState(Entity entity) : base(entity)
        {
            this.entity = entity;
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
        public PlayerAttackingStillState(Entity entity) : base(entity)
        {
            this.entity = entity;
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
        public PlayerAttackingMovingState(Entity entity) : base(entity)
        {
            this.entity = entity;
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
        public PlayerAttackingOnAirState(Entity entity) : base(entity)
        {
            this.entity = entity;
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