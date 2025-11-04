using UnityEngine;

public class FlyingEnemyResting : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.Dir = Vector2.zero;
        flyingEnemy.Rb.linearVelocity = Vector2.zero;
        flyingEnemy.BeginIdleTime(flyingEnemy.ChasingEnemySata.RestTime);
        Debug.Log("enemy resting");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        flyingEnemy.EndIdleTime();
        //evade

        if (flyingEnemy.DetectedPlayerCharacter)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.SHOOT);
        }
        else
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.IDLE);
        }

    }
}
