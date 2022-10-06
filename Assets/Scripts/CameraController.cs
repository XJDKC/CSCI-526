using System;
using UnityEngine;

[Serializable]
public class AnchorPoints
{
    public Transform topRightPoint;
    public Transform buttonLeftPoint;
}

public class CameraController : MonoBehaviour
{
    // Game Object of the two players, assign these in the Inspector
    public GameObject player1;
    public GameObject player2;

    // Anchors to confine the camera , assign these in the Inspector
    public AnchorPoints anchorPoints;

    // Control the zoom ratio of the Camera, the larger the value, the boarder the view is.
    [Range(1, 3)]
    private float _camZoomRatio = 1;


    // Default size of the camera, should be set in the Inspector
    // by definition, in orthographic mode, the camera size is half-size of screen's height
    private const float DefaultCamSize = 6.7f;
    private float _customCamSize;

    private Camera _cam;
    private Transform _camTransform;
    private float _curCamSize;
    private Vector3 _curCamPos;

    // The ratio of the screen, which is used to calculate the camera's width
    private float _aspectRatio;

    // Zoom ratio by two players, ensure the distance of two players to the length of the screen is not smaller than ZoomRatioByPlayers
    private const float ZoomRatioByPlayers = 0.5f;

    // Factor to control the smoothness when camera size is changing
    private const float ZoomSmoothFactor = 2.0f;

    private Vector3 _velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _camTransform = GetComponent<Transform>();
        _cam = GetComponent<Camera>();

        // get the aspect ration from Screen
        _aspectRatio = Screen.width * 1.0f / Screen.height;
        _customCamSize = DefaultCamSize * _camZoomRatio;


        // initialize the camera attributes
        if (_cam)
        {
            // enable the camera
            _cam.enabled = true;

            //enables the orthographic mode and set its initial size
            _cam.orthographic = true;
            _cam.orthographicSize = DefaultCamSize;
        }
    }

    private void Update()
    {
        _customCamSize = DefaultCamSize * _camZoomRatio;
    }

    void LateUpdate()
    {
        if (_cam && player1 && player2)
        {
            // get current size of the camera
            float curCamSize = _cam.orthographicSize;
            float cameraHeight = curCamSize * 2;
            float cameraWidth = cameraHeight * _aspectRatio;
            float camDiagnose = (float)Math.Sqrt(cameraHeight * cameraHeight + cameraWidth * cameraWidth);
            float ratioHeight2Diagnose = cameraHeight / camDiagnose;

            // 1. update the camera size;
            UpdateCameraSize(ratioHeight2Diagnose);

            // 2. update the position of camera, (within background if assigned)
            UpdateCameraPosition(cameraWidth, cameraHeight);

        }
    }

    /**
     * Update the camera size based on the distance of two players
     *
     * @Param ratioHeight2Diagnose: the ratio of the height to diagnose of the camera
     */
    private void UpdateCameraSize(float ratioHeight2Diagnose)
    {
        float playerDistance = (player1.transform.position - player2.transform.position).magnitude;

        // zoom the camera by the distance of two players
        float nextCamDiagnose = playerDistance / ZoomRatioByPlayers;
        float nextCamSize = nextCamDiagnose * ratioHeight2Diagnose / 2;

        // camera size could not be smaller than the default size;
        if (nextCamSize < _customCamSize)
        {
            // Debug.Log ("Reverse back to default size");
            nextCamSize = _customCamSize;
        }

        // cam.orthographicSize = nextCamSize * camZoomRatio;
        _curCamSize = nextCamSize;

        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _curCamSize, ZoomSmoothFactor * Time.deltaTime);
    }

    /**
     * Update the camera position based on the relative position of two players, confine it in the background, if applied
     *
     * @Param cameraWidth: width of the camera
     * @Param cameraHeight: height of the camera
     */
    private void UpdateCameraPosition(float cameraWidth, float cameraHeight)
    {
        var playerPos1 = player1.transform.position;
        var playerPos2 = player2.transform.position;
        float midX = (playerPos1.x + playerPos2.x) / 2;
        float midY = (playerPos1.y + playerPos2.y) / 2;


        // camera position could not leave the bounded area, if applied by two anchor points
        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)

        {
            // Debug.Log ("Camera Confined");
            var buttonLeftPos = anchorPoints.buttonLeftPoint.position;
            var topRightPos = anchorPoints.topRightPoint.position;
            midX = Mathf.Clamp(midX, buttonLeftPos.x + cameraWidth / 2, topRightPos.x - cameraWidth / 2);

            // for future use, if y-axis is needed
            midY = Mathf.Clamp(midY, buttonLeftPos.y + cameraHeight / 2, topRightPos.y - cameraHeight / 2);

        }

        var curCamPos = _camTransform.position;
        var newCamPos = new Vector3(midX, midY, curCamPos.z);
        // _camTransform.position = Vector3.Lerp(curCamPos, new Vector3(midX, midY, curCamPos.z), 2 * Time.deltaTime);
        // _camTransform.position = newCamPos;

        // _camTransform.position = Vector3.SmoothDamp(_camTransform.position, newCamPos, ref _velocity, 0.1f);
        _camTransform.position = Vector3.Lerp(_camTransform.position, newCamPos, 10f);
    }

    /**
     * public interface for manually set the zooming ratio of the camera
     *
     * @param: zoomRatio, recommended to be set between 1 to 2.5
     */
    public void SetZoomRatio(float zoomRatio)
    {
        _camZoomRatio = zoomRatio;
        // Debug.Log ("Set Zoom Ratio");
    }


}
