
using UnityEngine;

public class MenusBaseState
{
    protected UIManager uiManager;

    public virtual void BeginState(UIManager uiManager)
    {
        this.uiManager = uiManager;
        if (uiManager == null) return;
        if (uiManager.CurrentMenu != null)
        {
            uiManager.CurrentMenu.SetActive(false);
        }
        uiManager.PreviousMenu = uiManager.CurrentMenu;
        Debug.Log((int)uiManager.MenuState);
        uiManager.CurrentMenu = uiManager.PanelsList[(int)uiManager.MenuState];
        uiManager.CurrentMenu.SetActive(true);
    }

    public virtual void UpdateState()
    {

    }

    public virtual void ExitState() 
    {
    }
}
