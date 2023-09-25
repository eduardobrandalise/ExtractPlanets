using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class PlanetFactory : MonoBehaviour
{
    [FormerlySerializedAs("planetData")] public PlanetDataSO planetDataSo;

    private List<string> usedNames = new List<string>();
    private List<Sprite> usedSprites = new List<Sprite>();
    
    public Planet CreatePlanet()
    {
        Planet planet = gameObject.AddComponent<Planet>();

        planet.Name = GetUniqueName();
        planet.enabledSprite = GetUniqueSprite();
        planet.Position = GenerateRandomPosition();
        planet.ShipTripCost = GenerateTripCost(planet.Position);

        return planet;
    }

    private string GetUniqueName()
    {
        string name = planetDataSo.planetNames[Random.Range(0, planetDataSo.planetNames.Count)];
        while (usedNames.Contains(name))
        {
            name = planetDataSo.planetNames[Random.Range(0, planetDataSo.planetNames.Count)];
        }
        usedNames.Add(name);
        return name;
    }

    private Sprite GetUniqueSprite()
    {
        Sprite sprite = planetDataSo.planetSprites[Random.Range(0, planetDataSo.planetSprites.Count)];
        while (usedSprites.Contains(sprite))
        {
            sprite = planetDataSo.planetSprites[Random.Range(0, planetDataSo.planetSprites.Count)];
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
    private BigNumber GenerateTripCost(Vector3 position)
    {
        float distanceFromEarth = Vector3.Distance(Earth.Instance.Position, position);
        float tripCost = distanceFromEarth;

        return new BigNumber(tripCost);
    }
}
