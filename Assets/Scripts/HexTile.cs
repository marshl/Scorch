using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTile : MonoBehaviour
{
    public int x;
    public int y;

    public AStarNode astarNode;
    public TerrainData terrainData;
    public TileOverlay tileOverlay;

    public Dictionary<CELL_DIRECTION, HexTile> neighbourMap = new Dictionary<CELL_DIRECTION, HexTile>();

    public enum CELL_DIRECTION
    {
        NORTH_EAST,
        EAST,
        SOUTH_EAST,
        SOUTH_WEST,
        WEST,
        NORTH_WEST,
    }

    private void Awake()
    {
        this.astarNode = new AStarNode( this );

        this.tileOverlay.SetupOverlays();
    }

    public static Vector2 GetCentre( int _x, int _y, float _scale )
    {
        return new Vector2( GetWidth( _scale ) * ( _x - _y * 0.5f ), _scale * _y * 1.5f );
    }

    public static float GetWidth( float _scale )
    {
        return _scale * Mathf.Sqrt( 3 );
    }

    public Vector2 GetNormalisedPosition()
    {
        return HexTile.GetCentre( this.x, this.y, 1.0f );
    }

    public void SetHighlight( bool _highlighted )
    {
        this.tileOverlay.SetHighlight( _highlighted );
    }

    public static CELL_DIRECTION GetRandomDirection()
    {
        CELL_DIRECTION[] directions = (CELL_DIRECTION[])System.Enum.GetValues( typeof( CELL_DIRECTION ) );
        return directions[Random.Range( 0, directions.Length )];
    }
}
