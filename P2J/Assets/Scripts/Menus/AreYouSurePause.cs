using UnityEngine;

public class AreYouSurePause : MenusBaseState
{
    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
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
