using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerType { Player1, Player2 };

    public PlayerType playerType = PlayerType.Player1;
    public float playerSpeed = 2.0f;
    public float jumpSpeed= 1.0f;

    private float _movementInput = 0.0f;
    private float _jumpInput = 0.0f;

    private Rigidbody2D _rigidbody2D;

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
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValue<float>();
    }

    void Update()
    {
        bool onGround = Math.Abs(_rigidbody2D.velocity.y) < 1e-6f;
        Vector2 velocity = new Vector2(_movementInput * playerSpeed, _rigidbody2D.velocity.y);
        // Changes the vertical speed of the player.
        if (_jumpInput > 0.0f && onGround)
        {
            velocity.y = jumpSpeed;
        }
        _rigidbody2D.velocity = velocity;
    }
}
