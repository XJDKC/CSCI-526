using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonChangeGravityScale : MonoBehaviour
{
    public enum ButtonType { Minus = 1, Plus = 2 };

    public ButtonType buttonType = ButtonType.Minus;
    public float minusDelta;
    public float plusDelta;
    public float lowerBound;
    public float upperBound;
    public GameObject plus;
    public GameObject minus;

    private Vector3 position;
    private void Start()
    {
        position = transform.position;
        if (buttonType == ButtonType.Plus)
        {
            GetComponent<SpriteRenderer>().color = new Color(240, 90, 90);
            plus.SetActive(true);
            plus.transform.position = new Vector3(position.x - 0.6f, position.y, position.z);
        }
        else
        {
            minus.SetActive(true);
            minus.transform.position = new Vector3(position.x -0.6f, position.y, position.z);
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

                if(otherPlayer.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
                {
                    Vector2 jumpVel = new Vector2(0.0f, otherPlayer.GetComponent<PlayerController>().jumpSpeed);
                    otherPlayer.GetComponent<Rigidbody2D>().velocity = currGravityScale > 0 ? Vector2.up * jumpVel: Vector2.down * jumpVel;
                }
                //Debug.Log(otherPlayer.GetComponent<PlayerController>().playerType + ", gravity scale " + currGravityScale);  ///////////////
            }

        }
    }

    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var thisPlayer = other;
            var otherPlayer = player;
            if (thisPlayer.GetComponent<PlayerController>().playerType != otherPlayer.GetComponent<PlayerController>().playerType)
            {
                var currGravityScale = otherPlayer.GetComponent<Rigidbody2D>().gravityScale;
                currGravityScale = currGravityScale > 0 ? 2 : -2;
                otherPlayer.GetComponent<Rigidbody2D>().gravityScale = currGravityScale;
            }
        }
    }
    */
}
