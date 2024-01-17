using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    
    [SerializeField] private PlanetFactory planetFactory;
    
    public Planet SelectedPlanet { get; private set; }
    
    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); }
        else { instance = this; }
    }

    private void Start()
    {
        // Planet newPlanet = planetFactory.CreatePlanet();
        InputManager.Instance.objectSelected.AddListener(ManageObjectSelected);
    }

    private void OnDestroy()
    {
        InputManager.Instance.objectSelected.RemoveListener(ManageObjectSelected);
    }

    private void ManageObjectSelected(GameObject selectedObject)
    {
        if (selectedObject.TryGetComponent(out Planet planet))
        {
            planet.Clicked();
        }
        
        HUDManager.Instance.UpdateSelectedObject(selectedObject);
    }
}
