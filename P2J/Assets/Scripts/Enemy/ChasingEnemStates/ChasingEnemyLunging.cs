using System.Collections;
using UnityEngine;

public class ChasingEnemyLunging : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.BeginIdleTime(chasingEnemy.ChasingEnemySata.LungeTime);
        Debug.Log("enemy lunging");
    }

    public override void UpdateState()
    {
        chasingEnemy.Dir = chasingEnemy.Player.transform.position - chasingEnemy.gameObject.transform.position;
        chasingEnemy.Rb.linearVelocity = new Vector2(chasingEnemy.Dir.normalized.x, chasingEnemy.Rb.linearVelocityY) * chasingEnemy.ChasingEnemySata.LungeSpeed;
    }

    public override void ExitState()
    {
        //chasingEnemy.EndIdleTime();
        if (chasingEnemy.IsJumping)
        {
            chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.JUMPING);
        }
        else
        {
            chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.RESTING);
        }
    }
}
