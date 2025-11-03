using System.Collections;
using UnityEngine;

public class ChasingEnemyResting : ChasingEnemyBaseState
{
    public override void BeginState(ChasingEnemy enemy)
    {
        base.BeginState(enemy);
        chasingEnemy.Dir = Vector2.zero;
        chasingEnemy.Rb.linearVelocity = Vector2.zero;
        chasingEnemy.BeginIdleTime(chasingEnemy.ChasingEnemySata.RestTime);
        Debug.Log("enemy resting");
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        chasingEnemy.EndIdleTime();
        if (chasingEnemy.DetectedPlayerCharacter)
        {
            chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.LUNGING);
        }
        else 
        {
            chasingEnemy.ChangeState(ChasingEnemy.EnemyStates.IDLE);
        }
    }
}
