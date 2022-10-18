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
    private GameObject _player1;
    private GameObject _player2;

    // Anchors to confine the camera , assign these in the Inspector
    public AnchorPoints anchorPoints;

    // Default size of the camera, should be set in the Inspector
    // By definition, in orthographic mode, the camera size is half-size of screen's height
    public float defaultCameraSize = 6.7f;

    // The smoothing time used when changing the camera position
    public float moveSmoothTime = 0.25f;

    // The smoothing time used when zooming the camera
    public float zoomSmoothTime = 0.5f;

    // Zoom ratio by two players, ensure the distance of two players to the length of the screen is not greater than ZoomRatioByPlayers
    private const float ZoomRatioByPlayers = 0.7f;


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

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                _player1 = player;
            }
            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player2)
            {
                _player2 = player;
            }
        }
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
        if (_camera && _player1 && _player2)
        {
            UpdateCameraPosition();
            UpdateCameraSize();

        }
    }

    /**
     * Update the camera size based on the distance of two players
     */
    private void UpdateCameraSize()
    {
        float cameraHeight = _camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * _camera.aspect;

        var pos1 = _player1.transform.position;
        var pos2 = _player2.transform.position;
        float cameraDiagonal = Mathf.Sqrt(cameraHeight * cameraHeight + cameraWidth * cameraWidth);
        float playerDistance = (pos1 - pos2).magnitude;
        float playerDistanceX = Mathf.Abs(pos1.x - pos2.x);
        float playerDistanceY = Mathf.Abs(pos1.y - pos2.y);

        // zoom the camera by the distance of two players
        float nextCameraDiagonal = playerDistance / ZoomRatioByPlayers;
        float nextCameraSizeByDiagnal = (nextCameraDiagonal / cameraDiagonal) * cameraHeight / 2;

        float nextCameraHeight = playerDistanceY / ZoomRatioByPlayers;
        float nextCameraSizeByHeight = nextCameraHeight / 2;

        float nextCameraWidth = playerDistanceX / ZoomRatioByPlayers;
        float nextCameraSizeByWidth = nextCameraWidth / _camera.aspect / 2;

        float minimumCameraSize = defaultCameraSize * _cameraZoomRatio;

        // var nextCameraSize = Mathf.Max(nextCameraSizeByHeight, nextCameraSizeByWidth);
        // var nextCameraSize = Mathf.Max(nextCameraSizeByHeight, nextCameraSizeByWidth);
        float [] camSizes = {nextCameraSizeByDiagnal, nextCameraSizeByHeight, nextCameraSizeByWidth};
        var nextCameraSize = Mathf.Max(camSizes);

        // camera size could not be smaller than the default size;
        if (nextCameraSize < minimumCameraSize)
        {
            nextCameraSize = minimumCameraSize;
        }

        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)
        {
            var camMaxSize = (anchorPoints.topRightPoint.position.y - anchorPoints.buttonLeftPoint.position.y) / 2;
            if (nextCameraSize > camMaxSize)
            {
                nextCameraSize = camMaxSize;
            }
        }

        _camera.orthographicSize =
            Mathf.SmoothDamp(_camera.orthographicSize, nextCameraSize, ref _zoomVelocity, zoomSmoothTime);
    }

    /**
     * Update the camera position based on the relative position of two players, confine it in the background, if applied
     */
    private void UpdateCameraPosition()
    {
        var playerPos1 = _player1.transform.position;
        var playerPos2 = _player2.transform.position;
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
