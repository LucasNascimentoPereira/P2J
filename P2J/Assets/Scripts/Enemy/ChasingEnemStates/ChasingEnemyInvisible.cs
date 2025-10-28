using UnityEngine;

public class ChasingEnemyInvisible : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Dir = Vector2.zero;
        chasingEnemy.Rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.IDLE);
    }

}
