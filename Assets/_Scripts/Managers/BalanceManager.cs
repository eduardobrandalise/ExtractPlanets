using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    private static BalanceManager _instance;
    public static BalanceManager Instance { get { return _instance; } }

    [field: SerializeField] public float InitialBalance { get; private set; } = 0f;

    private HUDManager _hudManager;

    private float _timer = 0f;

    [field: SerializeField] public float Balance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    private void Start()
    {
        _hudManager = HUDManager.Instance;
        Balance += InitialBalance;
        _hudManager.UpdateBalanceDisplay(Balance);
    }

    private void FixedUpdate()
    {
        UpdateBalanceDisplay();
    }

    private void UpdateBalanceDisplay()
    {
        _hudManager.UpdateBalanceDisplay(Balance);
    }

    public void AddBalance(float amount)
    {
        Balance += amount;
    }
    
    public void SubtractBalance(float amount)
    {
        Balance -= amount;
    }
}
