using System;
using UnityEngine;

public class Earth : BasePlanet
{
    private static Earth _instance;
    public static Earth Instance { get { return _instance; } }

    public Earth(string name, Vector3 position) : base(name, position)
    {
        Name = "Earth";
        Position = position;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); }
        else { _instance = this; }
    }

    private void Start()
    {
        Position = transform.position;
    }

    void OnDestroy()
    {
        if (_instance == this) { _instance = null; }
    }
}
