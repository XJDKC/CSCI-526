using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IReversible
{
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Collecting stars and updating points
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            StarUI.CurrentStarQuantity += 1;
            Debug.Log(StarUI.CurrentStarQuantity);
            Destroy(gameObject);
        }
    }

    public void Reverse()
    {
        if (!_rigidbody2D) return;
        _rigidbody2D.gravityScale *= -1.0f;
    }
}
