using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : BasePlanet
{
    private static Sun _instance;
    public static Sun Instance { get { return _instance; } }

    public Sun(string name, Vector3 position) : base(name, position)
    {
        Name = "Sun";
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

