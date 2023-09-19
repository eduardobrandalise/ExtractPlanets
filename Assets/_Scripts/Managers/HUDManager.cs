using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public static HUDManager Instance { get { return instance; } }

    [SerializeField] private TextMeshProUGUI moneyDisplay;
    [SerializeField] private TextMeshProUGUI selectedObjectDisplay;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        { Destroy(gameObject); }
        else { instance = this; }
    }

    private void Start()
    {
        selectedObjectDisplay.text = "No object selected.";
    }

    public void UpdateBalanceDisplay(float moneyAmount)
    {
        moneyDisplay.text = moneyAmount.ToString();
    }

    public void UpdateSelectedObject(GameObject selectedObject)
    {
        selectedObjectDisplay.text = "Last selected object: " + selectedObject.name;
    }
}
