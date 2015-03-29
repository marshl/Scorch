using UnityEngine;
using System.Collections;

public class TerrainData : MonoBehaviour
{
    public enum TERRAIN_TYPE
    {
        PATH,
        GRASS,
        DIRT,
        FOREST,
    };

    public enum FIRE_TIER
    {
        FIRE_TIER_1,
        FIRE_TIER_2,
        FIRE_TIER_3,
    }

    public float fuelLoad;
    public float fireAmount;

    public FireTruck fireTruck;
}
