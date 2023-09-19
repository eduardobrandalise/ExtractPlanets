using UnityEngine;
using UnityEngine.Serialization;

public class TripManager : MonoBehaviour
{
    private static TripManager _instance;
    public static TripManager Instance { get { return _instance; } }

    [SerializeField] public PrefabListSO prefabListSO;
    
    private BalanceManager _balanceManager;
    
    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    private void Start()
    {
        _balanceManager = BalanceManager.Instance;
    }

    public void OrderTrip(Planet planet)
    {
        
    }
}
