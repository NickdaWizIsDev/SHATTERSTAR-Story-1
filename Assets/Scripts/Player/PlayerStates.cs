using UnityEditor.Experimental.Licensing;
using UnityEngine;
using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
{
    internal class PlayerStates
    {

        private void Register(StateMachine machine, params State[] states)
        {
            foreach (var s in states)
            {
                machine.availableStates.Add(s.GetType(), s);
            }
        }

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

            // Create substates.
            MovingOnGroundState = new PlayerMovingOnGroundState(player);
            MovingOnAirState = new PlayerMovingOnAirState(player);
            AttackingStillState = new PlayerAttackingStillState(player);
            AttackingMovingState = new PlayerAttackingMovingState(player);
            AttackingOnAirState = new PlayerAttackingOnAirState(player);

            // Now assign them.
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
}