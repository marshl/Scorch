using UnityEngine;
using System.Collections;

public class TerrainTile : HexTile
{
    public enum TERRAIN_TYPE
    {
        PATH,
        GRASS,
        DIRT,
        FOREST,
    };

    public float fuelLoad;
    public float fireAmount;
}
