using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] private PlanetFactory planetFactory;
    
    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); }
        else { instance = this; }
    }

    private void Start()
    {
        Planet newPlanet = planetFactory.CreatePlanet();
    }

    private void InstantiatePlanet(Planet planet)
    {
        // GameObject planetObject = Instantiate(planet, planet.Position, Quaternion.identity);
        // planetObject.GetComponent<PlanetController>().Initialize(newPlanet);
    }
}
