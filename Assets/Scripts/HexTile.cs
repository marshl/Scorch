using UnityEngine;
using System.Collections;

public class HexTile : MonoBehaviour
{
    public int x;
    public int y;

    public enum CELL_TYPE
    {
        PATH,
        GRASS,
        DIRT,
        FOREST,
    };

    public float fireCoverage;
    public float fuelAmount;

    public static Vector3 GetCentre( int _x, int _y, float _scale )
    {
        return new Vector3( GetWidth( _scale ) * ( _x - _y * 0.5f ), _scale * _y * 1.5f, 0.0f );
    }

    public static float GetWidth( float _scale )
    {
        return _scale * Mathf.Sqrt( 3 );
    }

    public class CubeCoord
    {
        public int x;
        public int y;
        public int z;

        public bool IsValid()
        {
            return x + y + z == 0;
        }

        public static CubeCoord FromAxialCoord( AxialCoord _c )
        {
            CubeCoord coord = new CubeCoord();
            coord.x = _c.q;
            coord.z = _c.r;
            coord.y = -coord.x - coord.z;
            return coord;
        }
    }

    public class AxialCoord
    {
        public int q;
        public int r;

        public AxialCoord( int _q, int _r )
        {
            this.q = _q;
            this.r = _r;
        }

        public static AxialCoord FromCubeCoord( CubeCoord _c )
        {
            return new AxialCoord( _c.x + _c.y, _c.z + _c.y );
        }
    }
}
