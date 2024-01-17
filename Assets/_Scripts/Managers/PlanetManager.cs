using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlanetManager : MonoBehaviour
{
    private static PlanetManager _instance;
    public static PlanetManager Instance { get { return _instance; } }

    public UnityEvent<Planet> planetWasSelected;
    
    private PlanetFactory _planetFactory;
    private List<Planet> _planetList = new List<Planet>();
    public Planet SelectedPlanet { get; private set; }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    private void Start()
    {
        _planetFactory = GetComponent<PlanetFactory>();
    }

    void OnDestroy()
    {
        if (_instance == this) { _instance = null; }

        foreach (Planet planet in _planetList)
        {
            planet.clicked.RemoveListener(UpdateSelectedPlanet);
        }
    }

    public void RegisterPlanet(Planet planet)
    {
        _planetList.Add(planet);
        planet.clicked.AddListener(UpdateSelectedPlanet);
    }

    public void UpdateSelectedPlanet(Planet planet)
    {
        SelectedPlanet = planet;
        planetWasSelected.Invoke(planet);
    }
}
