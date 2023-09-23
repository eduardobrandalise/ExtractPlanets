using UnityEngine;
using System.Collections.Generic;

public class PlanetFactory : MonoBehaviour
{
    public PlanetData planetData;

    private List<string> usedNames = new List<string>();
    private List<Sprite> usedSprites = new List<Sprite>();
    
    public Planet CreatePlanet()
    {
        Planet planet = gameObject.AddComponent<Planet>();

        planet.Name = GetUniqueName();
        planet.enabledSprite = GetUniqueSprite();
        planet.Position = GenerateRandomPosition();
        planet.shipTripCost = GenerateTripCost(planet.Position);

        return planet;
    }

    private string GetUniqueName()
    {
        string name = planetData.planetNames[Random.Range(0, planetData.planetNames.Count)];
        while (usedNames.Contains(name))
        {
            name = planetData.planetNames[Random.Range(0, planetData.planetNames.Count)];
        }
        usedNames.Add(name);
        return name;
    }

    private Sprite GetUniqueSprite()
    {
        Sprite sprite = planetData.planetSprites[Random.Range(0, planetData.planetSprites.Count)];
        while (usedSprites.Contains(sprite))
        {
            sprite = planetData.planetSprites[Random.Range(0, planetData.planetSprites.Count)];
        }
        usedSprites.Add(sprite);
        return sprite;
    }

    private Vector3 GenerateRandomPosition()
    {
        float minRadius = 3f;
        float maxRadius = 5f;
        float minHeight = 3f;
        float maxHeight = 5f;

        // Generate a random position within a specified range (e.g., spherical coordinates)
        float radius = Random.Range(minRadius, maxRadius);
        float angle = Random.Range(0f, 360f);
        float height = Random.Range(minHeight, maxHeight);

        // Convert spherical coordinates to Cartesian
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, height, z);
    }
    
    /// <summary>
    /// The trip cost is generated based on the distance from Earth.
    /// </summary>
    /// <param name="position">Position of the planet being generated.</param>
    /// <returns></returns>
    private float GenerateTripCost(Vector3 position)
    {
        float distanceFromEarth = Vector3.Distance(Earth.Instance.Position, position);
        float tripCost = distanceFromEarth;

        return tripCost;
    }
}
