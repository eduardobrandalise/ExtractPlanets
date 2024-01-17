using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    private static BalanceManager _instance;
    public static BalanceManager Instance { get { return _instance; } }

    [field: SerializeField] public float InitialBalance { get; private set; } = 0f;
    // [field: SerializeField] public float Balance { get; private set; }
    
    public BigNumber Balance { get { return _balance; } }
    private BigNumber _balance = new BigNumber(0,0);
    
    private HUDManager _hudManager;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    private void Start()
    {
        _balance.Add(new BigNumber(InitialBalance,0));
        
        _hudManager = HUDManager.Instance;
        
        UpdateBalanceDisplay();
    }

    private void FixedUpdate()
    {
        UpdateBalanceDisplay();
    }

    private void UpdateBalanceDisplay()
    {
        _hudManager.UpdateBalanceDisplay(_balance);
    }

    public void AddBalance(BigNumber amount)
    {
        _balance.Add(amount);
        // print(_balance.Value);
        // print(_balance.Exponent);
    }
    
    public void SubtractBalance(BigNumber amount)
    {
        _balance.Subtract(amount);
    }
}
