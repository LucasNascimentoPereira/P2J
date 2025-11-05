using UnityEngine;

public class FlyingEnemyEvade : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.BeginIdleTime(flyingEnemy.ChasingEnemySata.EvadeTime);
        flyingEnemy.Rb.AddForce((flyingEnemy.gameObject.transform.position - flyingEnemy.Player.transform.position) * flyingEnemy.ChasingEnemySata.EvadeSpeed);
        Debug.Log("enemy resting");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        flyingEnemy.EndIdleTime();
        if (flyingEnemy.DetectedPlayerCharacter)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.SHOOT);
        }
        else
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.RESTING);
        }
    }
}
