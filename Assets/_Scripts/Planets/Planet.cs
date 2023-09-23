using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class Planet : BasePlanet, IClickable
{
    // public UnityEvent<float> cargoTransferSpeedUpdated;

    [SerializeField] private CargoShip cargoShip;
    // [SerializeField] private Button updateButton;
    // [SerializeField] private TextMeshProUGUI cargoLoadingTimeDisplay;
    // [SerializeField] private GameObject enabledSprite;
    // [SerializeField] private GameObject disabledSprite;

    public Sprite enabledSprite;
    public Sprite disabledSprite;
    public float shipTravelSpeed;
    public float shipTripCost = 200f;
    public float shipCargoAmount = 210f;
    public float shipCargoTransferTime = 5f;
    
    protected BalanceManager BalanceManager;
    protected Earth Earth;
    
    private CargoShipState _cargoShipState;
    private UpgradeButton _upgradeButton;
    private Vector3 _upgradeButtonPositionAnchor;
    private bool _isEnabled = false;

    public Planet(string name, Vector3 position) : base(name, position)
    {
        Name = name;
        Position = position;
    }

    public void Start()
    {
        SetupManagers();

        Position = transform.position;
    }

    private void Update()
    {
        UpdateAvailability();
    }

    private void SetupManagers()
    {
        BalanceManager = BalanceManager.Instance;
        Earth = Earth.Instance;
    }

    public void Clicked()
    {
        if (!_isEnabled) return;

        BalanceManager.SubtractBalance(shipTripCost);

        InstantiateCargoShip();
    }

    private void InstantiateCargoShip()
    {
        Vector3 earthPosition = Earth.Position;
        cargoShip = Instantiate(cargoShip, earthPosition, Quaternion.identity);
        cargoShip.Initialize(this);
    }

    private void UpdateAvailability()
    {
        if (BalanceManager.Balance >= shipTripCost)
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

    private void IncreaseTravelSpeed()
    {
        shipTravelSpeed = shipTravelSpeed + 0.4f;
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