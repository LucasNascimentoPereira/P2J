using UnityEngine;

public class ChasingEnemyBaseState
{
    protected ChasingEnemy chasingEnemy;
    public virtual void BeginState(ChasingEnemy enemy)
    {
        chasingEnemy = enemy;
    }

    public virtual void UpdateState()
    {
        
    }

    public virtual void ExitState()
    {
        chasingEnemy = null;
    }
}
