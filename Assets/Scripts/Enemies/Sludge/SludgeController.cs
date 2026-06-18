using UnityEngine;

namespace Enemies.Sludge
{
    public class SludgeController : EnemyController
    {
        [SerializeField] private float initialRoamDirection;
        [SerializeField] private float idleWaitTime = 2f; // How long it waits before moving again
        private float roamTimer;

        protected override void InitializeStateMachine()
        {
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyRoamState(this, initialRoamDirection, obstacleLayer)
            );

            stateMachine.ChangeStateTo<EnemyIdleState>();
        }

        protected override void Update()
        {
            CurrentState.RecursiveDo();

            // If we are currently roaming (or chasing), keep resetting the timer.
            if (stateMachine.currentState is not EnemyIdleState) 
            {
                roamTimer = idleWaitTime; 
                return;
            }
            
            // Only tick down if we are successfully idling
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0)
            {
                stateMachine.ChangeStateTo<EnemyRoamState>();
            }
        }
    }
}