namespace Enemies
{
    public class CrystalCrawler : EnemyController
    {
        protected override void InitializeStateMachine()
        {
            stateMachine.AddStates(
                new EnemyIdleState(this),
                new EnemyRoamState(this, 1, obstacleLayer),
                new EnemyChaseState(this),
                new EnemyAttackState(this)
            );
            
            stateMachine.ChangeStateTo<EnemyIdleState>();
            isInterruptable = false;
            doesChase = true;
        }
    }
}