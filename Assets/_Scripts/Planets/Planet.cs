using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class Planet : MonoBehaviour
{
    public UnityEvent<float> cargoTransferSpeedUpdated;

    // [SerializeField] private TextMeshProUGUI cargoLoadingTimeDisplay;
    // [SerializeField] private GameObject enabledSprite;
    // [SerializeField] private GameObject disabledSprite;
    
    public string Name;
    public Sprite sprite;
    public Vector3 position;
    public float tripCost = 200f;
    public float cargoAmount = 210f;
    public float cargoTransferSpeed = 40f;

    protected BalanceManager BalanceManager;
    protected InputManager InputManager;
    protected TripManager TripManager;
    protected Earth Earth;
    protected CargoShip CargoShip;

    private CargoShipState _cargoShipState;
    private bool _isEnabled = false;
    private bool _isCargoShipNotNull;

    private void Awake()
    {
        position = gameObject.transform.position;
    }

    public virtual void Start()
    {
        _isCargoShipNotNull = CargoShip != null;
        SetupManagers();
    }
    
    private void Update()
    {
        UpdateAvailability();
    }

    private void SetupManagers()
    {
        BalanceManager = BalanceManager.Instance;
        InputManager = InputManager.Instance;
        TripManager = TripManager.Instance;
        Earth = Earth.Instance;
    }

    public void OnClick()
    {
        if (!_isEnabled) return;
        
        BalanceManager.SubtractBalance(tripCost);

        var cargoShipPrefab = TripManager.prefabListSO.cargoShip.GetComponent<CargoShip>();
        CargoShip = Instantiate(cargoShipPrefab, Earth.Position, Quaternion.identity);
        // CargoShip.arrivedAtDestination.AddListener();
        CargoShip.Initialize(this);
    }

    private void UpdateAvailability()
    {
        if (BalanceManager.Balance >= tripCost)
        {
            _isEnabled = true;
            EnablePlanet();
        }
        else
        {
            _isEnabled = false;
            DisablePlanet();
        }
    }

    private void EnablePlanet()
    {
        // enabledSprite.SetActive(true);
        // disabledSprite.SetActive(false);
    }
    
    private void DisablePlanet()
    {
        // enabledSprite.SetActive(false);
        // disabledSprite.SetActive(true);
    }
}