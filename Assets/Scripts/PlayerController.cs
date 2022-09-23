using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerType { Player1 = 1, Player2 = 2 };

    public PlayerType playerType = PlayerType.Player1;
    public float playerSpeed = 3.0f;
    public float jumpSpeed = 5.0f;

    [Flags]
    private enum PlayerState { Idle = 0, LeftMoving = 1, RightMoving = 2, Jumping = 4, Reversed = 8 };

    private float _moveInput = 0.0f;
    private float _jumpInput = 0.0f;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private PlayerState _playerState = PlayerState.Idle;
    private PlayerState _prevMoveState = PlayerState.Idle;

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValue<float>();
    }

    public void Reverse()
    {
        _playerState ^= PlayerState.Reversed;
    }

    private void Awake()
    {
        // Assign control scheme for players
        PlayerInput input = GetComponent<PlayerInput>();
        if (playerType == PlayerType.Player1)
        {
            input.SwitchCurrentControlScheme("Keyboard1", Keyboard.current);
        }
        else
        {
            input.SwitchCurrentControlScheme("Keyboard2", Keyboard.current);
        }
    }

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        UpdateState();
        UpdateVelocity();
        UpdateRotation();
        UpdateAnimation();
    }

    bool IsOnGround()
    {
        return _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
               _boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player"));
    }

    void UpdateVelocity()
    {
        bool onGround = IsOnGround();
        Vector2 velocity = new Vector2(_moveInput * playerSpeed, _rigidbody2D.velocity.y);
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
        if ((_playerState & PlayerState.LeftMoving) != 0 || (_playerState & PlayerState.RightMoving) != 0)
        {
            _playerAnimator.SetBool("run", true);
        }
        else
        {
            _playerAnimator.SetBool("run", false);
        }

        if ((_playerState & PlayerState.Jumping) != 0)
        {
            _playerAnimator.SetBool("jump", true);
        }
        else
        {
            _playerAnimator.SetBool("jump", false);
        }
    }
}
