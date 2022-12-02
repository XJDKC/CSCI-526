using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour, IReversible
{
    public enum PlayerType { Player1 = 1, Player2 = 2 };

    public PlayerType playerType = PlayerType.Player1;
    public bool reversed = false;
    public float moveDrag = 50.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 10.0f;
    public RuntimeAnimatorController animatorController1;
    public RuntimeAnimatorController animatorController2;

    [Flags]
    private enum PlayerState { Idle = 0, LeftMoving = 1, RightMoving = 2, OnGround = 4, Jumping = 8, Reversed = 16 };

    private static readonly string[] KeyboardSchemes = { "Keyboard1", "Keyboard2" };

    private float _moveInput;
    private float _jumpInput;
    private Animator _playerAnimator;
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private PlayerState _playerState = PlayerState.Idle;
    private PlayerState _prevMoveState = PlayerState.Idle;

    private bool _skipUpdate = false;
    private Matrix4x4 _rotationMatrix = Matrix4x4.identity;

    private void Awake()
    {
        // Assign control scheme for players
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        if (reversed) Reverse();
    }

    private void Start()
    {
        if (playerType == PlayerType.Player1)
        {
            _playerInput.SwitchCurrentControlScheme(KeyboardSchemes[0], Keyboard.current);
            if (animatorController1)
            {
                _playerAnimator.runtimeAnimatorController = animatorController1;
            }
        }
        else
        {
            _playerInput.SwitchCurrentControlScheme(KeyboardSchemes[1], Keyboard.current);
            if (animatorController2)
            {
                _playerAnimator.runtimeAnimatorController = animatorController2;
            }
        }
    }

    void FixedUpdate()
    {
        if (ShouldSkipUpdate(true)) return;
        UpdateRotationMatrix();
        UpdatePlayerState();
        UpdateMovement();
    }

    void Update()
    {
        if (ShouldSkipUpdate(false)) return;
        UpdateRotationMatrix();
        UpdateRotation();
        UpdateAnimation();
    }


    public void Reverse()
    {
        if (_rigidbody2D) _rigidbody2D.gravityScale *= -1.0f;
        _playerState ^= PlayerState.Reversed;
        UpdateRotation();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValue<float>();
    }

    bool ShouldSkipUpdate(bool fixedUpdate)
    {
        // skip update if the time scale is zero or it is the first frame after unfreezing
        _skipUpdate = _skipUpdate || Time.timeScale == 0.0f;
        if (_skipUpdate && fixedUpdate)
        {
            _skipUpdate = false;
            return true;
        }

        return _skipUpdate;
    }

    bool IsOnGround()
    {
        if (_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
            _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")) ||
            _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Switch")) ||
            _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return true;
        }

        foreach (var gate in FindObjectsOfType<GravityChange>())
        {
            if (playerType == PlayerType.Player1 && gate.gateMode != GravityChange.GateMode.SecondPlayer ||
                playerType == PlayerType.Player2 && gate.gateMode != GravityChange.GateMode.FirstPlayer)
            {
                continue;
            }

            foreach (var gateCollider in gate.gameObject.GetComponents<BoxCollider2D>())
            {
                if (gateCollider.isTrigger && _boxCollider2D.IsTouching(gateCollider) &&
                    gateCollider.transform.rotation != Quaternion.identity)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void UpdateRotationMatrix()
    {
        // Update Rotation Matrix
        if (Physics2D.gravity.y != 0)
        {
            var quaternion = Quaternion.Euler(0.0f, 0.0f, Physics2D.gravity.y < 0.0f ? 0.0f : 180.0f);
            _rotationMatrix = Matrix4x4.Rotate(quaternion);
        }
        else
        {
            var quaternion = Quaternion.Euler(0.0f, 0.0f, Physics2D.gravity.x < 0.0f ? -90.0f : 90.0f);
            _rotationMatrix = Matrix4x4.Rotate(quaternion);
        }
    }

    void UpdatePlayerState()
    {
        // Update Prev Movement State
        if ((_playerState & (PlayerState.LeftMoving | PlayerState.RightMoving)) != PlayerState.Idle &&
            _prevMoveState != (_playerState & (PlayerState.LeftMoving | PlayerState.RightMoving)))
        {
            _prevMoveState = _playerState & (PlayerState.LeftMoving | PlayerState.RightMoving);
        }

        // Update Movement State
        _playerState &= ~(PlayerState.LeftMoving | PlayerState.RightMoving);
        if (_moveInput > 0.0f) _playerState |= PlayerState.RightMoving;
        else if (_moveInput < 0.0f) _playerState |= PlayerState.LeftMoving;
        else _playerState &= ~(PlayerState.LeftMoving & PlayerState.RightMoving);

        // Update OnGround state
        bool onGround = IsOnGround();
        _playerState &= ~PlayerState.OnGround;
        _playerState = onGround ? _playerState | PlayerState.OnGround : _playerState;

        // Update Jumping State
        if (onGround || (_playerState & PlayerState.Jumping) != 0)
        {
            _playerState &= ~PlayerState.Jumping;
            if ((_playerState & PlayerState.Reversed) == 0 && _jumpInput > 0.0f ||
                (_playerState & PlayerState.Reversed) != 0 && _jumpInput != 0.0f)
            {
                _playerState |= PlayerState.Jumping;
            }
        }
    }

    void UpdateMovement()
    {
        bool onGround = (_playerState & PlayerState.OnGround) != 0;
        var velocity = new Vector4(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y, 0.0f, 1.0f);

        // Add force in horizontal direction based on the speed.
        var inversedVelocity = _rotationMatrix.inverse * velocity;
        var horizontalForce = new Vector4((_moveInput * moveSpeed - inversedVelocity.x) * moveDrag, 0.0f, 0.0f, 1.0f);
        _rigidbody2D.AddForce(_rotationMatrix * horizontalForce);

        var jumpVelocity = inversedVelocity;
        // Changes the vertical speed of the player.
        if (onGround)
        {
            // Can use up and down key to jump if the player is reversed
            if ((_playerState & PlayerState.Reversed) == 0 && _jumpInput > 0.0f)
            {
                jumpVelocity.y = jumpSpeed;
            }
            else if ((_playerState & PlayerState.Reversed) != 0 && _jumpInput != 0.0f)
            {
                jumpVelocity.y = -jumpSpeed;
            }
        }

        jumpVelocity = _rotationMatrix * new Vector4(jumpVelocity.x, jumpVelocity.y, 0.0f, 1.0f);

        _rigidbody2D.velocity = jumpVelocity;
    }

    void UpdateRotation()
    {
        bool reversed = (_playerState & PlayerState.Reversed) != 0;

        var reverseQuaternion = new Quaternion();
        if ((_playerState & PlayerState.LeftMoving) != 0 || (_playerState & PlayerState.RightMoving) != 0)
        {
            bool leftMoving = (_playerState & PlayerState.LeftMoving) != 0;
            reverseQuaternion = Quaternion.Euler(reversed ? 180.0f : 0.0f, leftMoving ? 180.0f : 0.0f, 0.0f);
        }
        else
        {
            bool leftMoving = (_prevMoveState & PlayerState.LeftMoving) != 0;
            reverseQuaternion = Quaternion.Euler(reversed ? 180.0f : 0.0f, leftMoving ? 180.0f : 0.0f, 0.0f);
        }

        transform.localRotation = _rotationMatrix.rotation * reverseQuaternion;
    }

    void UpdateAnimation()
    {
        var runId = Animator.StringToHash("run");
        bool isMoving = (_playerState & (PlayerState.LeftMoving | PlayerState.RightMoving)) != 0;
        _playerAnimator.SetBool(runId, isMoving);

        var jumpId = Animator.StringToHash("jump");
        bool isJumping = (_playerState & PlayerState.Jumping) != 0;
        _playerAnimator.SetBool(jumpId, isJumping);
    }

    void AudioControl()
    {
        if ((_playerState & PlayerState.Jumping) != 0)
        {
            AudioController.Instance.PlayOneShot("Jump");
        }
    }
}
