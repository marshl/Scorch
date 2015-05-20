using UnityEngine;
using System.Collections;

public class TerrainData : MonoBehaviour
{
    public HexTile hexTile;
    public enum TERRAIN_TYPE
    {
        PATH,
        GRASS,
        DIRT,
        FOREST,
    };

    public float fuelLoad;
    public int fireLevel;
    public int tempFireLevel;
}
