using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skilles : MenusBaseState
{
    private TMP_Text coins;
    private GameObject textsGameObject;
    private Dictionary<string, TMP_Text> pricesTexts = new();
    private Color buyColor = Color.green;
    private Color notBuyColor = Color.red;


    public override void BeginState(UIManager uiManager)
    {
        base.BeginState(uiManager);
        coins = uiManager.CurrentMenu.transform.Find("Coins").GetComponent<TMP_Text>();
        textsGameObject = uiManager.CurrentMenu.transform.Find("Buttons").gameObject;
        for (int i = 0; i < textsGameObject.transform.childCount; ++i)
        {
            pricesTexts.Add(textsGameObject.transform.GetChild(i).name, textsGameObject.transform.GetChild(i).GetComponentInChildren<TMP_Text>());
            Debug.Log(pricesTexts.GetValueOrDefault(textsGameObject.transform.GetChild(i).name));
        }
        foreach (var price in pricesTexts)
        {
            price.Value.text = GameManager.Instance.Prices.GetValueOrDefault(price.Key).UpgradeValue.ToString();
        }
        UpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        coins.text = GameManager.Instance.Coins.ToString();
        foreach (var price in pricesTexts)
        {
            if (int.Parse(price.Value.text.Trim()) <= GameManager.Instance.Coins)
            {
                Debug.Log(int.Parse(price.Value.text.Trim()));
                price.Value.color = buyColor;
            }
            else
            {
                price.Value.color = notBuyColor;
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        uiManager.ShowPanelEnum(UIManager.menusState.HUD);
    }

}
