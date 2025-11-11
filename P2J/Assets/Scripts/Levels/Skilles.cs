using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Skilles : MonoBehaviour
{
    [SerializeField] private TMP_Text coins;
    [SerializeField] private GameObject textsGameObject;
    private Dictionary<string, TMP_Text> pricesTexts = new();
    [SerializeField] private Color buyColor = Color.green;
    [SerializeField] private Color notBuyColor = Color.red;

    private void OnEnable()
    {
        for (int i = 0; i < textsGameObject.transform.childCount; ++i) 
        {
            pricesTexts.Add(textsGameObject.transform.GetChild(i).name, textsGameObject.transform.GetChild (i).GetComponentInChildren<TMP_Text>());
        }
        foreach (var price in pricesTexts)
        {
            price.Value.text = GameManager.Instance.Prices.GetValueOrDefault(price.Key).UpgradeValue.ToString();
        }
    }
    
    

    public void UpdateMenu()
    {
        coins.text = GameManager.Instance.Coins.ToString();
        foreach(var price in pricesTexts)
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
}
