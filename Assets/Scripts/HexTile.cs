using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTile : MonoBehaviour
{
    public GameObject highlightPrefab;
    public GameObject fireOverlayPrefab;

    public int x;
    public int y;

    public AStarNode astarNode;
    public TerrainData terrainData;

    public bool isHighlighted;
    private GameObject highlightOverlayObj;

    public Dictionary<TerrainData.FIRE_TIER, GameObject> fireOverlayMap;
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

        this.SetupOverlays();
    }

    private void SetupOverlays()
    {
        this.highlightOverlayObj = GameObject.Instantiate( this.highlightPrefab, this.transform.position + new Vector3( 0.0f, 0.0f, -5.0f ), this.transform.rotation ) as GameObject;
        this.highlightOverlayObj.SetActive( false );

        this.fireOverlayMap = new Dictionary<TerrainData.FIRE_TIER, GameObject>();
        this.AddFireOverlay( TerrainData.FIRE_TIER.FIRE_TIER_1, 1, 0.3f );
        this.AddFireOverlay( TerrainData.FIRE_TIER.FIRE_TIER_2, 2, 0.5f );
        this.AddFireOverlay( TerrainData.FIRE_TIER.FIRE_TIER_3, 3, 0.7f );
    }

    private void AddFireOverlay( TerrainData.FIRE_TIER _tier, int _offset, float _scale )
    {
        GameObject fireOverlay = GameObject.Instantiate( this.fireOverlayPrefab, this.transform.position + new Vector3( 0, 0, -_offset ), this.transform.rotation ) as GameObject;
        fireOverlay.transform.localScale = Vector3.one * _scale;
        this.fireOverlayMap.Add( _tier, fireOverlay );
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
        this.isHighlighted = _highlighted;
        this.highlightOverlayObj.SetActive( _highlighted );
    }
}
