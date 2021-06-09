using UnityEngine;

public class WorldObjects : MonoBehaviour
{
    public static WorldObjects Instance;
    public Light sun { get; private set; }
    public GameObject road { get; private set; }
    public GameObject oldRoad { get; private set; }
    public GameObject terrain { get; private set; }
    public GameObject oldTerrain { get; private set; }
    public GameObject outlineRoad { get; private set; }
    private void Awake()
    {
        Instance = this;
        sun = RenderSettings.sun;
        road = GameObject.Find("/Map/Road");
        oldRoad = GameObject.Find("/Map/OldRoad");
        terrain = GameObject.Find("/Map/Terrain");
        oldTerrain = GameObject.Find("/Map/OldTerrain");
        outlineRoad = GameObject.Find("/Map/OutlineRoad");
    }
}
