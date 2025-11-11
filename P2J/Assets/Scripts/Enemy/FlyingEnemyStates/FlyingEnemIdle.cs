using UnityEngine;

public class FlyingEnemIdle : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.Move();
        Debug.Log("Enemy idle");
    }

    public override void UpdateState()
    {
        flyingEnemy.Rb.linearVelocity = flyingEnemy.Dir.normalized * flyingEnemy.ChasingEnemySata.IdleSpeed;
    }

    public override void ExitState()
    {
        flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.RESTING);
    }
}
