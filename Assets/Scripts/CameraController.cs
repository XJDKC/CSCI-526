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
    [Flags]
    public enum RotationState { Horizontal = 0, Vertical = 1, Rotating = 2 };

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
    private float _camMaxSize = 100f;
    private float _zoomVelocity = 0.0f;
    private Vector3 _moveVelocity = Vector3.zero;

    private bool _anchored;
    private Vector3 _buttonLeftPos;
    private Vector3 _topRightPos;

    private float _prevAngle;
    private RotationState _prevState;
    private RotationState _currState;

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

            // camera's angle
            _prevAngle = 0f;

            // init current camera rotation state
            _currState = RotationState.Horizontal;

            _prevAngle = 0;
        }

        if (anchorPoints.topRightPoint && anchorPoints.buttonLeftPoint)
        {
            _anchored = true;
            _camMaxSize = (anchorPoints.topRightPoint.position.y - anchorPoints.buttonLeftPoint.position.y) / 2;
            _buttonLeftPos = anchorPoints.buttonLeftPoint.position;
            _topRightPos = anchorPoints.topRightPoint.position;
        }
    }

    void Update()
    {
        var eulerAngles = transform.eulerAngles;

        // _currState = eulerAngles.z == 0f || Math.Abs(eulerAngles.z - 180f) < 0.1f ? RotationState.Horizontal : RotationState.Vertical;

        // Debug.Log(transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z);
    }

    private void LateUpdate()
    {
        var currAngle = transform.eulerAngles.z;
        bool rotating = Mathf.Abs(_prevAngle - currAngle) > 0;
        _prevAngle = currAngle;

        if (rotating)
        {
            _currState = RotationState.Rotating;
        }

        if (currAngle == 0 || Math.Abs(currAngle - 180) < 0.01f)
        {
            _currState = RotationState.Horizontal;
        }
        else if (Math.Abs(currAngle - 90) < 0.01f || Math.Abs(currAngle - 270) < 0.01f)
        {
            _currState = RotationState.Vertical;
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

        // var nextCameraSize = Mathf.Max(nextCameraSizeByHeight, nextCameraSizeByWidth);
        // var nextCameraSize = Mathf.Max(nextCameraSizeByHeight, nextCameraSizeByWidth);
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
        var playerPos1 = _player1.transform.position;
        var playerPos2 = _player2.transform.position;
        float midX = (playerPos1.x + playerPos2.x) / 2;
        float midY = (playerPos1.y + playerPos2.y) / 2;


        // camera position could not leave the bounded area, if applied by two anchor points
        if (_anchored)
        {
            float cameraHeight = _camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * _camera.aspect;

            // eulerAngles.z
            if (_currState == RotationState.Horizontal)
            {
                var minX = _buttonLeftPos.x + cameraWidth / 2;
                var maxX = _topRightPos.x - cameraWidth / 2;
                if (minX < maxX) midX = Mathf.Clamp(midX, minX, maxX);

                var minY = _buttonLeftPos.y + cameraHeight / 2;
                var maxY = _topRightPos.y - cameraHeight / 2;
                if (minY < maxY) midY = Mathf.Clamp(midY, minY, maxY);
            }
            else if (_currState == RotationState.Vertical)
            {
                // midX = Mathf.Clamp(midX, _buttonLeftPos.y + cameraWidth / 2, _topRightPos.y - cameraWidth / 2);
            }
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

    /**
     * public interface to get camera rotating status
     *
     * return: Horizontal = 0, Vertical = 1, Rotating = 2
     *
     */
    public int GetCameraStatus()
    {
        return (int)_currState;
    }
}
