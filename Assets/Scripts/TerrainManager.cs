using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerrainManager : MonoBehaviour {

    public int displayWidth;
    public int displayHeight;

    private int internalWidth;
    public float scale;

    public List<List<HexTile>> cellTable;

    public GameObject hexagonPrefab;

    private List<HexTile> availableTiles;

    private void Start()
    {
        this.internalWidth = ( displayWidth * 3 ) / 2;
        this.cellTable = new List<List<HexTile>>( this.displayHeight );

        for ( int y = 0; y < this.displayHeight; ++y )
        {
            this.cellTable.Add( new List<HexTile>( this.internalWidth ) );

            for ( int x = 0; x < this.internalWidth; ++x )
            {
                if ( this.IsCellDisplayable( x, y ) )
                {
                    HexTile hex = this.CreateCell( x, y );
                    this.cellTable[y].Add( hex );
                }
                else
                {
                    this.cellTable[y].Add( null );
                }
            }
        }

        this.availableTiles = this.GetAvailableTiles();
        this.BuildNeighbourConnections();
    }

    /*private void Update()
    {
        Vector3 pos1 = Camera.main.WorldToScreenPoint( this.GetCell( 0, 0 ).gameObject.transform.position );
        Vector3 pos2 = Camera.main.WorldToScreenPoint( this.GetCell( 0, 1 ).gameObject.transform.position );
        Debug.Log( pos1 + " : " + pos2 + " = " + (pos1-pos2).magnitude );
    }*/


    public HexTile CreateCell( int _x, int _y )
    {
        GameObject hex = GameObject.Instantiate( this.hexagonPrefab, HexTile.GetCentre( _x, _y, this.scale ), Quaternion.identity ) as GameObject;
        hex.name = hex.name + "_" + _x + "_" + _y;
        HexTile tileScript = hex.GetComponent<HexTile>();
        tileScript.x = _x;
        tileScript.y = _y;
        //hex.transform.localScale *= this.scale / 1.154f;
        return tileScript;
    }

    public HexTile GetCell( int _x, int _y )
    {
        if ( !IsCellDisplayable( _x, _y ) )
        {
            throw new ArgumentException( "Invalid cell index: " + _x + " : " + _y );
        }
        return this.cellTable[_y][_x];
    }

    public HexTile GetCellAxial( AxialCoord _c )
    {
        return this.GetCell( _c.q, _c.r );
    }

    public bool IsCellDisplayable( int _x, int _y )
    {
        return _x - _y / 2 >= 0
            && _x - _y / 2 < this.displayWidth
            && IsValidCellIndex( _x, _y );
    }

    public bool IsValidCellIndex( int _x, int _y )
    {
        return _x >= 0 && _x < this.internalWidth
            && _y >= 0 && _y < this.displayHeight;
    }

    public void BuildNeighbourConnections()
    {
        HexTile.CELL_DIRECTION[] directions = (HexTile.CELL_DIRECTION[])Enum.GetValues( typeof( HexTile.CELL_DIRECTION ) );

        foreach ( HexTile tile in this.availableTiles )
        {
            foreach ( HexTile.CELL_DIRECTION dir in directions )
            {
                AxialCoord coord = new AxialCoord( tile.x, tile.y );
                coord = coord.AddDirection( dir );
                if ( this.IsCellDisplayable( coord.q, coord.r ) )
                {
                    tile.neighbourMap.Add( dir, this.GetCellAxial( coord ) );
                }
            }
        }
    }

    public HexTile GetTileClosestTo( Vector2 _position )
    {
        HexTile closestTile = null;
        float closestDistance = float.MaxValue;
        foreach ( HexTile tile in this.availableTiles )
        {
            Vector2 pos = HexTile.GetCentre( tile.x, tile.y, this.scale );
            float dist = ( _position - pos ).magnitude;
            if ( dist < closestDistance )
            {
                closestDistance = dist;
                closestTile = tile;
            }
        }

        if ( closestDistance > this.scale )
        {
            return null;
        }

        return closestTile;
    }

    public List<HexTile> FindPath( HexTile _start, HexTile _end )
    {
        this.PrepareAStarNodes( _end.astarNode );

        List<AStarNode> openSet = new List<AStarNode>();
        openSet.Add( _start.astarNode );

        while ( openSet.Count > 0 )
	    {
            openSet.Sort( delegate( AStarNode _a, AStarNode _b ) { return _a.distToDestinationNode.CompareTo( _b.distToDestinationNode); } );
            AStarNode currentNode = openSet[0];

		    if ( currentNode.hextile == _end )
		    {
                List<HexTile> path = new List<HexTile>();
			    do
			    {
				    path.Add( currentNode.hextile );
			    } while( ( currentNode = currentNode.parent ) != null );
			    return path;
		    }

            openSet.RemoveAt( 0 );
            currentNode.isClosed = true;
		
            foreach ( KeyValuePair<HexTile.CELL_DIRECTION, HexTile> pair in currentNode.hextile.neighbourMap )
		    {
                AStarNode neighbour = pair.Value.astarNode;

                if ( neighbour.isClosed )
                {
                    continue;
                }
			
                float tentativeCostToThisPoint = currentNode.costToThisPoint + currentNode.GetCostTo( neighbour );
			    bool tentativeIsBetter = false;

                if ( !openSet.Contains( neighbour ) )
                {
				    openSet.Add( neighbour );
				    tentativeIsBetter = true;
			    }
			    else if( tentativeCostToThisPoint <  neighbour.costToThisPoint )
			    {
				    tentativeIsBetter = true;
			    }

			    if ( tentativeIsBetter )
			    {
                    neighbour.parent = currentNode;
                    neighbour.costToThisPoint = tentativeCostToThisPoint;
			    }
		    }
	    }
        return null;
    }

    private List<HexTile> GetAvailableTiles()
    {
        List<HexTile> list = new List<HexTile>();
        for ( int y = 0; y < this.displayHeight; ++y )
        {
            for ( int x = 0; x < this.internalWidth; ++x )
            {
                if ( !this.IsCellDisplayable( x, y ) )
                    continue;

                HexTile hex = this.GetCell( x, y );
                if ( hex != null )
                {
                    list.Add( hex );
                }
            }
        }
        return list;
    }

    private void PrepareAStarNodes( AStarNode _endNode )
    {
        foreach ( HexTile tile in this.availableTiles )
        {
            tile.astarNode.Prepare( _endNode );
        }
    }

    public void RemoveHighlights()
    {
        foreach ( HexTile tile in this.availableTiles )
        {
            tile.SetHighlight( false );
        }
    }
}
