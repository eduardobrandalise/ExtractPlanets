using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[System.Serializable]
public class Planet : BasePlanet, IClickable
{
    public UnityEvent<Planet> clicked;
    public UnityEvent upgradeApplied;

    [SerializeField] private string planetName;
    [SerializeField] private CargoShip cargoShip;
    [SerializeField] public SpriteRenderer enabledSpriteRenderer;
    [SerializeField] public SpriteRenderer disabledSpriteRenderer;


    protected PlanetManager PlanetManager;
    protected BalanceManager BalanceManager;
    protected Earth Earth;

    private bool _isEnabled = false;
    private bool _isCargoShipDeployed;

    public BigNumber ShipTripCost;
    public BigNumber ShipCargoValue;
    public float travelSpeed = 1f;
    public float transferTime = 5f;
    private int _travelSpeedLevel = 1;
    private int _transferTimeLevel = 1;
    private float _travelSpeedRateGrowth = 1.06f;
    private float _transferTimeRateGrowth = 1.02f;
    private BigNumber _travelSpeedUpgradeBaseCost = new BigNumber(40,0);
    private BigNumber _transferTimeUpgradeBaseCost = new BigNumber(80,0);
    public BigNumber _travelSpeedUpgradeCurrentCost;
    public BigNumber _transferTimeUpgradeCurrentCost;
    public float travelSpeedMultiplier = 1f;
    public float transferTimeMultiplier = 1f;

    public Planet(string name, Vector3 position) : base(name, position)
    {
        Name = name;
        Position = position;
    }

    public void Start()
    {
        SaveReferencesToManagers();

        if (Name != planetName) { Name = planetName; }
        Position = transform.position;
        ShipTripCost = new BigNumber(200,0);
        ShipCargoValue = new BigNumber(500000000000, 0);

        _travelSpeedUpgradeBaseCost = new BigNumber(40, 0);
        _transferTimeUpgradeBaseCost = new BigNumber(80, 0);

        _travelSpeedUpgradeCurrentCost = _travelSpeedUpgradeBaseCost;
        _transferTimeUpgradeCurrentCost = _transferTimeUpgradeBaseCost;        

        PlanetManager.RegisterPlanet(this);
    }

    private void Update()
    {
        UpdateAvailability();
    }

    private void SaveReferencesToManagers()
    {
        PlanetManager = PlanetManager.Instance;
        BalanceManager = BalanceManager.Instance;
        Earth = Earth.Instance;
    }

    public void Clicked()
    {
        print(BalanceManager.Balance.CompareTo(ShipTripCost));
        
        clicked.Invoke(this);

        if (!_isEnabled || _isCargoShipDeployed) return;

        BalanceManager.SubtractBalance(ShipTripCost);

        InstantiateCargoShip();
    }

    private void InstantiateCargoShip()
    {
        Vector3 earthPosition = Earth.Position;
        cargoShip = Instantiate(cargoShip, earthPosition, Quaternion.identity);
        cargoShip.Initialize(this);
        _isCargoShipDeployed = true;
    }

    private void UpdateAvailability()
    {
        if (BalanceManager.Balance.CompareTo(ShipTripCost) == ComparisonResult.Less)
        {
            DisablePlanet();
        }
        else
        {
            EnablePlanet();
        }
    }

    public void UpgradeTravelSpeed()
    {
        _travelSpeedLevel++;
        BalanceManager.SubtractBalance(_travelSpeedUpgradeCurrentCost);
        CalculateProduction(UpgradeType.TravelSpeed);
        CalculateNextLevelCost(UpgradeType.TravelSpeed);
        print(_travelSpeedLevel);
    }

    public void UpgradeTransferTime()
    {
        _transferTimeLevel++;
        BalanceManager.SubtractBalance(_transferTimeUpgradeCurrentCost);
        CalculateProduction(UpgradeType.TransferTime);
        CalculateNextLevelCost(UpgradeType.TransferTime);
        print(_transferTimeLevel);
    }

    private void FireUpgradeAppliedEvent()
    {
        upgradeApplied.Invoke();
    }

    private void IncreaseTravelSpeed()
    {
        travelSpeed = travelSpeed + 0.4f;
    }
    
    private void CalculateNextLevelCost(UpgradeType upgradeType)
    {
        double multiplier;
        BigNumber baseCost;
        
        switch (upgradeType)
        {
            case UpgradeType.TravelSpeed:
                baseCost = _travelSpeedUpgradeBaseCost;
                multiplier = Mathf.Pow(_travelSpeedRateGrowth,_travelSpeedLevel);
                baseCost.Multiply(multiplier);
                _transferTimeUpgradeCurrentCost = baseCost;
                break;
            case UpgradeType.TransferTime:
                baseCost = _transferTimeUpgradeBaseCost;
                multiplier = Mathf.Pow(_transferTimeRateGrowth, _transferTimeLevel);
                baseCost.Multiply(multiplier);
                _transferTimeUpgradeCurrentCost = baseCost;
                break;
        }
    }

    private void CalculateProduction(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.TravelSpeed:
                travelSpeed = (travelSpeed * _travelSpeedLevel) * travelSpeedMultiplier;               
                break;
            case UpgradeType.TransferTime:
                transferTime = (transferTime * _transferTimeLevel) * transferTimeMultiplier;                
                break;
        }
    }

    private void EnablePlanet()
    {
        _isEnabled = true;
        
        enabledSpriteRenderer.gameObject.SetActive(true);
        disabledSpriteRenderer.gameObject.SetActive(false);
    }

    private void DisablePlanet()
    {
        _isEnabled = false;
        
        enabledSpriteRenderer.gameObject.SetActive(false);
        disabledSpriteRenderer.gameObject.SetActive(true);
    }
}