using UnityEngine;

public class MainLevel : MenusBaseState
{
    //private Camera _camera;

    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        GameManager.Instance.PauseGame(false);
        uiManager.CameraReference(1);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        uiManager.ShowPanelEnum(UIManager.menusState.PAUSEMENU);
    }
}
