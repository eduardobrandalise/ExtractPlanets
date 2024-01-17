using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeButtonUI : Button
{
    [SerializeField] TextMeshProUGUI costText;

    private TextMeshProUGUI titleText;
    private Image iconImage;

    public void UpdateCostText(BigNumber amount)
    {
        costText.text = amount.GetValue(2);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}