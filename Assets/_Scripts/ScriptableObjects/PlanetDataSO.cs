using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetDataSO", menuName = "Custom/Planet Data")]
public class PlanetDataSO : ScriptableObject
{
    public List<string> planetNames;
    public List<Sprite> planetSprites;
}