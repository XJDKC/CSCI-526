using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerType { Player1 = 1, Player2 = 2 };

    [Flags]
    public enum PlayerState { Idle = 0, LeftMoving = 1, RightMoving = 2, Jumping = 4, Reversed = 8 };

    public PlayerType playerType = PlayerType.Player1;
    public float playerSpeed = 3.0f;
    public float jumpSpeed = 5.0f;

    private float _movementInput = 0.0f;
    private float _jumpInput = 0.0f;
    private Animator _playerAnimator;
    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _capsuleCollider2D;
    private PlayerState _playerState = PlayerState.Idle;

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<float>();
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
        return _capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    void UpdateVelocity()
    {
        bool onGround = IsOnGround();
        Vector2 velocity = new Vector2(_movementInput * playerSpeed, _rigidbody2D.velocity.y);
        // Changes the vertical speed of the player.
        if (_jumpInput != 0.0f && onGround)
        {
            velocity.y = jumpSpeed * _jumpInput;
        }

        _rigidbody2D.velocity = velocity;
    }

    void UpdateState()
    {
        _playerState &= ~(PlayerState.LeftMoving | PlayerState.RightMoving);
        if (_movementInput > 0.0f) _playerState |= PlayerState.RightMoving;
        else if (_movementInput < 0.0f) _playerState |= PlayerState.LeftMoving;
        else _playerState &= ~(PlayerState.LeftMoving & PlayerState.RightMoving);
        _playerState &= ~PlayerState.Jumping;
        if ((_playerState & PlayerState.Reversed) == 0 && _jumpInput > 0.0f ||
            (_playerState & PlayerState.Reversed) != 0 && _jumpInput < 0.0f)
        {
            _playerState |= PlayerState.Jumping;
        }
        else
        {
            _playerState &= ~PlayerState.Jumping;
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
            transform.localRotation = Quaternion.Euler(reversed ? 180.0f : 0.0f, transform.localPosition.y, 0.0f);
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
