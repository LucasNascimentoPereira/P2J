using UnityEngine;

public class FlyingEnemyLunging : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        Debug.Log("Enemy lungung");
    }

    public override void UpdateState()
    {
        flyingEnemy.Dir = flyingEnemy.Player.transform.position - flyingEnemy.gameObject.transform.position;
        flyingEnemy.Rb.linearVelocity = new Vector2(flyingEnemy.Dir.normalized.x * flyingEnemy.ChasingEnemySata.LungeSpeed, flyingEnemy.Rb.linearVelocityY);
    }

    public override void ExitState()
    {
        if (flyingEnemy.DetectedPlayerCharacter)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.SHOOT);
        }
    }
}
