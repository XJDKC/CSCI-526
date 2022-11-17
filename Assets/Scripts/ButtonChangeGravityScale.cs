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

    private Vector3 _position;
    private Animator _myAnimator;
    private void Start()
    {
        _position = transform.position;
        _myAnimator = GetComponent<Animator>();
        if (buttonType == ButtonType.Plus)
        {
            // GetComponent<SpriteRenderer>().color = new Color(240, 90, 90);
            plus.SetActive(true);
            if(transform.rotation.z.Equals(1))
                plus.transform.position = new Vector3(_position.x, _position.y - 0.3f, _position.z);
            else
                plus.transform.position = new Vector3(_position.x, _position.y + 0.3f, _position.z);
        }
        else
        {
            minus.SetActive(true);
            if (transform.rotation.z.Equals(1))
                minus.transform.position = new Vector3(_position.x, _position.y - 0.3f, _position.z);
            else
                minus.transform.position = new Vector3(_position.x, _position.y + 0.3f, _position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _myAnimator.SetBool("down", true);

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var thisPlayer = other;
            var otherPlayer = player;
            bool touchongGround;

            if (thisPlayer.GetComponent<PlayerController>().playerType != otherPlayer.GetComponent<PlayerController>().playerType)
            {
                var currGravityScale = otherPlayer.GetComponent<Rigidbody2D>().gravityScale;
                touchongGround = otherPlayer.GetComponent<BoxCollider2D>()
                    .IsTouchingLayers(LayerMask.GetMask("Ground"));

                if (buttonType == ButtonType.Minus && touchongGround)
                {
                    currGravityScale = (currGravityScale > 0 ?
                        Math.Max(lowerBound, currGravityScale - minusDelta) : Math.Min(-lowerBound, currGravityScale + minusDelta));
                }

                else if (buttonType == ButtonType.Plus && touchongGround)
                {
                    currGravityScale = (currGravityScale > 0 ?
                        Math.Min(upperBound, currGravityScale + plusDelta) : Math.Max(-upperBound, currGravityScale - plusDelta));
                }

                otherPlayer.GetComponent<Rigidbody2D>().gravityScale = currGravityScale;

                if(touchongGround)
                {
                    Vector2 jumpVel = new Vector2(0.0f, otherPlayer.GetComponent<PlayerController>().jumpSpeed);
                    otherPlayer.GetComponent<Rigidbody2D>().velocity = currGravityScale > 0 ? Vector2.up * jumpVel: Vector2.down * jumpVel;
                }
                //Debug.Log(otherPlayer.GetComponent<PlayerController>().playerType + ", gravity scale " + currGravityScale);  ///////////////
            }

        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        _myAnimator.SetBool("down", false);
    }

}
