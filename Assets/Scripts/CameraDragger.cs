using UnityEngine;
using System.Collections;

public class CameraDragger : MonoBehaviour
{
    private struct ScreenClick
    {
        public Vector3 cameraPosition;
        public Vector3 inputPosition;
    }

    private ScreenClick lastClick = new ScreenClick();

	void Update ()
    {
	    if ( Input.GetMouseButtonDown( 0 ) )
        {
            this.lastClick.cameraPosition = this.transform.position;
            this.lastClick.inputPosition = Input.mousePosition;
        }

        if ( Input.GetMouseButton( 0 ) )
        {
            Vector3 direction = this.camera.ScreenToWorldPoint(this.lastClick.inputPosition) - this.camera.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = this.lastClick.cameraPosition + direction;
        }

        float scroll = Input.GetAxis( "Mouse ScrollWheel" );
        this.camera.orthographicSize -= scroll;
	}
}
