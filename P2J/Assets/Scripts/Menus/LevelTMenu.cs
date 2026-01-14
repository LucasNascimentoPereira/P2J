using UnityEngine;
using System.Collections;

public class LevelTMenu : MenusBaseState
{
	private float idleTime = 5.0f;
	private float idleTimeCutscene = 87.0f;	
    public override void BeginState(UIManager uiManager)
    {
	    base.BeginState(uiManager);
	    uiManager.BeginIdleTime(idleTime);
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
