using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Star : MonoBehaviour, IReversible
{
    public float delaySecond = 6F;
    public float fadeSecond = 2F;

    private int _isReversed;
    private float _elapsedTime;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer = null;
    private bool _isCollided;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyNow());
    }

    //Destory after delaySecond time
    private IEnumerator DestroyNow()
    {
        yield return new WaitForSeconds(delaySecond);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
        if (_elapsedTime >= delaySecond - fadeSecond)
        {
            Color newColor = _spriteRenderer.color;
            newColor.a = Mathf.Max((delaySecond - _elapsedTime) / fadeSecond, 0.0f);
            _spriteRenderer.color = newColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject;
        if (player.CompareTag("Player") && _isCollided == false)
        {
            _isCollided = true;

            AudioController.instance.PlayOneShot("StarsCollide");
            // data collect
            DataManager.AddStarPoints();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        // Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        if (gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0.8f &&
            (col.collider.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
             col.collider.IsTouchingLayers(LayerMask.GetMask("Player")) ||
             col.collider.IsTouchingLayers(LayerMask.GetMask("Platform"))))
        {
            AudioController.instance.PlayOneShot("StarsCollide");
        }
    }

    public void Reverse()
    {
        if (!_rigidbody2D) return;
        _rigidbody2D.gravityScale *= -1.0f;
    }
}
