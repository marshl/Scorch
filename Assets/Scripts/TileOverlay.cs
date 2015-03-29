using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileOverlay : MonoBehaviour
{
    public GameObject highlightPrefab;
    public GameObject fireOverlayPrefab;

    private GameObject highlightOverlayObj;

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
    }

    private void AddFireOverlay( FIRE_DISPLAY_TIER _tier, int _offset, float _scale )
    {
        GameObject fireOverlay = GameObject.Instantiate( this.fireOverlayPrefab, this.transform.position + new Vector3( 0, 0, -_offset ), this.transform.rotation ) as GameObject;
        fireOverlay.transform.localScale = Vector3.one * _scale;
        this.fireOverlayMap.Add( _tier, fireOverlay );
        fireOverlay.SetActive( false );
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
