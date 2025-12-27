using UnityEngine;

public class MainLevel : MenusBaseState
{
    //private Camera _camera;

    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        GameManager.Instance.PauseGame(false);
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
