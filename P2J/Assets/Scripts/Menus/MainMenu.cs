
public class MainMenu : MenusBaseState
{
    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        uiManager.CameraReference(0);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
        uiManager.ShowPanelEnum(UIManager.menusState.AREYOUSUREEXIT);
    }
}
