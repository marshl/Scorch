using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HexTest : MonoBehaviour
{
    public int displayWidth;
    public int displayHeight;

    private int internalWidth;
    public float scale;

    public List<List<HexTile>> cellTable;

    public GameObject hexagonPrefab;

    private void Start()
    {
        this.internalWidth = (displayWidth * 3) / 2;
        this.cellTable = new List<List<HexTile>>( this.displayHeight );

        for ( int y = 0; y < this.displayHeight; ++y )
        {
            this.cellTable.Add( new List<HexTile>( this.internalWidth ) );

            for ( int x = 0; x < this.internalWidth; ++x )
            {
                if ( this.IsCellDisplayable( x, y ) )
                {
                    GameObject hex = GameObject.Instantiate( this.hexagonPrefab, HexTile.GetCentre( x, y, this.scale ), Quaternion.identity ) as GameObject;
                    HexTile tileScript = hex.GetComponent<HexTile>();
                    tileScript.x = x;
                    tileScript.y = y;
                    this.cellTable[y].Add( tileScript );
                    hex.transform.localScale *= this.scale / 1.154f;
                    /*HexTile.CubeCoord c = new HexTile.CubeCoord( new HexTile.AxialCoord( x, y ) );
                    tileScript.cubeCoord  = new Vector3( c.x, c.y, c.z );*/
                }
                else
                {
                    this.cellTable[y].Add( null );
                }
            }
        }

        this.BuildNeighbourConnections();
    }

    public HexTile GetCell( int _x, int _y )
    {
        return this.cellTable[_y][_x];
    }

    public HexTile GetCellAxial( HexTile.AxialCoord _c )
    {
        return this.GetCell( _c.q, _c.r );
    }

    public bool IsCellDisplayable( int _x, int _y )
    {
        return _x - _y / 2 >= 0
            && _x - _y / 2 < this.displayWidth;
    }

    public bool IsValidCellIndex( int _x, int _y )
    {
        return _x >= 0 && _x < this.internalWidth - 1
            && _y >= 0 && _y < this.displayHeight - 1;
    }

    public void BuildNeighbourConnections()
    {
        HexTile.CELL_DIRECTION[] directions = (HexTile.CELL_DIRECTION[])Enum.GetValues( typeof( HexTile.CELL_DIRECTION ) );
        for ( int y = 0; y < this.displayHeight; ++y )
        {
            for ( int x = 0; x < this.internalWidth; ++x )
            {
                HexTile tile = this.cellTable[y][x];
                if ( tile == null )
                    continue;

                foreach ( HexTile.CELL_DIRECTION dir in directions )
                {
                    HexTile.AxialCoord coord = new HexTile.AxialCoord( x, y );
                    coord = coord.AddDirection( dir );
                    if ( this.IsValidCellIndex( coord.r, coord.q ) )
                    {
                        tile.neighbourMap.Add( dir, this.GetCellAxial( coord ) );
                    }
                }
            }
        }
    }
}
