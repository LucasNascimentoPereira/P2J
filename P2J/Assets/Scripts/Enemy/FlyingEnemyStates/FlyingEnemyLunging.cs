using UnityEngine;

public class FlyingEnemyLunging : FlyingEnemyBaseState
{
    public override void BeginState(FlyingEnemy enemy)
    {
        base.BeginState(enemy);
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        //idle
        //shoot
    }
}
