using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TerrainManager : MonoBehaviour {

    public int displayWidth;
    public int displayHeight;

    private int internalWidth;
    public float scale;

    public List<List<HexTile>> tileTable;

    public GameObject hexagonPrefab;

    public List<HexTile> availableTiles;

    public void Initiaiise()
    {
        this.internalWidth = ( displayWidth * 3 ) / 2;
        this.tileTable = new List<List<HexTile>>( this.displayHeight );

        for ( int y = 0; y < this.displayHeight; ++y )
        {
            this.tileTable.Add( new List<HexTile>( this.internalWidth ) );

            for ( int x = 0; x < this.internalWidth; ++x )
            {
                if ( this.IsValidDisplayableTile( x, y ) )
                {
                    HexTile hex = this.CreateTile( x, y );
                    this.tileTable[y].Add( hex );
                }
                else
                {
                    this.tileTable[y].Add( null );
                }
            }
        }

        this.availableTiles = this.GetAvailableTiles();
        this.BuildNeighbourConnections();

        LibNoise.Perlin perlin = new LibNoise.Perlin();
        perlin.Frequency *= 5.0f;
        Debug.Log( perlin.Frequency + " : " + perlin.Lacunarity + " : " + perlin.NoiseQuality + " : " + perlin.OctaveCount + " : " + perlin.Persistence + " : " + perlin.Seed );
        perlin.Seed = UnityEngine.Random.Range( 0, 1000 );

        float min = float.MaxValue;
        float max = float.MinValue;
        foreach ( HexTile tile in this.availableTiles )
        {
            tile.terrainData.fuelLoad = ((float)perlin.GetValue( (double)tile.x/(double)this.internalWidth, (double)tile.y/(double)this.displayHeight, 0.0 ) + 1.0f) / 2.0f;

            tile.tileOverlay.SetFuelOverlay();
            min = Mathf.Min( min, tile.terrainData.fuelLoad );
            max = Mathf.Max( max, tile.terrainData.fuelLoad );
        }
        Debug.Log( "Min: " + min + " max: " + max );
    }

    /*private void Update()
    {
        Vector3 pos1 = Camera.main.WorldToScreenPoint( this.GetCell( 0, 0 ).gameObject.transform.position );
        Vector3 pos2 = Camera.main.WorldToScreenPoint( this.GetCell( 0, 1 ).gameObject.transform.position );
        Debug.Log( pos1 + " : " + pos2 + " = " + (pos1-pos2).magnitude );
    }*/


    private HexTile CreateTile( int _x, int _y )
    {
        GameObject hex = GameObject.Instantiate( this.hexagonPrefab, HexTile.GetCentre( _x, _y, this.scale ), Quaternion.identity ) as GameObject;
        hex.name = hex.name + "_" + _x + "_" + _y;
        HexTile tileScript = hex.GetComponent<HexTile>();
        tileScript.x = _x;
        tileScript.y = _y;
        //hex.transform.localScale *= this.scale / 1.154f;
        return tileScript;
    }

    public HexTile GetDisplayableTile( int _x, int _y )
    {
        if ( !IsValidDisplayableTile( _x, _y ) )
        {
            throw new ArgumentException( "Invalid tile index: " + _x + " : " + _y );
        }
        return this.tileTable[_y][_x];
    }

    public HexTile GetTileAxial( AxialCoord _c )
    {
        return this.GetDisplayableTile( _c.q, _c.r );
    }

    public bool IsValidDisplayableTile( int _x, int _y )
    {
        return _x - _y / 2 >= 0
            && _x - _y / 2 < this.displayWidth
            && IsValidTileIndex( _x, _y );
    }

    public bool IsValidTileIndex( int _x, int _y )
    {
        return _x >= 0 && _x < this.internalWidth
            && _y >= 0 && _y < this.displayHeight;
    }

    private void BuildNeighbourConnections()
    {
        HexTile.TILE_DIRECTION[] directions = (HexTile.TILE_DIRECTION[])Enum.GetValues( typeof( HexTile.TILE_DIRECTION ) );

        foreach ( HexTile tile in this.availableTiles )
        {
            foreach ( HexTile.TILE_DIRECTION dir in directions )
            {
                AxialCoord coord = new AxialCoord( tile.x, tile.y );
                coord = coord.AddDirection( dir );
                if ( this.IsValidDisplayableTile( coord.q, coord.r ) )
                {
                    tile.neighbourMap.Add( dir, this.GetTileAxial( coord ) );
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
		
            foreach ( KeyValuePair<HexTile.TILE_DIRECTION, HexTile> pair in currentNode.hextile.neighbourMap )
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
                if ( !this.IsValidDisplayableTile( x, y ) )
                    continue;

                HexTile hex = this.tileTable[y][x];
                list.Add( hex );
            }
        }
        return list;
    }

    private void PrepareAStarNodes( AStarNode _endNode )
    {
        foreach ( HexTile tile in this.availableTiles )
        {
            tile.astarNode.Initialise( _endNode );
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
