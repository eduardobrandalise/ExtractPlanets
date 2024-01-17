using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlanetPanelUI : MonoBehaviour
{
    public UnityEvent upgradeTravelSpeedButtonWasClicked;
    public UnityEvent upgradeTransferTimeButtonWasClicked;

    [SerializeField] private TextMeshProUGUI planetNameText;
    [SerializeField] private TextMeshProUGUI cargoValueText;
    [SerializeField] private UpgradeButtonUI upgradeTravelSpeedButton;
    [SerializeField] private UpgradeButtonUI upgradeTransferTimeButton;

    private InputManager _inputManager;
    private PlanetManager _planetManager;
    private BalanceManager _balanceManager;
    
    private Planet _currentSelectedPlanet;
    private bool _isCurrentSelectedPlanetNull;

    private void Start()
    {
        _planetManager = PlanetManager.Instance;
        _balanceManager = BalanceManager.Instance;

        _isCurrentSelectedPlanetNull = _currentSelectedPlanet == null;
        
        _planetManager.planetWasSelected.AddListener(UpdateSelectedPlanetData);      
        upgradeTravelSpeedButton.onClick.AddListener(UpgradeTravelSpeed);
        upgradeTransferTimeButton.onClick.AddListener(UpgradeTransferTime);        
    }

    private void OnDestroy()
    {
        _planetManager.planetWasSelected.RemoveListener(UpdateSelectedPlanetData);        
        upgradeTravelSpeedButton.onClick.RemoveListener(UpgradeTravelSpeed);
        upgradeTransferTimeButton.onClick.RemoveListener(UpgradeTransferTime);
    }

    private void Update()
    {
        if (_isCurrentSelectedPlanetNull) { return; }
        UpdateButtonStatus();
    }
    
    private void UpdateButtonStatus()
    {
        ComparisonResult _travelSpeedResult = _currentSelectedPlanet._travelSpeedUpgradeCurrentCost.CompareTo(_balanceManager.Balance);
        ComparisonResult _transferTimeResult = _currentSelectedPlanet._travelSpeedUpgradeCurrentCost.CompareTo(_balanceManager.Balance);

        if (_travelSpeedResult == ComparisonResult.Equal || _travelSpeedResult == ComparisonResult.Greater) { upgradeTravelSpeedButton.Disable(); }
        else { upgradeTravelSpeedButton.Enable(); }

        if (_transferTimeResult == ComparisonResult.Equal || _transferTimeResult == ComparisonResult.Greater) { upgradeTransferTimeButton.Disable(); }
        else { upgradeTransferTimeButton.Enable(); }
    }

    private void UpgradeTravelSpeed()
    {
        _currentSelectedPlanet.UpgradeTravelSpeed();
        //print("Upgrade Travel Speed Clicked");
    }

    private void UpgradeTransferTime()
    {
        _currentSelectedPlanet.UpgradeTransferTime();
        //print("Upgrade Transfer Speed Clicked");
    }

    private void UpdateSelectedPlanetData(Planet planet)
    {
        if (_isCurrentSelectedPlanetNull || _currentSelectedPlanet != planet)
        {
            RemoveListener();
            return;
        }

        _currentSelectedPlanet = planet;
        _currentSelectedPlanet.upgradeApplied.AddListener(UpdateButtonData);
        print("Added listener to: " + _currentSelectedPlanet);
        UpdatePlanetData();
    }

    private void UpdatePlanetData()
    {
        planetNameText.text = _currentSelectedPlanet.Name;
        cargoValueText.text = _currentSelectedPlanet.ShipCargoValue.GetValue(0);
        UpdateButtonData();
    }
    private void UpdateButtonData()
    {
        upgradeTravelSpeedButton.UpdateCostText(_currentSelectedPlanet._travelSpeedUpgradeCurrentCost);
        upgradeTransferTimeButton.UpdateCostText(_currentSelectedPlanet._transferTimeUpgradeCurrentCost);
    }

    private void RemoveListener()
    {
        _currentSelectedPlanet.upgradeApplied.RemoveListener(UpdateButtonData);
        print("Removed listener from: " + _currentSelectedPlanet);      
    }
}
