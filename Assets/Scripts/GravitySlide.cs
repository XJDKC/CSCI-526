using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySlide : MonoBehaviour
{
    public enum SlideMode {Top, Down};
    public SlideMode slideMode = SlideMode.Top;
    private Dictionary<Collider2D, bool> _colliderChangeGravity = new Dictionary<Collider2D, bool>();
    
    private void OnTriggerStay2D(Collider2D collider) {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;
        
        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        bool playerInBound = false;

        // check if the box contains player's center
        var bound = GetComponent<BoxCollider2D>().bounds;
        if (bound.Contains(collider.bounds.center)){
            playerInBound = true;
        }

        // if player's center in the box bounds
        if (playerInBound == true) {
            // already change player's gravity
            if (_colliderChangeGravity.ContainsKey(collider)) return;

            var gravityScale = collider.gameObject.GetComponent<Rigidbody2D>().gravityScale;
            if (gravityScale > 0 && slideMode == SlideMode.Top ||
                gravityScale < 0 && slideMode == SlideMode.Down) {
                
                reversibleObject.Reverse();

                _colliderChangeGravity[collider] = true;
                
            } else {
                _colliderChangeGravity[collider] = false;
            }
        } else {
            // already change player's gravity, and player is leaving the box
            if (_colliderChangeGravity.ContainsKey(collider))
            {
                // if player has changed gravity
                if (_colliderChangeGravity[collider])   reversibleObject.Reverse();

                // clean the dictionary
                _colliderChangeGravity.Remove(collider);
            }
        }
    }


}
