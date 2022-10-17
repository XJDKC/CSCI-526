using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    public float force = 1000f;
    public bool isReverse = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col is BoxCollider2D&&col.GameObject().CompareTag("Player"))
        {   col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            var playerBottom = col.gameObject.GetComponent<Renderer>().bounds.min.y;
            var playerTop = col.gameObject.GetComponent<Renderer>().bounds.max.y;
            var springBottom = gameObject.GetComponent<Renderer>().bounds.min.y;
            var springTop = gameObject.GetComponent<Renderer>().bounds.max.y;

            if ( playerBottom + 0.05 >= springTop && isReverse == false)
            {
                col.GameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, isReverse ? -1 : 1) * force);
            }

            if (playerTop - 0.05 <= springBottom && isReverse == true)
            {
                col.GameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, isReverse ? -1 : 1) * force);
            }
        }
    }
}
