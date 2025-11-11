using UnityEngine;

public class FlyingEnemyShoot : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
        flyingEnemy.Dir = Vector2.zero;
        flyingEnemy.Rb.linearVelocity = Vector2.zero;
        flyingEnemy.BeginIdleTime(flyingEnemy.ChasingEnemySata.ShootTime);
        flyingEnemy.ShootBullet();
        Debug.Log("enemy shooting");
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        flyingEnemy.EndIdleTime();
        if (flyingEnemy.DetectedPlayerCharacter)
        {
            flyingEnemy.ChangeState(FlyingEnemy.EnemyStates.RESTING);
        }

    }
}
