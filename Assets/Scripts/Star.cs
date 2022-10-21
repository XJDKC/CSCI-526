using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IReversible
{
    public float delaySecond = 5F;
    private float fadeSpeed = 0;

    private int _isReverse;
    private SpriteRenderer spriteRenderer = null;

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            this.spriteRenderer = spriteRenderer;
        fadeSpeed = this.spriteRenderer.color.a * Time.fixedDeltaTime / delaySecond;
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
        float alpha = spriteRenderer.color.a - fadeSpeed;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.r, spriteRenderer.color.r, alpha);
        if (alpha <= 0)
            Destroy(gameObject);
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var player = col.gameObject;
        if (player.CompareTag("Player") && col.collider is CapsuleCollider2D)
        {
            //data collect
            DataManager.AddStarPoints();
            Destroy(gameObject);
        }
    }

    public void Reverse()
    {
        if (!_rigidbody2D) return;
        _rigidbody2D.gravityScale *= -1.0f;
    }
}
