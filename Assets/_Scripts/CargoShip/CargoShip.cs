using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Branda.Utils;

public class CargoShip : MonoBehaviour, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private SpriteRenderer cargoShipSprite;
    [SerializeField] private float stoppingDistance = 0.4f;

    public CargoShipState CurrentState { get; private set; }

    private Vector3 _currentPosition;
    private Planet _homePlanet;
    private Earth _earth;
    private BalanceManager _balanceManager;
    private BasePlanet _currentDestinationPlanet;
    private float _travelSpeed;
    private float _transferTime;
    private float _transferTimer = 0f;
    private bool _isCurrentDestinationNull;

    private void Start()
    {
        _isCurrentDestinationNull = _homePlanet == null;
        _earth = Earth.Instance;
        _balanceManager = BalanceManager.Instance;
        
        // _basePlanet.cargoTransferSpeedUpdated.AddListener(UpdateTransferTime);
        // CurrentState = CargoShipState.GoingToBasePlanet;
        // SetDestination(_basePlanet);
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    public void Initialize(Planet homePlanet)
    {
        _homePlanet = homePlanet;
        _travelSpeed = _homePlanet.shipTravelSpeed;
        _transferTime = _homePlanet.shipCargoTransferTime;
        SetDestination(_homePlanet);
        CurrentState = CargoShipState.GoingToHomePlanet;
        // PrintCurrentState();
    }

    private void UpdateState()
    {
        if (_isCurrentDestinationNull) return;
        
        _currentPosition = transform.position;

        switch (CurrentState)
        {
            case CargoShipState.GoingToHomePlanet:
                Move();
                break;
            case CargoShipState.Loading:
                TransferCargo();
                break;
            case CargoShipState.GoingToEarth:
                Move();
                break;
            case CargoShipState.Unloading:
                TransferCargo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetDestination(BasePlanet destinationPlanet)
    {
        _currentDestinationPlanet = destinationPlanet;
    }

    private void Move()
    {
        HandleSprite();
        
        float stepMovement = _travelSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(_currentPosition, _currentDestinationPlanet.Position, stepMovement);

        float distance = Vector3.Distance(_currentPosition, _currentDestinationPlanet.Position);
        if (distance < stoppingDistance)
        {
            if (_currentDestinationPlanet == _homePlanet)
            {
                CurrentState = CargoShipState.Loading;
                // PrintCurrentState();
            }
            else
            {
                CurrentState = CargoShipState.Unloading;
                // PrintCurrentState();
            }
        }
    }

    private void TransferCargo()
    {
        AlignShipHorizontally();

        _transferTimer += Time.deltaTime;

        if (_transferTimer > _transferTime) { _transferTimer = _transferTime; } //Round the number to avoid inconsistencies in the normalization below.

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = _transferTimer / _transferTime
        });

        if (_transferTimer >= _transferTime)
        {
            if (CurrentState == CargoShipState.Loading)
            {
                ResetTransferTimer();
                SetDestination(_earth);
                CurrentState = CargoShipState.GoingToEarth;
                // PrintCurrentState();
            }
            else if (CurrentState == CargoShipState.Unloading)
            {
                _balanceManager.AddBalance(_homePlanet.shipCargoAmount);
                ResetTransferTimer();
                SetDestination(_homePlanet);
                CurrentState = CargoShipState.GoingToHomePlanet;
                // PrintCurrentState();
            }
        }
    }

    private void HandleSprite()
    {
        FlipSpriteVertically();
        RotateShipTowardsDestination();
    }

    private void FlipSpriteVertically()
    {
        if (_currentPosition.x > _currentDestinationPlanet.Position.x) { cargoShipSprite.flipY = true; }
        else { cargoShipSprite.flipY = false; }
    }

    private void RotateShipTowardsDestination()
    {
        Vector3 direction = (_currentDestinationPlanet.Position - _currentPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void AlignShipHorizontally()
    {
        if (_currentDestinationPlanet.Position.x < _currentPosition.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            cargoShipSprite.flipY = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            cargoShipSprite.flipY = false;
        }
    }

    private void UpdateTransferTime(float transferTime)
    {
        _transferTime = transferTime;
    }

    private void ResetTransferTimer()
    {
        _transferTimer = 0f;
    }

    private void PrintCurrentState() { print(CurrentState); }
    
    // Source: https://docs.unity3d.com/ScriptReference/UIElements.ProgressBar.html
    public ProgressBar CreateProgressBar()
    {
        var progressBar = new ProgressBar
        {
            title = "Progress",
            lowValue = 0f,
            highValue = 100f,
            value = 0f
        };

        progressBar.schedule.Execute(() =>
        {
            progressBar.value += 2f;
        }).Every(75).Until(() => progressBar.value >= 100f);

        return progressBar;
    }
}
