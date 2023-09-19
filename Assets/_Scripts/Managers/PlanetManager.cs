using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private PlanetFactory _planetFactory;
    private List<Planet> _planetList = new List<Planet>();

    private void Start()
    {
        _planetFactory = GetComponent<PlanetFactory>();
    }
}
