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

    // The ratio of the screen, which is used to calculate the camera's width
    private float _aspectRatio;


    // Threshold Ratio of the distance to the length of camera's diagnose. if reached, the camera start zooming
    private const float zoomThresholdRatio = 0.5f;


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
        _customCamSize = Mathf.Lerp(_customCamSize,  DefaultCamSize * _camZoomRatio, 2 * Time.deltaTime);
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _curCamSize, 1.7f * Time.deltaTime);
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
            UpdateCameraSize(camDiagnose, ratioHeight2Diagnose);

            // 2. update the position of camera, (within background if assigned)
            UpdateCameraPosition(cameraWidth, cameraHeight);

        }
    }



    /**
     * Update the camera size based on the distance of two players
     *
     * @Param camDiagnose: length of diagnose of the camera
     * @Param ratioHeight2Diagnose: the ratio of the height to diagnose of the camera
     */
    private void UpdateCameraSize(float camDiagnose, float ratioHeight2Diagnose)
    {
        float nextCamSize = _customCamSize;
        float xyDistance = (player1.transform.position - player2.transform.position).magnitude;
        float distanceRatioXY = xyDistance / camDiagnose;

        // if the ratio of the distance between two players takes zoomThresholdRatio, start zooming
        if (Math.Abs(distanceRatioXY - zoomThresholdRatio) < 0.1f)
        {
            float nextCamDiagnose = xyDistance / zoomThresholdRatio;
            nextCamSize = nextCamDiagnose * ratioHeight2Diagnose / 2;
            // Debug.Log ("distanceRatioY,  zoomThresholdRatioY = " + distanceRatioY + ", " + zoomThresholdRatioY);
        }

        // camera size could not be smaller than the default size;
        if (nextCamSize < _customCamSize)
        {
            // Debug.Log ("Reverse back to default size");
            nextCamSize = _customCamSize;
        }

        // cam.orthographicSize = nextCamSize * camZoomRatio;
        _curCamSize = nextCamSize;
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


        // camera position could not leave the following bounded area, if applied
        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)

        {
            if (midX < anchorPoints.buttonLeftPoint.position.x + cameraWidth / 2)
            {
                midX = anchorPoints.buttonLeftPoint.position.x + cameraWidth / 2;
            }

            if (midX > anchorPoints.topRightPoint.position.x - cameraWidth / 2)
            {
                midX = anchorPoints.topRightPoint.position.x - cameraWidth / 2;

            }

            // for future use, if y-axis is needed

            if (midY > anchorPoints.topRightPoint.position.y - cameraHeight / 2)
            {
                midY = anchorPoints.topRightPoint.position.y - cameraHeight / 2;
            }

            if (midY < anchorPoints.buttonLeftPoint.position.y + cameraHeight / 2)
            {
                midY = anchorPoints.buttonLeftPoint.position.y + cameraHeight / 2;
            }

        }

        var curCamPos = _camTransform.position;
        _camTransform.position = Vector3.Lerp(curCamPos, new Vector3(midX, midY, curCamPos.z), 0.01f);
    }

    /**
     * public interface for manually set the zooming ratio of the camera
     *
     * @param: zoomRatio, recommended to be set between 1 to 2.5
     */
    public void setZoomRatio(float zoomRatio)
    {
        _camZoomRatio = zoomRatio;
        // Debug.Log ("Set Zoom Ratio");
    }


}
