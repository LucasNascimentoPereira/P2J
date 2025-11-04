using UnityEngine;

public class FlyingEnemyLunging : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.BeginIdleTime(flyingEnemy.ChasingEnemySata.LungeTime);
        Debug.Log("enemy lunging");
    }

    public override void UpdateState()
    {
        flyingEnemy.Dir = flyingEnemy.Player.transform.position - flyingEnemy.gameObject.transform.position;
        flyingEnemy.Rb.linearVelocity = new Vector2(flyingEnemy.Dir.normalized.x * flyingEnemy.ChasingEnemySata.LungeSpeed, flyingEnemy.Rb.linearVelocityY);
    }

    public override void ExitState()
    {
        flyingEnemy.EndIdleTime();
        if (flyingEnemy.IsJumping)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.JUMPING);
        }
        else
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.RESTING);
        }
    }
}
