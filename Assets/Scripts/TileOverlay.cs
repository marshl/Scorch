using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileOverlay : MonoBehaviour
{
    public HexTile tile;

    public GameObject overlayPrefab;

    public Sprite fireLowSprite;
    public Sprite fireMedSprite;
    public Sprite fireHighSprite;

    public Sprite mediumFuelSprite;
    public Sprite highFuelSprite;

    public Sprite highlightSprite;

    public GameObject highlightOverlayObj;
    public GameObject fireOverlayObj;
    public GameObject fuelOverlayObj;

    public enum FIRE_DISPLAY_TIER : int
    {
        NO_FIRE,
        FIRE_TIER_1,
        FIRE_TIER_2,
        FIRE_TIER_3,
    }

    public Dictionary<FIRE_DISPLAY_TIER, Sprite> fireOverlayMap;

    public void SetupOverlays()
    {
        this.highlightOverlayObj = this.CreateOverlay( 0.5f );
        this.highlightOverlayObj.GetComponent<SpriteRenderer>().sprite = this.highlightSprite;

        this.fireOverlayObj = this.CreateOverlay( 0.3f );
        this.fireOverlayMap = new Dictionary<FIRE_DISPLAY_TIER, Sprite>();
        this.fireOverlayMap.Add( FIRE_DISPLAY_TIER.FIRE_TIER_1, this.fireLowSprite );
        this.fireOverlayMap.Add( FIRE_DISPLAY_TIER.FIRE_TIER_2, this.fireMedSprite );
        this.fireOverlayMap.Add( FIRE_DISPLAY_TIER.FIRE_TIER_3, this.fireHighSprite );

        this.fuelOverlayObj = this.CreateOverlay( 0.2f );
    }

	private GameObject CreateOverlay( float _level )
	{
        GameObject overlay = GameObject.Instantiate( this.overlayPrefab, this.transform.position, this.transform.rotation ) as GameObject;
		overlay.transform.parent = this.transform;
		overlay.transform.localPosition = new Vector3( 0.0f, 0.0f, -_level );
        overlay.SetActive( false );
		return overlay;
	}

    public void SetFuelOverlay()
    {
        if ( this.tile.terrainData.fuelLoad > 0.4f && this.tile.terrainData.fuelLoad < 0.6f )
        {
            this.fuelOverlayObj.SetActive( true );
            this.fuelOverlayObj.GetComponent<SpriteRenderer>().sprite = this.mediumFuelSprite;
        }
        else if ( this.tile.terrainData.fuelLoad >= 0.6f )
        {
            this.fuelOverlayObj.SetActive( true );
            this.fuelOverlayObj.GetComponent<SpriteRenderer>().sprite = this.highFuelSprite;
        }
        else
        {
            this.fuelOverlayObj.SetActive( false );
        }
    }

    public void SetHighlight( bool _highlighted )
    {
        this.highlightOverlayObj.SetActive( _highlighted );
    }

    public void SetFireOverlay( FIRE_DISPLAY_TIER _tier )
    {
        if ( _tier == FIRE_DISPLAY_TIER.NO_FIRE )
        {
            this.fireOverlayObj.SetActive( false );
        }
        else
        {
            this.fireOverlayObj.SetActive( true );
            this.fireOverlayObj.GetComponent<SpriteRenderer>().sprite = this.fireOverlayMap[_tier];
        }
    }
}
