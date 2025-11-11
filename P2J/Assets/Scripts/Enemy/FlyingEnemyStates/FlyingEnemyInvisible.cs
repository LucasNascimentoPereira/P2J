using UnityEngine;

public class FlyingEnemyInvisible : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.Dir = Vector2.zero;
        flyingEnemy.Rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.IDLE);
    }
}
