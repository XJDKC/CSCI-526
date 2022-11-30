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
    public enum CameraState { Horizontal = 0, Vertical = 1 }

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

    // Game Object of the two players, assign these in the Inspector
    private GameObject _player1;
    private GameObject _player2;

    // Related Components
    private Camera _camera;
    private Transform _cameraTransform;

    // Control the zoom ratio of the Camera, the larger the value, the broader the view is.
    private float _cameraZoomRatio = 1;
    private float _zoomVelocity = 0.0f;
    private Vector3 _moveVelocity = Vector3.zero;

    private bool _anchored;
    private float _anchorWidth;
    private float _anchorHeight;
    private Vector3 _buttonLeftPos;
    private Vector3 _topRightPos;

    // central middle points of the two anchors
    private Vector3 _midAnchorPos;

    private CameraState _currState = CameraState.Horizontal;


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

        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)
        {
            _anchored = true;
            _buttonLeftPos = anchorPoints.buttonLeftPoint.position;
            _topRightPos = anchorPoints.topRightPoint.position;
            _anchorWidth = _topRightPos.x - _buttonLeftPos.x;
            _anchorHeight = _topRightPos.y - _buttonLeftPos.y;
            _midAnchorPos = new Vector3((_topRightPos.x + _buttonLeftPos.x) / 2,
                (_topRightPos.y + _buttonLeftPos.y) / 2, _cameraTransform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (_camera && _player1 && _player2)
        {
            UpdateCameraSize();
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

        float[] camSizes = { nextCameraSizeByDiagnal, nextCameraSizeByHeight, nextCameraSizeByWidth };
        var nextCameraSize = Mathf.Max(camSizes);

        // camera size should be clamped;
        // nextCameraSize = Mathf.Clamp(nextCameraSize, minimumCameraSize, _camMaxSize);
        nextCameraSize = Mathf.Max(nextCameraSize, minimumCameraSize);

        _camera.orthographicSize =
            Mathf.SmoothDamp(_camera.orthographicSize, nextCameraSize, ref _zoomVelocity, zoomSmoothTime);
    }

    /**
     * Update the camera position based on the relative position of two players, confine it in the background, if applied
     */
    private void UpdateCameraPosition()
    {
        var nextCameraPos = NextPosition(_currState);

        _cameraTransform.position =
            Vector3.SmoothDamp(_cameraTransform.position, nextCameraPos, ref _moveVelocity, moveSmoothTime);
    }


    /**
     * public interface for manually setting the zooming ratio of the camera
     *
     * @param: zoomRatio, recommended to be set between 1 to 2.5
     */
    public void SetZoomRatio(float zoomRatio)
    {
        _cameraZoomRatio = zoomRatio;
    }

    /**
     * public interface for getting the camera state
     *
     * @return: camera rotating state: Horizontal = 0, Vertical = 1
     */
    public CameraState GetCameraState()
    {
        return _currState;
    }

    /**
     * @param: next stage of camera
     * @return: next position(Vector3) based on the next stage of camera
     *
     * Horizontal: next position should be clamped by four directions
     * Vertical: next position doesn't have to be clamped
     *
     */
    public Vector3 NextPosition(CameraState nextState)
    {
        _currState = nextState;

        Vector3 nextPosition = GetMidPosition();
        if (nextState == CameraState.Horizontal)
        {
            nextPosition = ClampPosition(nextPosition);
        }

        return nextPosition;
    }

    /**
     * Get mid position of two players
     */
    private Vector3 GetMidPosition()
    {
        var playerPos1 = _player1.transform.position;
        var playerPos2 = _player2.transform.position;
        float midX = (playerPos1.x + playerPos2.x) / 2;
        float midY = (playerPos1.y + playerPos2.y) / 2;
        return new Vector3(midX, midY, transform.position.z);
    }

    /**
     * clamp positions by anchor points
     */
    private Vector3 ClampPosition(Vector3 nextCameraPos)
    {
        if (!_anchored) return new Vector3(nextCameraPos.x, nextCameraPos.y, transform.position.z);
        var currentCameraWidth = _camera.orthographicSize * _camera.aspect * 2;
        var currentCameraHeight = _camera.orthographicSize * 2;

        // flags for the cameras to have them clamped in x and y axises
        var followPlayersX = _anchorWidth > currentCameraWidth;
        var followPlayersY = _anchorHeight > currentCameraHeight;
        var nextX = nextCameraPos.x;
        var nextY = nextCameraPos.y;
        var minX = _buttonLeftPos.x + currentCameraWidth / 2;
        var maxX = _topRightPos.x - currentCameraWidth / 2;
        nextX = followPlayersX ? Mathf.Clamp(nextX, minX, maxX) : _midAnchorPos.x;

        var minY = _buttonLeftPos.y + currentCameraHeight / 2;
        var maxY = _topRightPos.y - currentCameraHeight / 2;
        nextY = followPlayersY ? Mathf.Clamp(nextY, minY, maxY) : _midAnchorPos.y;

        return new Vector3(nextX, nextY, transform.position.z);
    }
}
