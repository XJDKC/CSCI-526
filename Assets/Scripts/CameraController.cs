using System;
using UnityEngine;

[Serializable]
public class AnchorPoints
{
    public Transform topRightPoint;
    public Transform buttonLeftPoint;
}

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // Game Object of the two players, assign these in the Inspector
    public GameObject player1;
    public GameObject player2;

    // Anchors to confine the camera , assign these in the Inspector
    public AnchorPoints anchorPoints;

    // Default size of the camera, should be set in the Inspector
    // By definition, in orthographic mode, the camera size is half-size of screen's height
    public float defaultCameraSize = 6.7f;

    // The smoothing time used when changing camera position
    public float moveSmoothTime = 0.25f;

    // Factor to control the smoothness when camera size is changing
    public float zoomSmoothTime = 0.5f;

    // Zoom ratio by two players, ensure the distance of two players to the length of the screen is not greater than ZoomRatioByPlayers
    public float zoomRatioByPlayers = 0.5f;


    // Related Components
    private Camera _camera;
    private Transform _cameraTransform;

    // Control the zoom ratio of the Camera, the larger the value, the broader the view is.
    private float _cameraZoomRatio = 1;
    private float _zoomVelocity = 0.0f;
    private Vector3 _moveVelocity = Vector3.zero;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraTransform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize the camera attributes
        if (_camera)
        {
            // enable the camera
            _camera.enabled = true;

            // enables the orthographic mode and set its initial size
            _camera.orthographic = true;
            _camera.orthographicSize = defaultCameraSize;
        }
    }

    void FixedUpdate()
    {
        if (_camera && player1 && player2)
        {
            // 1. update the camera size;
            UpdateCameraSize();

            // 2. update the position of camera, (within background if assigned)
            UpdateCameraPosition();
        }
    }

    /**
     * Update the camera size based on the distance of two players
     */
    private void UpdateCameraSize()
    {
        float cameraHeight = _camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * _camera.aspect;
        float cameraDiagonal = Mathf.Sqrt(cameraHeight * cameraHeight + cameraWidth * cameraWidth);
        float playerDistance = (player1.transform.position - player2.transform.position).magnitude;

        // zoom the camera by the distance of two players
        float nextCameraDiagonal = playerDistance / zoomRatioByPlayers;
        float nextCameraSize = (nextCameraDiagonal / cameraDiagonal) * cameraHeight / 2;

        float minimumCameraSize = defaultCameraSize * _cameraZoomRatio;

        // camera size could not be smaller than the default size;
        if (nextCameraSize < minimumCameraSize)
        {
            nextCameraSize = minimumCameraSize;
        }

        _camera.orthographicSize =
            Mathf.SmoothDamp(_camera.orthographicSize, nextCameraSize, ref _zoomVelocity, zoomSmoothTime);
    }

    /**
     * Update the camera position based on the relative position of two players, confine it in the background, if applied
     */
    private void UpdateCameraPosition()
    {
        var playerPos1 = player1.transform.position;
        var playerPos2 = player2.transform.position;
        float midX = (playerPos1.x + playerPos2.x) / 2;
        float midY = (playerPos1.y + playerPos2.y) / 2;

        // camera position could not leave the bounded area, if applied by two anchor points
        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)
        {
            float cameraHeight = _camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _camera.aspect;
            var buttonLeftPos = anchorPoints.buttonLeftPoint.position;
            var topRightPos = anchorPoints.topRightPoint.position;
            midX = Mathf.Clamp(midX, buttonLeftPos.x + cameraWidth / 2, topRightPos.x - cameraWidth / 2);
            midY = Mathf.Clamp(midY, buttonLeftPos.y + cameraHeight / 2, topRightPos.y - cameraHeight / 2);
        }

        var currCameraPos = _cameraTransform.position;
        var nextCameraPos = new Vector3(midX, midY, currCameraPos.z);
        _cameraTransform.position =
            Vector3.SmoothDamp(currCameraPos, nextCameraPos, ref _moveVelocity, moveSmoothTime);
    }

    /**
     * public interface for manually set the zooming ratio of the camera
     *
     * @param: zoomRatio, recommended to be set between 1 to 2.5
     */
    public void SetZoomRatio(float zoomRatio)
    {
        _cameraZoomRatio = zoomRatio;
    }
}
