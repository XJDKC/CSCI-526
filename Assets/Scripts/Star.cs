using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    [SerializeField]
    public float delaySecond = 5F;

    [SerializeField]
    private float fadeSpeed = 0;
    private SpriteRenderer spriteRenderer = null;


    //Collecting stars and updaing points
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            StarUI.CurrentStarQuantity += 1;
            //Debug.Log(StarUI.CurrentStarQuantity);
            Destroy(gameObject);
        }
    }

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
}
