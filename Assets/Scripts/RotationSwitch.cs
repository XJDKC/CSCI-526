using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotationSwitch : MonoBehaviour
{
    public enum SwitchState { Left, Right }

    public float angleLimit = 45.0f;
    public float animationTime = 1.0f;
    public SwitchState initState = SwitchState.Left;

    private GameObject _base;
    private GameObject _lever;
    private GameObject _camera;
    private GameObject _player1;
    private GameObject _player2;
    private Rigidbody2D _rigidbody2D;
    private HingeJoint2D _hingeJoint2D;

    private static bool registeredSceneUnloadEvent = false;

    private static readonly float AngleEpsilon = 1.0f;
    private static readonly float RigidBodyMass = 40.0f;
    private static readonly float DivisionNumbers = 256.0f;
    private static readonly float InitForceMagnitude = 1000.0f;
    private static readonly float FrictionForceMagnitude = 10.0f;

    private SwitchState _prevState;
    private SwitchState _currState;
    private Matrix4x4 _rotationMatrix;
    private Vector2 _frictionForce;

    private void Awake()
    {
        var firstChild = transform.GetChild(0);
        var secondChild = transform.GetChild(1);
        if (firstChild && secondChild)
        {
            _base = firstChild.gameObject;
            _lever = secondChild.gameObject;
            _rigidbody2D = _lever.GetComponent<Rigidbody2D>();
            _hingeJoint2D = _lever.GetComponent<HingeJoint2D>();
            _rigidbody2D.mass = RigidBodyMass;
        }

        _camera = FindObjectOfType<Camera>().gameObject;

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


        if (!registeredSceneUnloadEvent)
        {
            registeredSceneUnloadEvent = true;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        _prevState = initState;
        _currState = initState;
        _rotationMatrix = Matrix4x4.Rotate(transform.rotation);
        _frictionForce = _rotationMatrix * new Vector4(0, -FrictionForceMagnitude, 0.0f, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_rigidbody2D || !_hingeJoint2D) return;

        _hingeJoint2D.limits = new JointAngleLimits2D { min = angleLimit, max = -angleLimit };

        var initForce = new Vector2(initState == SwitchState.Left ? -InitForceMagnitude : InitForceMagnitude, 0.0f);
        _rigidbody2D.AddForce(_rotationMatrix * new Vector4(initForce.x, initForce.y, 0.0f, 1.0f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rigidbody2D) return;

        _rigidbody2D.AddForce(_frictionForce * _rigidbody2D.mass);
    }

    private void Update()
    {
        if (!_rigidbody2D || !_hingeJoint2D || !_camera || !_player1 || !_player2) return;

        _prevState = _currState;

        var angleZ = _lever.transform.localRotation.eulerAngles.z;
        angleZ = angleZ <= 180.0f ? angleZ : angleZ - 360.0f;
        if (Mathf.Abs(angleZ - angleLimit) < AngleEpsilon)
        {
            _currState = SwitchState.Left;
        }
        else if (Mathf.Abs(angleZ + angleLimit) < AngleEpsilon)
        {
            _currState = SwitchState.Right;
        }

        if (_currState != _prevState)
        {
            StartCoroutine(ExecuteRotation());
        }
    }

    IEnumerator ExecuteRotation()
    {
        Time.timeScale = 0.0f;

        // Change Gravity
        var prevGravity = Physics2D.gravity;
        var deltaAngle = Quaternion.identity;
        if (_currState == SwitchState.Left)
        {
            deltaAngle = Quaternion.Euler(0.0f, 0.0f, -90.0f);
            Physics2D.gravity = new Vector2(prevGravity.y, -prevGravity.x);
        }
        else
        {
            deltaAngle = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            Physics2D.gravity = new Vector2(-prevGravity.y, prevGravity.x);
        }

        // Do animations
        var elapsedTime = 0.0f;
        var startTime = Time.realtimeSinceStartup;
        var deltaTime = animationTime / DivisionNumbers;
        var cameraPosition = _camera.transform.position;
        var cameraRotation = _camera.transform.localRotation;
        var player1Rotation = _player1.transform.localRotation;
        var player2Rotation = _player2.transform.localRotation;
        var targetPosition = GetCameraTargetPosition();
        while (elapsedTime <= animationTime)
        {
            var t = Mathf.Clamp(elapsedTime / animationTime, 0.0f, 1.0f);
            _camera.transform.position = Vector3.Slerp(cameraPosition, targetPosition, t);
            var targetRotation = deltaAngle * cameraRotation;
            _camera.transform.localRotation = Quaternion.Slerp(cameraRotation, targetRotation, t);
            targetRotation = deltaAngle * player1Rotation;
            _player1.transform.localRotation = Quaternion.Slerp(player1Rotation, targetRotation, t);
            targetRotation = deltaAngle * player2Rotation;
            _player2.transform.localRotation = Quaternion.Slerp(player2Rotation, targetRotation, t);

            yield return new WaitForSecondsRealtime(deltaTime);
            elapsedTime = Time.realtimeSinceStartup - startTime;
        }

        _camera.transform.localRotation = deltaAngle * cameraRotation;
        _player1.transform.localRotation = deltaAngle * player1Rotation;
        _player2.transform.localRotation = deltaAngle * player2Rotation;

        Time.timeScale = 1.0f;
    }

    static void OnSceneUnloaded(Scene scene)
    {
        var gravity = Physics2D.gravity;
        var magnitude = Mathf.Max(Mathf.Abs(gravity.x), Mathf.Abs(gravity.y));
        Physics2D.gravity = new Vector2(0.0f, -magnitude);
    }

    private Vector3 GetCameraTargetPosition()
    {
        var cameraController = _camera.GetComponent<CameraController>();
        var currCameraState = cameraController.GetCameraState();
        if (currCameraState == CameraController.CameraState.Horizontal)
        {
            return cameraController.NextPosition(CameraController.CameraState.Vertical);
        }
        else
        {
            return cameraController.NextPosition(CameraController.CameraState.Horizontal);
        }
    }
}
