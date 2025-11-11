using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UpgradeData
{
    [SerializeField] private string _upgradeName;
    [SerializeField] private int _upgradeLevel;
    [SerializeField] private int _upgradeValue;
    public string UpgradeName
    {
        get => _upgradeName;
        set => _upgradeName = value;
    }

    public int UpgradeLevel
    {
        get => _upgradeLevel;
        set => _upgradeLevel = value;
    }

    public int UpgradeValue
    {
        get => _upgradeValue;
        set => _upgradeValue = value;
    }
}