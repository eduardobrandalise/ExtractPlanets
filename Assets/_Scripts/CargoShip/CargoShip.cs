using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Branda.Utils;

public class CargoShip : MonoBehaviour, IHasProgress
{
    public UnityEvent<float> finishedLoadingCargo;
    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private SpriteRenderer cargoShipSprite;
    [SerializeField] private float transferSpeed;
    [SerializeField] private float travelSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.1f;

    public CargoShipState CurrentState { get; private set; }

    private Vector3 _currentPosition;
    private Planet _basePlanet;
    private Earth _earth;
    private BalanceManager _balanceManager;
    private Planet _currentDestinationPlanet;
    private float _transferTime;
    private float _transferTimer = 0f;
    private float _cargoTransferSpeed;
    private bool _isCurrentDestinationNull;

    private void Start()
    {
        _isCurrentDestinationNull = _basePlanet == null;
        _earth = Earth.Instance;
        _balanceManager = BalanceManager.Instance;
        
        _basePlanet.cargoTransferSpeedUpdated.AddListener(UpdateTransferTime);
        // CurrentState = CargoShipState.GoingToBasePlanet;
        // SetDestination(_basePlanet);
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    public void Initialize(Planet basePlanet)
    {
        _basePlanet = basePlanet;
        _cargoTransferSpeed = _basePlanet.cargoTransferSpeed;
        _transferTime = _basePlanet.cargoAmount / transferSpeed;
        SetDestination(_basePlanet);
        CurrentState = CargoShipState.GoingToBasePlanet;
        // PrintCurrentState();
    }

    public void UpdateState()
    {
        if (_isCurrentDestinationNull) return;
        
        _currentPosition = transform.position;

        switch (CurrentState)
        {
            case CargoShipState.GoingToBasePlanet:
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

    private void SetDestination(Planet destinationPlanet)
    {
        _currentDestinationPlanet = destinationPlanet;


    }

    private void Move()
    {
        HandleSprite();
        
        float stepMovement = travelSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(_currentPosition, _currentDestinationPlanet.position, stepMovement);

        float distance = Vector3.Distance(_currentPosition, _currentDestinationPlanet.position);

        if (distance < stoppingDistance)
        {
            if (_currentDestinationPlanet == _basePlanet)
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

    private void HandleSprite()
    {
        FlipSpriteVertically();
        RotateShipTowardsDestination();
    }

    private void FlipSpriteVertically()
    {
        if (_currentPosition.x > _currentDestinationPlanet.position.x) { cargoShipSprite.flipY = true; }
        else { cargoShipSprite.flipY = false; }
    }

    private void RotateShipTowardsDestination()
    {
        Vector3 direction = (_currentDestinationPlanet.position - _currentPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void AlignShipHorizontally()
    {
        if (_currentDestinationPlanet.position.x < _currentPosition.x)
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

    private void TransferCargo()
    {
        AlignShipHorizontally();

        _transferTimer += Time.deltaTime;

        if (_transferTimer > _transferTime) { _transferTime = Mathf.Floor(_transferTime); } //Round down the time to avoid problems in the comparison below.

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
                _balanceManager.AddBalance(_basePlanet.cargoAmount);
                ResetTransferTimer();
                SetDestination(_basePlanet);
                CurrentState = CargoShipState.GoingToBasePlanet;
                // PrintCurrentState();
            }
        }
    }

    private void UpdateTransferTime(float transferTime)
    {
        _cargoTransferSpeed = transferTime;
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
