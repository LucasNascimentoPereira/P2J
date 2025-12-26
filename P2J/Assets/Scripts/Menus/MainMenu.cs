using UnityEngine;

public class MainMenu : MenusBaseState
{
    //private Camera _camera;

    public override void BeginState(GameObject menu, GameObject currentMenu, GameObject previousMenu)
    {
        base.BeginState(menu, currentMenu, previousMenu);
        Debug.Log(menu);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
