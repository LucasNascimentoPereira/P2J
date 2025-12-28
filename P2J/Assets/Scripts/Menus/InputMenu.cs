using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class InputMenu : MenusBaseState
{
    private GameObject bContainer = null;
    private int bContainerChildCount = 0;
    private GameObject tContainer = null;
    private int tContainerChildCount = 0;

    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        bContainer = uiManager.CurrentMenu.transform.Find("Buttons").gameObject;
        bContainerChildCount = uiManager.CurrentMenu.transform.childCount;
        tContainer = uiManager.CurrentMenu.transform.Find("Texts").gameObject;
        tContainerChildCount = uiManager.CurrentMenu.transform.childCount;
        UpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        for (int i = 0; i < tContainerChildCount; i++) 
        {
            tContainer.transform.GetChild(i).GetComponent<TMP_Text>().text = InputSystem.actions.FindAction(uiManager.Binding).bindings[i].ToString();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
