using UnityEngine;
using System.Collections;

public class CameraDragger : MonoBehaviour
{
    public SimManager simManager;
    public TerrainManager terrainManager;

    private struct ScreenClick
    {
        public Vector3 cameraPosition;
        public Vector3 inputPosition;
    }

    private ScreenClick lastClick = new ScreenClick();

    public float orthoZoomSpeed = 0.5f;
    public float minimumZoom = 1.0f;
    public float maximumZoom = 1.0f;

    public float rotationMultiplier = 1.0f;
    private Vector2 storedRotationLine = Vector2.zero;
    private float storedRotation = 0.0f;

    public Vector2 minPosition;
    public Vector2 maxPosition;

    private float mouseDownTimer = 0.0f;
    public float clickDuration = 0.25f;
    private Vector3 clickPosition;
    public float clickRadius = 5.0f;

    private void Update()
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
        if ( scroll != 0.0f )
        {
            this.camera.orthographicSize = Mathf.Clamp( this.camera.orthographicSize - scroll, this.minimumZoom, this.maximumZoom );
        }

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
        }

        if ( Input.GetMouseButton( 0 ) )
        {
            this.mouseDownTimer += Time.deltaTime;
            this.clickPosition = Input.mousePosition;
        }

        if ( Input.GetMouseButtonUp( 0 ) )
        {
            if ( this.mouseDownTimer < this.clickDuration && (this.clickPosition - Input.mousePosition).magnitude < this.clickRadius )
            {
                this.OnMouseClickEvent();
            }
            this.mouseDownTimer = 0.0f;
        }

        this.minPosition = this.transform.position - this.camera.ViewportToWorldPoint( Vector3.zero )
           + new Vector3( -HexTile.GetWidth( terrainManager.scale ) / 2, -terrainManager.scale / 2, 0 );

        this.maxPosition = ( this.transform.position - this.camera.ViewportToWorldPoint( Vector3.one ) ) +
            new Vector3( HexTile.GetWidth( terrainManager.scale ) * ( terrainManager.displayWidth - 1 ),
                terrainManager.scale * ( terrainManager.displayHeight - 0.75f ) * 1.5f, 0.0f );

        this.transform.position = new Vector3(
            Mathf.Clamp( this.transform.position.x, this.minPosition.x, this.maxPosition.x ),
            Mathf.Clamp( this.transform.position.y, this.minPosition.y, this.maxPosition.y ),
            this.transform.position.z );
    }

    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 0, Screen.height *0.8f, Screen.width, Screen.height * 0.2f), "Stimulate Fire") )
        {
            this.simManager.RunEnvironmentSimulation();
        }
    }

    private void OnMouseClickEvent()
    {
        Vector3 translatedPosition = this.camera.ScreenToWorldPoint( Input.mousePosition );
        this.simManager.SelectTile( translatedPosition );
    }

    private void RotationInput()
    {
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
}
