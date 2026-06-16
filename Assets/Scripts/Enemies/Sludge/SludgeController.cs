using UnityEngine;

namespace Enemies.Sludge
{
    public class SludgeController : EnemyController
    {
        [SerializeField] private float initialRoamDirection;
        private float roamTimer;
        protected override void InitializeStateMachine()
        {
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyRoamState(this, initialRoamDirection)
            );
        }

        protected override void Update()
        {
            CurrentState.RecursiveDo();

            if (stateMachine.currentState is not EnemyIdleState) return;
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0)
            {
                stateMachine.ChangeStateTo<EnemyRoamState>();
            }
        }
    }
}