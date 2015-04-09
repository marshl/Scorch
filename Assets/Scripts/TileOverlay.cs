using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileOverlay : MonoBehaviour
{
    public HexTile tile;

    public GameObject highlightPrefab;
    public GameObject fireOverlayPrefab;

    public GameObject mediumFuelOverlayPrefab;
    public GameObject highFUleOverlayPrefab;

    private GameObject highlightOverlayObj;

    private GameObject mediumFuelOverlayObj;
    private GameObject highFuelOverlayObj;

    public Dictionary<FIRE_DISPLAY_TIER, GameObject> fireOverlayMap;

    public enum FIRE_DISPLAY_TIER
    {
        NO_FIRE,
        FIRE_TIER_1,
        FIRE_TIER_2,
        FIRE_TIER_3,
    }

    public void SetupOverlays()
    {
        this.highlightOverlayObj = GameObject.Instantiate( this.highlightPrefab, this.transform.position + new Vector3( 0.0f, 0.0f, -5.0f ), this.transform.rotation ) as GameObject;
        this.highlightOverlayObj.SetActive( false );

        this.fireOverlayMap = new Dictionary<FIRE_DISPLAY_TIER, GameObject>();
        this.AddFireOverlay( FIRE_DISPLAY_TIER.FIRE_TIER_1, 1, 0.3f );
        this.AddFireOverlay( FIRE_DISPLAY_TIER.FIRE_TIER_2, 2, 0.5f );
        this.AddFireOverlay( FIRE_DISPLAY_TIER.FIRE_TIER_3, 3, 0.7f );

        this.mediumFuelOverlayObj = GameObject.Instantiate( this.mediumFuelOverlayPrefab, this.transform.position + new Vector3( 0.0f, 0.0f, -2.0f ), this.transform.rotation ) as GameObject;
        this.mediumFuelOverlayObj.SetActive( false );

        this.highFuelOverlayObj = GameObject.Instantiate( this.highFUleOverlayPrefab, this.transform.position + new Vector3( 0.0f, 0.0f, -3.0f ), this.transform.rotation ) as GameObject;
        this.highFuelOverlayObj.SetActive( false );
    }

    private void AddFireOverlay( FIRE_DISPLAY_TIER _tier, int _offset, float _scale )
    {
        GameObject fireOverlay = GameObject.Instantiate( this.fireOverlayPrefab, this.transform.position + new Vector3( 0, 0, -_offset ), this.transform.rotation ) as GameObject;
        fireOverlay.transform.localScale = Vector3.one * _scale;
        this.fireOverlayMap.Add( _tier, fireOverlay );
        fireOverlay.SetActive( false );
    }

    public void SetFuelOverlay()
    {
        if ( this.tile.terrainData.fuelLoad > 0.4f && this.tile.terrainData.fuelLoad < 0.6f )
        {
            this.mediumFuelOverlayObj.SetActive( true );
        }
        else if ( this.tile.terrainData.fuelLoad >= 0.6f )
        {
            this.highFuelOverlayObj.SetActive( true );
        }
    }

    public void SetHighlight( bool _highlighted )
    {
        this.highlightOverlayObj.SetActive( _highlighted );
    }

    public void SetFireOverlay( FIRE_DISPLAY_TIER _tier )
    {
        foreach ( KeyValuePair<FIRE_DISPLAY_TIER, GameObject> pair in this.fireOverlayMap )
        {
            pair.Value.SetActive( pair.Key == _tier );
        }
    }
}
