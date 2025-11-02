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

    private void Start()
    {
        for (int i = 0; i < textsGameObject.transform.childCount; ++i) 
        {
            pricesTexts.Add(textsGameObject.transform.GetChild(i).name, textsGameObject.transform.GetChild (i).GetComponent<TMP_Text>());
        }
        foreach (var price in pricesTexts)
        {
            price.Value.text = GameManager.Instance.Prices.GetValueOrDefault(price.Key).ToString();
        }
    }
    private void OnEnable()
    {
        UpdateMenu();
    }

    public void UpdateMenu()
    {
        coins.text = GameManager.Instance.Coins.ToString();
        foreach(var price in pricesTexts)
        {
            if (int.Parse(price.Value.text.Trim()) >= int.Parse(coins.text.Trim()))
            {
                price.Value.color = buyColor;
            }
            else 
            {
                price.Value.color = notBuyColor;
            }
        }
    }
}
