using System;
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
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 10.0f;
    public RuntimeAnimatorController animatorController1;
    public RuntimeAnimatorController animatorController2;

    [Flags]
    private enum PlayerState { Idle = 0, LeftMoving = 1, RightMoving = 2, Jumping = 4, Reversed = 8 };

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

    private Transform _parentTransform = null;

    private void Awake()
    {
        // Assign control scheme for players
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _parentTransform = transform.parent;
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

    void Update()
    {
        UpdateState();
        UpdateVelocity();
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

    bool IsOnGround()
    {
        return _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
               _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")) ||
               _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platform")) ||
               _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Gate"));
    }

    void UpdateVelocity()
    {
        bool onGround = IsOnGround();
        Vector2 velocity = new Vector2(_moveInput * moveSpeed, _rigidbody2D.velocity.y);
        // Changes the vertical speed of the player.
        if (onGround && ((_playerState & PlayerState.Reversed) == 0 && _jumpInput > 0.0f ||
                         (_playerState & PlayerState.Reversed) != 0 && _jumpInput < 0.0f))
        {
            velocity.y = jumpSpeed * _jumpInput;
        }

        _rigidbody2D.velocity = velocity;
    }

    void UpdateState()
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

        // Update Jumping State
        _playerState &= ~PlayerState.Jumping;
        if ((_playerState & PlayerState.Reversed) == 0 && _jumpInput > 0.0f ||
            (_playerState & PlayerState.Reversed) != 0 && _jumpInput < 0.0f)
        {
            _playerState |= PlayerState.Jumping;
        }
    }

    void UpdateRotation()
    {
        bool reversed = (_playerState & PlayerState.Reversed) != 0;
        if ((_playerState & PlayerState.LeftMoving) != 0 || (_playerState & PlayerState.RightMoving) != 0)
        {
            bool leftMoving = (_playerState & PlayerState.LeftMoving) != 0;
            transform.localRotation = Quaternion.Euler(reversed ? 180.0f : 0.0f, leftMoving ? 180.0f : 0.0f, 0.0f);
        }
        else
        {
            bool leftMoving = (_prevMoveState & PlayerState.LeftMoving) != 0;
            transform.localRotation = Quaternion.Euler(reversed ? 180.0f : 0.0f, leftMoving ? 180.0f : 0.0f, 0.0f);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var bodyCollider = collision.collider;
        var feetCollider = collision.otherCollider;
        if (collision.gameObject.CompareTag("Player") && feetCollider is BoxCollider2D &&
            bodyCollider is CapsuleCollider2D)
        {
            transform.parent = bodyCollider.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var bodyCollider = collision.collider;
        var feetCollider = collision.otherCollider;
        if (collision.gameObject.CompareTag("Player") && feetCollider is BoxCollider2D &&
            bodyCollider is CapsuleCollider2D)
        {
            transform.parent = _parentTransform;
        }
    }
}
