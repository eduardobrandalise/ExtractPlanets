using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlanet : MonoBehaviour
{
    // Properties
    public string Name { get; set; }
    public Vector3 Position { get; set; }

    // Constructor
    public BasePlanet(string name, Vector3 position)
    {
        Name = name;
        Position = position;
    }
}
