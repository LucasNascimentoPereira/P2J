using UnityEngine;

public class ChasingEnemyJump : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Rb.AddForceY(chasingEnemy.ChasingEnemySata.Jump, ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.RESTING);
    }
}
