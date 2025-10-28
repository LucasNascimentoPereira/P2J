using UnityEngine;

public class ChasingEnemyJump : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Rb.AddForce(new Vector2(chasingEnemy.Rb.linearVelocityX, chasingEnemy.ChasingEnemySata.Jump), ForceMode2D.Impulse);
    }

    public override void UpdateState()
    {
        chasingEnemy.Rb.linearVelocity = new Vector2(chasingEnemy.Rb.linearVelocityX, chasingEnemy.Rb.linearVelocityY);
    }

    public override void ExitState()
    {
        chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.RESTING);
    }
}
