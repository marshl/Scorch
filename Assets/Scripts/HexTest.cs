using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
                }
                else
                {
                    this.cellTable[y].Add( null );
                }
            }
        }
    }

    public bool IsCellDisplayable( int _x, int _y )
    {
        return _x - _y / 2 >= 0
            && _x - _y / 2 < this.displayWidth;
    }
}
