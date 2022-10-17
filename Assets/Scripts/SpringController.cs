using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpringController : MonoBehaviour
{
    public float force = 1000f;
    public bool isReverse = false;
    private int _reverse = 1;

    private void Start()
    {
        _reverse = isReverse ? 1 : -1;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        if (col is BoxCollider2D)
        {
            var playerBottom = col.gameObject.GetComponent<Renderer>().bounds.min.y;
            var springTop = gameObject.GetComponent<Renderer>().bounds.max.y;
            if (col.GameObject().CompareTag("Player") && playerBottom + 0.05 >= springTop)
                col.GameObject().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1 * _reverse) * force);
        }
    }
}
