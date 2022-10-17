using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySlide : MonoBehaviour
{
    public enum SlideMode {Top, Down};
    public SlideMode slideMode = SlideMode.Top;

    private void OnTriggerEnter2D(Collider2D collider) {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;
        
        var gravityScale = collider.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        if (gravityScale > 0 && slideMode == SlideMode.Top ||
            gravityScale < 0 && slideMode == SlideMode.Down ) {
            
            reversibleObject.Reverse();
        }

        Debug.LogFormat("Player gravity side is: {0}", collider.gameObject.GetComponent<Rigidbody2D>().gravityScale);
    }
}
