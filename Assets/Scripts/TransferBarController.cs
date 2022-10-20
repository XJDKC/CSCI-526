using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransferBarController : MonoBehaviour
{
    public enum TransferMode { SameSide, OppositeSide }

    public Color barColor = Color.yellow * 0.95f;
    public Color activeBarColor = new Color(255, 255, 0);
    public TransferMode transferMode = TransferMode.OppositeSide;

    private Dictionary<GameObject, HashSet<GameObject>> _bar2TouchingObjects = new();

    private void Awake()
    {
        var childTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (var barObject in childTransforms.Skip(1))
        {
            _bar2TouchingObjects.Add(barObject.gameObject, new HashSet<GameObject>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var (barObject, touchingObjects) in _bar2TouchingObjects)
        {
            barObject.GetComponent<SpriteRenderer>().color = barColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var barCollider = collision.otherCollider;
        var playerCollider = collision.collider;
        if (!_bar2TouchingObjects.ContainsKey(barCollider.gameObject)) return;
        if (!playerCollider.gameObject.CompareTag("Player")) return;
        if (playerCollider is not BoxCollider2D) return;

        var player = playerCollider.gameObject;
        var playerType = player.GetComponent<PlayerController>().playerType;
        float relativeVelocityY = collision.relativeVelocity.y;
        float velocityY = transferMode == TransferMode.OppositeSide ? relativeVelocityY : -relativeVelocityY;

        foreach (var (barObject, touchingObjects) in _bar2TouchingObjects)
        {
            if (barObject == barCollider.gameObject)
            {
                touchingObjects.Add(player);
            }
            else
            {
                foreach (var otherPlayer in touchingObjects)
                {
                    var rigidbody2D = otherPlayer.GetComponent<Rigidbody2D>();
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, velocityY);
                }
            }
        }

        SetRendererColor();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var barCollider = collision.otherCollider;
        var playerCollider = collision.collider;

        if (!_bar2TouchingObjects.ContainsKey(barCollider.gameObject)) return;
        if (!playerCollider.gameObject.CompareTag("Player")) return;
        if (playerCollider is not BoxCollider2D) return;

        var player = playerCollider.gameObject;
        var playerType = player.GetComponent<PlayerController>().playerType;
        _bar2TouchingObjects[barCollider.gameObject].Remove(player);

        SetRendererColor();
    }

    void SetRendererColor()
    {
        bool actived = false;
        foreach (var (barObject, touchingObjects) in _bar2TouchingObjects)
        {
            if (touchingObjects.Count != 0)
            {
                actived = true;
                break;
            }
        }

        foreach (var (barObject, touchingObjects) in _bar2TouchingObjects)
        {
            if (actived)
            {
                barObject.GetComponent<SpriteRenderer>().color = activeBarColor;
            }
            else
            {
                barObject.GetComponent<SpriteRenderer>().color = barColor;
            }
        }
    }
}
