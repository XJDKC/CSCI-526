using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    public float force = 1000f;
    public bool isReverse = false;
    private Animator _springAnimator;

    private void Start()
    {
        _springAnimator = GetComponent<Animator>();
        if (isReverse)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        }
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        var col = obj.collider;
        if (col is BoxCollider2D && col.GameObject().CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            var playerBottom = col.gameObject.GetComponent<Renderer>().bounds.min.y;
            var playerTop = col.gameObject.GetComponent<Renderer>().bounds.max.y;
            var springBottom = gameObject.GetComponent<Renderer>().bounds.min.y;
            var springTop = gameObject.GetComponent<Renderer>().bounds.max.y;


            if (playerBottom + 0.05 >= springTop && isReverse == false)
            {
                var isCollidedId = Animator.StringToHash("isCollided");
                _springAnimator.SetBool(isCollidedId, true);
                col.GameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, isReverse ? -1 : 1) * force);
                AudioController.instance.Play("Spring");
            }

            if (playerTop - 0.05 <= springBottom && isReverse == true)
            {
                var isCollidedId = Animator.StringToHash("isCollided");
                _springAnimator.SetBool(isCollidedId, true);
                col.GameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, isReverse ? -1 : 1) * force);
                AudioController.instance.Play("Spring");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        var col = other.collider;
        if (col is BoxCollider2D && col.GameObject().CompareTag("Player"))
        {
            var isCollidedId = Animator.StringToHash("isCollided");
            _springAnimator.SetBool(isCollidedId, false);
        }
    }
}
