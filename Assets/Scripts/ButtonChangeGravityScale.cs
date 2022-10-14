using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeGravityScale : MonoBehaviour
{
    public enum ButtonType { Minus = 1, Plus = 2 };

    public ButtonType buttonType = ButtonType.Minus;
    public float delta;
    public float lowerBound;
    public float upperBound;
    public GameObject player1;
    public GameObject player2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerController>();

            if (player.playerType == PlayerController.PlayerType.Player1)
            {
                var curr = player2.GetComponent<Rigidbody2D>().gravityScale;
                if (buttonType == ButtonType.Minus)
                {
                    curr = (curr > 0 ? Math.Max(lowerBound, curr - delta) : Math.Min(-lowerBound, curr + delta));
                }
                player2.GetComponent<Rigidbody2D>().gravityScale = curr;
                Debug.Log(player.playerType + ", gravity scale " + curr);  ////////////////
            }
            else if (player.playerType == PlayerController.PlayerType.Player2)
            {
                var curr = player1.GetComponent<Rigidbody2D>().gravityScale;
                if (buttonType == ButtonType.Minus)
                {
                    curr = (curr > 0 ? Math.Max(lowerBound, curr - delta) : Math.Min(-lowerBound, curr + delta));
                }
                player1.GetComponent<Rigidbody2D>().gravityScale = curr;
                Debug.Log(player.playerType + ", gravity scale " + curr);  ////////////////
            }
        }
    }
}
