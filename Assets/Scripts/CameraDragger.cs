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

    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    public float rotationMultiplier = 1.0f;
    private Vector2 storedRotationLine = Vector2.zero;
    private float storedRotation = 0.0f;

    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            this.lastClick.cameraPosition = this.transform.position;
            this.lastClick.inputPosition = Input.mousePosition;
        }

        if ( Input.GetMouseButton( 0 ) )
        {
            Vector3 direction = this.camera.ScreenToWorldPoint( this.lastClick.inputPosition ) - this.camera.ScreenToWorldPoint( Input.mousePosition );
            this.transform.position = this.lastClick.cameraPosition + direction;
        }

        float scroll = Input.GetAxis( "Mouse ScrollWheel" );
        this.camera.orthographicSize -= scroll;

        // If there are two touches on the device...
        if ( Input.touchCount == 2 )
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch( 0 );
            Touch touchOne = Input.GetTouch( 1 );

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = ( touchZeroPrevPos - touchOnePrevPos ).magnitude;
            float touchDeltaMag = ( touchZero.position - touchOne.position ).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the orthographic size based on the change in distance between the touches.
            camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            camera.orthographicSize = Mathf.Max( camera.orthographicSize, 0.1f );


            /*Vector2 touchGap = touchZero.position - touchOne.position;
            Vector2 gapDir = touchGap.normalized;
            float dir0 = Vector2.Dot(gapDir, touchZero.deltaPosition);
            float dir1 = Vector2.Dot(gapDir, touchOne.deltaPosition);

            if ( dir0 < 0 && dir1 > 0 )
            {
                this.transform.Rotate(this.transform.forward, touchOne.deltaPosition.magnitude);
            }
            else if ( dir0 > 1 && dir1 < 0 )
            {
                this.transform.Rotate(this.transform.forward, -(touchOne.deltaPosition.magnitude) );
            }*/

            //Vector2 oldGap  = (touchZeroPrevPos - touchOnePrevPos).normalized;
            /*Vector2 currentGap  = (touchZero.position - touchOne.position).normalized;
            float d = Vector2.Dot( new Vector2(1,0), currentGap );

            this.transform.rotation = Quaternion.Euler( 0, 0, d - this.storedRotation );
            */
        }
        /*else
        {
            this.storedRotation = this.transform.rotation.eulerAngles.z;
        }*/

        // When the user initially puts two fingers on the screen, track that angle to measure against for rotation

        if ( Input.touchCount == 2 && this.storedRotationLine.magnitude > 0.0f )
        {
            Vector2 line = ( Input.GetTouch( 0 ).position - Input.GetTouch( 1 ).position ).normalized;
            float r = Mathf.Acos( Vector2.Dot( this.storedRotationLine, line ) );
            Vector2 perp = new Vector2( this.storedRotationLine.y, this.storedRotationLine.x );
            r *= Vector2.Dot( perp, line ) > 0 ? 1.0f : -1.0f;
            this.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, this.storedRotation + r * this.rotationMultiplier );
        }

        if ( Input.touchCount == 2 && ( Input.GetTouch( 0 ).phase == TouchPhase.Began || Input.GetTouch( 1 ).phase == TouchPhase.Began ) )
        {
            this.storedRotationLine = ( Input.GetTouch( 0 ).position - Input.GetTouch( 1 ).position ).normalized;
            this.storedRotation = this.transform.rotation.eulerAngles.z;
        }
    }

    private void OnGUI()
    {
        GUI.Label( new Rect( 5, 5, 250, 50 ), this.storedRotationLine + " : " + this.storedRotation );
    }
}
