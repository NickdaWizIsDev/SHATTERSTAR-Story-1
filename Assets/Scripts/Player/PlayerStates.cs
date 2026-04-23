using UnityEditor.Experimental.Licensing;
using UnityEngine;
using Assets.Scripts.Statemachine;

namespace Assets.Scripts.Player
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

            player.stateMachine.AddStates(IdleState, MovingState, AttackState, DashingState);
        }
        
        internal State IdleState;
        internal State MovingState;
        internal State AttackState;
        internal State DashingState;
    }
}