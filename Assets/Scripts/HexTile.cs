using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTile : MonoBehaviour
{
    public int x;
    public int y;


    public enum CELL_DIRECTION
    {
        NORTH_EAST,
        EAST,
        SOUTH_EAST,
        SOUTH_WEST,
        WEST,
        NORTH_WEST,
    }

    public Dictionary<CELL_DIRECTION, HexTile> neighbourMap = new Dictionary<CELL_DIRECTION, HexTile>();

    public static Vector3 GetCentre( int _x, int _y, float _scale )
    {
        return new Vector3( GetWidth( _scale ) * ( _x - _y * 0.5f ), _scale * _y * 1.5f, 0.0f );
    }

    public static float GetWidth( float _scale )
    {
        return _scale * Mathf.Sqrt( 3 );
    }

    /*public class CubeCoord
    {
        public int x;
        public int y;
        public int z;

        public CubeCoord()
        {
            this.x = this.y = this.z = 0;
        }

        public CubeCoord( int _x, int _y, int _z )
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public CubeCoord( AxialCoord _c )
        {
            this.x = _c.q;
            this.z = _c.r;
            this.y = -_c.q - _c.r;
        }

        public CubeCoord FromDirection( CELL_DIRECTION _dir )
        {
            switch ( _dir )
            {
                case CELL_DIRECTION.EAST:
                    return new CubeCoord( 1, 0, 0 );
                case CELL_DIRECTION.WEST:
                    return new CubeCoord( -1, 0, 0 );
                case CELL_DIRECTION.NORTH_EAST:
                    return new CubeCoord( 0, 1, 0 );
                case CELL_DIRECTION.NORTH_WEST:
                    return new CubeCoord( 1, -1, 0 );
                case CELL_DIRECTION.SOUTH_EAST:
                    return new CubeCoord( -1, 1, 0 );
                case CELL_DIRECTION.SOUTH_WEST:
                    return new CubeCoord( -1, -1, 0 );
                default:
                    throw new KeyNotFoundException();
            }
        }

        /*public CubeCoord GetNeighbour( CELL_DIRECTION _dir )
        {

        }* /

    }*/

    public class AxialCoord
    {
        public int q;
        public int r;

        public AxialCoord( int _q, int _r )
        {
            this.q = _q;
            this.r = _r;
        }

        /*public static AxialCoord FromCubeCoord( CubeCoord _c )
        {
            return new AxialCoord( _c.x + _c.y, _c.z + _c.y );
        }*/
        
        public AxialCoord AddDirection( CELL_DIRECTION _dir )
        {
            switch ( _dir )
            {
                case CELL_DIRECTION.EAST:
                    return new AxialCoord( this.q + 1, this.r );
                case CELL_DIRECTION.WEST:
                    return new AxialCoord( this.q - 1, this.r );
                case CELL_DIRECTION.NORTH_EAST:
                    return new AxialCoord( this.q, this.r + 1 );
                case CELL_DIRECTION.NORTH_WEST:
                    return new AxialCoord( this.q - 1, this.r + 1 );
                case CELL_DIRECTION.SOUTH_EAST:
                    return new AxialCoord( this.q - 1, this.r - 1 );
                case CELL_DIRECTION.SOUTH_WEST:
                    return new AxialCoord( this.q, this.r - 1);
                default:
                    throw new KeyNotFoundException();
            }
        }
    }
}
