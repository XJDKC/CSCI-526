using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IReversible
{
    private Rigidbody2D _rigidbody2D;
    public float delaySecond = 5F;
    private SpriteRenderer spriteRenderer = null;
    private float fadeSpeed = 0;

    private int _isReverse;

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
