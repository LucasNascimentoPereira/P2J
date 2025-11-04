using UnityEngine;

public class FlyingEnemyEvade : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        Debug.Log("enemy resting");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        flyingEnemy.EndIdleTime();
        if (flyingEnemy.DetectedPlayerCharacter)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.LUNGING);
        }
        else
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.IDLE);
        }
    }
}
