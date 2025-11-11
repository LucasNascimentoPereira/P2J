using UnityEngine;

public class FlyingEnemyBaseState
{
    protected FlyingEnemy flyingEnemy;
    public virtual void BeginState(FlyingEnemy enemy)
    {
        flyingEnemy = enemy;
    }

    public virtual void UpdateState()
    {

    }

    public virtual void ExitState()
    {
        flyingEnemy = null;
    }
}
