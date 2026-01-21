using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public void GameEnding()
    {
	    GameManager.Instance.LoadLevel(0);
	    UIManager.Instance.ShowPanelEnum(UIManager.menusState.CREDITSMENU);
    }
}
