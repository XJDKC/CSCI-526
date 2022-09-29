using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Star Fadeout Component
public class FadeOut : MonoBehaviour
{
    public float delaySecond = 5F;
    public float initialSpeed=6;
    private SpriteRenderer spriteRenderer = null;
    private float fadeSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            this.spriteRenderer = spriteRenderer;
        fadeSpeed = this.spriteRenderer.color.a * Time.fixedDeltaTime / delaySecond;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,initialSpeed);
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

    // Update is called once per frame
    void Update()
    {

    }
}
