using HSM;
using UnityEngine;

namespace Enemies
{
    public class ElvenConstruct : EnemyController
    {
        protected override void InitializeStateMachine()
        {
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyChaseState(this),
                new EnemyAttackState(this)
            );
            
            stateMachine.ChangeStateTo<EnemyIdleState>();
        }
    }
}