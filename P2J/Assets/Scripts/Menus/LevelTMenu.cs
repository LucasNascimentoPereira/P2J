using UnityEngine;
using System.Collections;

public class LevelTMenu : MenusBaseState
{
	private float idleTime = 5.0f;
	private float idleTimeCutscene = 87.0f;	
    public override void BeginState(UIManager uiManager)
    {
	    base.BeginState(uiManager);
	    if (uiManager.IsSkipCutscene)
	    {
		    uiManager.BeginIdleTime(idleTime);
		    uiManager.CurrentMenu.transform.GetChild(0).gameObject.SetActive(true);
	    }
	    else
	    {
		    uiManager.BeginIdleTime(idleTimeCutscene);
		    uiManager.CurrentMenu.transform.GetChild(0).gameObject.SetActive(true);
		    uiManager.CurrentMenu.transform.GetChild(1).gameObject.SetActive(true);
	    }
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
