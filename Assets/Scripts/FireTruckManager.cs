using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FireTruckManager: MonoBehaviour
{
    public GameObject fireTruckPrefab;

    private List<FireTruck> fireTrucks = new List<FireTruck>();

    private void Start()
    {
        GameObject fireTruckObj = GameObject.Instantiate( this.fireTruckPrefab, Vector3.zero, Quaternion.identity ) as GameObject;
        FireTruck fireTruck = fireTruckObj.GetComponent<FireTruck>();

        this.fireTrucks.Add( fireTruck );
        fireTruck.MoveToTile( GameManager.instance.terrainManager.GetDisplayableTile( 0, 0 ) );
    }

    private void Update()
    {

    }
}
