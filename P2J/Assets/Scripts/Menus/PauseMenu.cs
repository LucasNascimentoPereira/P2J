using UnityEngine;

public class PauseMenu : MenusBaseState
{
    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        GameManager.Instance.PauseGame(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        uiManager.ShowPanelEnum(UIManager.menusState.HUD);
    }
}
