using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FireTruck : MonoBehaviour
{
    private HexTile attachedTile;

    public void MoveToTile( HexTile _tile )
    {
        this.attachedTile = _tile;
        this.RecalculatePosition();
    }

    private void RecalculatePosition()
    {
        if ( this.attachedTile == null )
        {
            throw new Exception();
        }

        this.transform.position = this.attachedTile.transform.position;
    }
}

