using HSM;

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
            DeathState = new PlayerDeathState(player);

            player.stateMachine.AddStates(IdleState, MovingState, AttackState, DashingState, DeathState);
        }

        private State IdleState;
        private State MovingState;
        private State AttackState;
        private State DashingState;
        private State DeathState;
    }
}