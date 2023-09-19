using UnityEngine;

public class Earth : Planet
{
    private static Earth _instance;
    public static Earth Instance { get { return _instance; } }
    
    public Vector3 Position => transform.position;

    public Earth(string name, float tripCost)
    {
        Name = "Earth";
        base.tripCost = 0f;
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    void OnDestroy()
    {
        if (_instance == this) { _instance = null; }
    }

    public override void Start()
    {
        BalanceManager = BalanceManager.Instance;
        InputManager = InputManager.Instance;
        TripManager = TripManager.Instance;
    }
}
