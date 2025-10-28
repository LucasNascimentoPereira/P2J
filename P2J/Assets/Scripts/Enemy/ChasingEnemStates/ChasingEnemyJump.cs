using UnityEngine;

public class ChasingEnemyJump : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Rb.AddForce(new Vector2(chasingEnemy.Rb.linearVelocityX, chasingEnemy.ChasingEnemySata.Jump), ForceMode2D.Force);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.RESTING);
    }
}
