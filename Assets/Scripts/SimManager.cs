using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SimManager : MonoBehaviour
{
    public TerrainManager terrainManager;

    public HexTile selectedTile;
    public HexTile tile2;

    public bool b;

    public List<HexTile> path;

    public enum STATE
    {
        PLAYER_TURN,
        ENVIRONMENT_TURN,
    }

    public void SelectTile( Vector3 _position )
    {
        if ( b )
        {
            this.selectedTile = this.terrainManager.GetTileClosestTo( _position );
        }
        else
        {
            tile2 = this.terrainManager.GetTileClosestTo( _position );
        }
        b = !b;

        this.terrainManager.RemoveHighlights();
        if ( this.selectedTile != null )
        {
            this.selectedTile.SetHighlight( true );
        }

        if ( this.selectedTile != null && this.tile2 != null )
        {
            this.path = this.terrainManager.FindPath( this.selectedTile, this.tile2 );
        }

        foreach ( HexTile tile in this.path )
        {
            tile.SetHighlight( true );
        }
    }

    public void RunEnvironmentSimulation()
    {
        //foreach ( )
    }
}

