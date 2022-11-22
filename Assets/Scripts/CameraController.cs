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
    private float _camMaxSize = 100f;
    private float _zoomVelocity = 0.0f;
    private Vector3 _moveVelocity = Vector3.zero;

    private bool _anchored;
    private Vector3 _buttonLeftPos;
    private Vector3 _topRightPos;


    private CameraState _currState;


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

            // init current camera rotation state
            _currState = CameraState.Horizontal;
        }

        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)
        {
            _anchored = true;
            var blPos = anchorPoints.buttonLeftPoint.position;
            var trPos = anchorPoints.topRightPoint.position;
            _camMaxSize = (trPos.y - blPos.y) / 2;
            _buttonLeftPos = blPos;
            _topRightPos = trPos;
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
        nextCameraSize = Mathf.Clamp(nextCameraSize, minimumCameraSize, _camMaxSize);

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
     * clamp positions by anchor points
     */
    private Vector3 ClampPosition(Vector3 nextCameraPos)
    {
        var midX = nextCameraPos.x;
        var midY = nextCameraPos.y;
        if (_anchored)
        {
            float cameraHeight = _camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _camera.aspect;

            var minX = _buttonLeftPos.x + cameraWidth / 2;
            var maxX = _topRightPos.x - cameraWidth / 2;
            if (minX < maxX) midX = Mathf.Clamp(midX, minX, maxX);

            var minY = _buttonLeftPos.y + cameraHeight / 2;
            var maxY = _topRightPos.y - cameraHeight / 2;
            if (minY < maxY) midY = Mathf.Clamp(midY, minY, maxY);
        }

        return new Vector3(midX, midY, transform.position.z);
    }

    /*
     * Get mid position of two players
     */
    private Vector3 GetMidPostion()
    {
        var playerPos1 = _player1.transform.position;
        var playerPos2 = _player2.transform.position;
        float midX = (playerPos1.x + playerPos2.x) / 2;
        float midY = (playerPos1.y + playerPos2.y) / 2;
        return new Vector3(midX, midY, transform.position.z);
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
     * @param: next stage of camera
     * @return: next position(Vector3) based on the next stage of camera
     *
     * Horizontal: next position should be clamped by four directions
     * Vertical: next position doesn't have to be clamped
     * Rotating: #
     *
     */
    public Vector3 NextPosition(CameraState nextState)
    {
        Vector3 nextPosition = GetMidPostion();
        _currState = nextState;

        if (nextState == CameraState.Vertical)
        {
            return nextPosition;
        }

        if (nextState == CameraState.Horizontal)
        {
            Vector3 clampedPosition = ClampPosition(nextPosition);
            return clampedPosition;
        }

        return transform.position;
    }
}
