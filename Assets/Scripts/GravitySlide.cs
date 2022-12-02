using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySlide : MonoBehaviour
{
    public enum SlideMode { Up, Down };

    public float scrollSpeed = 0.2f;
    public SlideMode slideMode = SlideMode.Up;

    private SpriteRenderer _spriteRenderer;
    private Dictionary<Collider2D, bool> _colliderChangeGravity = new Dictionary<Collider2D, bool>();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        float rotationZ = slideMode == SlideMode.Up ? 0.0f : 180.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    private void Update()
    {
        var offsetY = scrollSpeed * Time.time;
        _spriteRenderer.material.mainTextureOffset = new Vector2(0.0f, -offsetY);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        AudioController.Instance.PlayOneShot("Slides");
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        bool playerInBound = false;

        // check if the box contains player's center
        var bound = GetComponent<BoxCollider2D>().bounds;
        if (bound.Contains(collider.bounds.center))
        {
            playerInBound = true;
        }

        // if player's center in the box bounds
        if (playerInBound == true)
        {
            // already change player's gravity
            if (_colliderChangeGravity.ContainsKey(collider)) return;

            var gravityScale = collider.gameObject.GetComponent<Rigidbody2D>().gravityScale;
            if (gravityScale > 0 && slideMode == SlideMode.Up ||
                gravityScale < 0 && slideMode == SlideMode.Down)
            {
                reversibleObject.Reverse();

                _colliderChangeGravity[collider] = true;
            }
            else
            {
                _colliderChangeGravity[collider] = false;
            }
        }
        else
        {
            // already change player's gravity, and player is leaving the box
            if (_colliderChangeGravity.ContainsKey(collider))
            {
                // if player has changed gravity
                if (_colliderChangeGravity[collider]) reversibleObject.Reverse();

                // clean the dictionary
                _colliderChangeGravity.Remove(collider);
            }
        }
    }
}
