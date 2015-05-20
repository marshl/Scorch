using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimManager : MonoBehaviour
{
    public HexTile.TILE_DIRECTION? windDirection = null;
    public int selfIgnitingStage;
    public int spreadableFireStage;
    public int maximumInfernoStage;

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
            this.selectedTile = GameManager.instance.terrainManager.GetTileClosestTo( _position );
        }
        else
        {
            tile2 = GameManager.instance.terrainManager.GetTileClosestTo( _position );
        }
        b = !b;

        GameManager.instance.terrainManager.RemoveHighlights();
        if ( this.selectedTile != null )
        {
            this.selectedTile.SetHighlight( true );
        }

        if ( this.selectedTile != null && this.tile2 != null )
        {
            this.path = GameManager.instance.terrainManager.FindPath( this.selectedTile, this.tile2 );
        }

        foreach ( HexTile tile in this.path )
        {
            tile.SetHighlight( true );
        }
    }

    public void RunEnvironmentSimulation()
    {
        int randIndex = Random.Range( 0, GameManager.instance.terrainManager.availableTiles.Count );
        GameManager.instance.terrainManager.availableTiles[randIndex].terrainData.fireLevel++;

        foreach ( HexTile tile in GameManager.instance.terrainManager.availableTiles )
        {
            TerrainData terrain = tile.terrainData;
            if ( this.IsTerrainPristine( terrain ) )
                continue;

            if ( this.IsTerrainSelfIgniting( terrain ) )
            {
                this.IncreaseFireLevel( terrain );
            }

            if ( this.IsTerrainFireSpreadable( terrain ) )
            {
                this.SpreadFire( terrain );
            }
        }
    }

    public bool IsTerrainPristine( TerrainData _terrain )
    {
        return _terrain.fireLevel <= 0;
    }

    public bool IsTerrainSelfIgniting( TerrainData _terrain )
    {
        return _terrain.fireLevel >= this.selfIgnitingStage;
    }

    public bool IsTerrainFireSpreadable( TerrainData _terrain )
    {
        return _terrain.fireLevel >= this.spreadableFireStage;
    }

    public void IncreaseFireLevel( TerrainData _terrain )
    {
        _terrain.fireLevel = Mathf.Min( _terrain.fireLevel + 1, this.maximumInfernoStage );
        _terrain.hexTile.tileOverlay.SetFireOverlay( this.FireLevelToDisplayTier( _terrain.fireLevel ) );
    }

    public void SpreadFire( TerrainData _terrain )
    {
        HexTile.TILE_DIRECTION windDirection = this.windDirection == null
              ? HexTile.GetRandomDirection() : this.windDirection.Value;

        if ( !_terrain.hexTile.neighbourMap.ContainsKey( windDirection ) )
            return;

        TerrainData t = _terrain.hexTile.neighbourMap[windDirection].terrainData;
        this.IncreaseFireLevel( t );
    }

    public TileOverlay.FIRE_DISPLAY_TIER FireLevelToDisplayTier( int _fireLevel )
    {
        if ( _fireLevel <= 0 )
        {
            return TileOverlay.FIRE_DISPLAY_TIER.NO_FIRE;
        }
        else if ( _fireLevel < this.selfIgnitingStage )
        {
            return TileOverlay.FIRE_DISPLAY_TIER.FIRE_TIER_1;
        }
        else if ( _fireLevel < this.spreadableFireStage )
        {
            return TileOverlay.FIRE_DISPLAY_TIER.FIRE_TIER_2;
        }
        else
        {
            return TileOverlay.FIRE_DISPLAY_TIER.FIRE_TIER_3;
        }
    }
}

