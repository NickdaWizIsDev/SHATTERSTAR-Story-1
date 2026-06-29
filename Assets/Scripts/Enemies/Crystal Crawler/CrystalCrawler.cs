using HSM;
using PrimeTween;
using UnityEngine;

namespace Enemies
{
    public class CrystalCrawler : EnemyController
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