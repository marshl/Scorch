using UnityEngine;
using System.Collections;
using System;

public class AxialCoord
{
    public int q;
    public int r;

    public AxialCoord( int _q, int _r )
    {
        this.q = _q;
        this.r = _r;
    }

    public AxialCoord AddDirection( HexTile.CELL_DIRECTION _dir )
    {
        switch ( _dir )
        {
            case HexTile.CELL_DIRECTION.EAST:
                return new AxialCoord( this.q + 1, this.r );
            case HexTile.CELL_DIRECTION.WEST:
                return new AxialCoord( this.q - 1, this.r );
            case HexTile.CELL_DIRECTION.NORTH_EAST:
                return new AxialCoord( this.q + 1, this.r + 1 );
            case HexTile.CELL_DIRECTION.NORTH_WEST:
                return new AxialCoord( this.q , this.r + 1 );
            case HexTile.CELL_DIRECTION.SOUTH_EAST:
                return new AxialCoord( this.q, this.r - 1 );
            case HexTile.CELL_DIRECTION.SOUTH_WEST:
                return new AxialCoord( this.q - 1, this.r - 1 );
            default:
                throw new ArgumentException();
        }
    }
}
