using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public void Clicked()
    {
        print(this.name + " was pressed!");
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