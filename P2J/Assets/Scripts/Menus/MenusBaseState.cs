using UnityEngine;

public class MenusBaseState
{
    protected GameObject _currentMenu;
    protected GameObject _previousMenu;
    protected GameObject _menu;

    public virtual void BeginState(GameObject menu, GameObject currentMenu, GameObject previousMenu)
    {
        _currentMenu = currentMenu;
        _previousMenu = previousMenu;
        _menu = menu;
    }

    public virtual void UpdateState()
    {

    }

    public virtual void ExitState() 
    {
        _currentMenu = null;
        _previousMenu = null;
        _menu = null;
    }
}
