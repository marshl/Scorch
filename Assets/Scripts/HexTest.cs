using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTest : MonoBehaviour
{
	public int width;
	public int height;
	public float scale;

	public List<List<Cell>> cellTable;

	public int currentX;
	public int currentY;

	public CubeCoord cubeCoord;

    public GameObject hexagonPrefab;

	private void Start()
	{
		this.cellTable = new List<List<Cell>>( this.height );

		for ( int y = 0; y < this.height; ++y )
		{
			this.cellTable.Add( new List<Cell>( this.width ) );

			for ( int x = 0; x < this.width; ++x )
			{
				Cell c = new Cell();
				this.cellTable[y].Add( c );
				c.x = x;
				c.y = y;

                GameObject.Instantiate(this.hexagonPrefab, c.GetCentre( this.scale ), Quaternion.identity );
			}
		}
		this.cubeCoord = new CubeCoord();
	}

	public void Update()
	{
		for ( int y = 0; y < this.height; ++y )
		{
			for ( int x = 0; x < this.width; ++x )
			{
				Cell c = this.cellTable[y][x];

				Color colour = new Color();
				colour.r = ((float)x) / 100;
				colour.g = ((float)y) / 100;
				colour.b = 0.5f;
				colour.a = 1.0f;

				c.RenderAsHexagon( this.scale, colour );
			}
		}

		this.cellTable [this.currentY] [this.currentX].RenderAsHexagon (1.0f, Color.white );

		AxialCoord axial = AxialCoord.FromCubeCoord (this.cubeCoord);
		if ( axial.q >= 0 && axial.q < this.width
		  && axial.r >= 0 && axial.r < this.height )
		{
			this.cellTable[axial.q][axial.r].RenderAsHexagon( 1.0f, Color.yellow );
		}

		if ( Input.GetKeyDown (KeyCode.UpArrow) )
		{
			this.currentY = Mathf.Min( this.currentY + 1, this.height - 1 );
		}
		else if ( Input.GetKeyDown (KeyCode.DownArrow) )
		{
			this.currentY = Mathf.Max( this.currentY - 1, 0 );
		}

		if ( Input.GetKeyDown (KeyCode.LeftArrow) )
		{
			this.currentX = Mathf.Max( this.currentX - 1, 0 );
		}
		else if ( Input.GetKeyDown (KeyCode.RightArrow) )
		{
			this.currentX = Mathf.Min( this.currentX + 1, this.width - 1 );
		}

		if ( Input.GetKeyDown (KeyCode.Q) )
		{
			++this.cubeCoord.x;
		}
		else if ( Input.GetKeyDown (KeyCode.A) )
		{
			--this.cubeCoord.x;
		}
		
		if ( Input.GetKeyDown (KeyCode.W) )
		{
			++this.cubeCoord.y;
		}
		else if ( Input.GetKeyDown (KeyCode.S) )
		{
			--this.cubeCoord.y;
		}

		if ( Input.GetKeyDown (KeyCode.E) )
		{
			++this.cubeCoord.z;
		}
		else if ( Input.GetKeyDown (KeyCode.D) )
		{
			--this.cubeCoord.z;
		}
	}

	public class Cell
	{
		public int x;
		public int y;

		public void RenderAsSquare( float _scale )
		{
			Debug.DrawLine( new Vector3 (this.x, this.y) * _scale, new Vector3 (this.x + 1, this.y) * _scale);
			Debug.DrawLine( new Vector3 (this.x, this.y) * _scale, new Vector3 (this.x, this.y+1) * _scale);
			Debug.DrawLine( new Vector3 (this.x+1, this.y) * _scale, new Vector3 (this.x + 1, this.y+1) * _scale);
			Debug.DrawLine( new Vector3 (this.x, this.y+1) * _scale, new Vector3 (this.x + 1, this.y+1) * _scale);
		}

		public void RenderAsHexagon( float _scale, Color _colour )
		{
			Vector3 centre = new Vector3 ();
			float w = _scale * Mathf.Sqrt( 3 );
			centre.x = (x * w) - (y * w) / 2.0f;
			centre.y = _scale * y * 3.0f / 2.0f;

			for ( int i = 0; i <= 6; ++i )
			{
				float f1 = i * (Mathf.PI * 2 / 6 );
				float f2 = (i+1) * (Mathf.PI * 2 / 6 );
				Vector3 o1 = new Vector2( Mathf.Sin(f1),Mathf.Cos(f1) );
				Vector3 o2 = new Vector3( Mathf.Sin(f2), Mathf.Cos(f2) );

				Debug.DrawLine( centre + o1 * _scale, centre + o2 * _scale, _colour );
			}
		}

        public Vector3 GetCentre( float _scale )
        {
            Vector3 centre = new Vector3();
            float w = _scale * Mathf.Sqrt(3);
            centre.x = (x * w) - (y * w) / 2.0f;
            centre.y = _scale * y * 3.0f / 2.0f;
            return centre;
        }
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
			CubeCoord coord = new CubeCoord ();
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

		/*public Vector2 GetWorldPosition( float _scale )
		{
			Vector2 output = new Vector2 ();
			float w = this.GetWidth (_scale);
			output.x = r * w + q * w / 2;
			output.y = _scale * q;
			return output;
		}*/

		public float GetWidth( float _scale )
		{
			return _scale * Mathf.Sqrt( 3 );
		}
	}

	/*public class OffsetCoord
	{
		public int x;
		public int y;
	}*/
}
