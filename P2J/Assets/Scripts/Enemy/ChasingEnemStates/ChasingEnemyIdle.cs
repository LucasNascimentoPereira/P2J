using UnityEngine;

public class ChasingEnemyIdle : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Move();
    }

    public override void UpdateState()
    {
        chasingEnemy.Rb.linearVelocity = new Vector2(chasingEnemy.Dir.normalized.x, chasingEnemy.Rb.linearVelocityY) * chasingEnemy.ChasingEnemySata.ChaseSpeed;
    }

    public override void ExitState()
    {
        chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.RESTING);
    }

}
