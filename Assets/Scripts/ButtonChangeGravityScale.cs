using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeGravityScale : MonoBehaviour
{
    public enum ButtonType { Minus = 1, Plus = 2 };

    public ButtonType buttonType = ButtonType.Minus;
    public float minusDelta;
    public float plusDelta;
    public float lowerBound;
    public float upperBound;

    private void Start()
    {
        if (buttonType == ButtonType.Plus)
        {
            GetComponent<SpriteRenderer>().color = new Color(230, 90, 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var thisPlayer = other;
            var otherPlayer = player;

            if (thisPlayer.GetComponent<PlayerController>().playerType != otherPlayer.GetComponent<PlayerController>().playerType)
            {
                var currGravityScale = otherPlayer.GetComponent<Rigidbody2D>().gravityScale;

                if (buttonType == ButtonType.Minus)
                {
                    currGravityScale = (currGravityScale > 0 ?
                        Math.Max(lowerBound, currGravityScale - minusDelta) : Math.Min(-lowerBound, currGravityScale + minusDelta));
                }

                else if (buttonType == ButtonType.Plus)
                {
                    currGravityScale = (currGravityScale > 0 ?
                        Math.Min(upperBound, currGravityScale + plusDelta) : Math.Max(-upperBound, currGravityScale - plusDelta));
                }

                otherPlayer.GetComponent<Rigidbody2D>().gravityScale = currGravityScale;
                //Debug.Log(otherPlayer.GetComponent<PlayerController>().playerType + ", gravity scale " + currGravityScale);  ///////////////
            }

        }
    }
}
