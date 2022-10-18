using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferBarController : MonoBehaviour
{
    public enum TransferMode { SameSide, OppositeSide }

    public Color barColor = Color.yellow;
    public TransferMode transferMode = TransferMode.OppositeSide;

    private GameObject _firstBar = null;
    private GameObject _secondBar = null;
    private HashSet<GameObject> _touchingObjects1 = new HashSet<GameObject>();
    private HashSet<GameObject> _touchingObjects2 = new HashSet<GameObject>();

    private void Awake()
    {
        var childTransforms = transform.GetComponentsInChildren<Transform>();
        if (childTransforms.Length < 3) return;
        _firstBar = childTransforms[1].gameObject;
        _secondBar = childTransforms[2].gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_firstBar || !_secondBar) return;
        _firstBar.GetComponent<SpriteRenderer>().color = barColor;
        _secondBar.GetComponent<SpriteRenderer>().color = barColor;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var barCollider = collision.otherCollider;
        var playerCollider = collision.collider;
        if (!_firstBar || !_secondBar) return;
        if (!playerCollider.gameObject.CompareTag("Player")) return;
        if (playerCollider is not BoxCollider2D) return;

        var player = playerCollider.gameObject;
        var playerType = player.GetComponent<PlayerController>().playerType;
        float relativeVelocityY = collision.relativeVelocity.y;
        float velocityY = transferMode == TransferMode.OppositeSide ? relativeVelocityY : -relativeVelocityY;
        if (barCollider.gameObject == _firstBar)
        {
            _touchingObjects1.Add(player);
            foreach (var otherPlayer in _touchingObjects2)
            {
                var rigidbody2D = otherPlayer.GetComponent<Rigidbody2D>();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, velocityY);
            }
        }
        else
        {
            _touchingObjects2.Add(player);
            foreach (var otherPlayer in _touchingObjects1)
            {
                var rigidbody2D = otherPlayer.GetComponent<Rigidbody2D>();
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, velocityY);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var barCollider = collision.otherCollider;
        var playerCollider = collision.collider;
        if (!_firstBar || !_secondBar) return;
        if (!playerCollider.gameObject.CompareTag("Player")) return;
        if (playerCollider is not BoxCollider2D) return;

        var player = playerCollider.gameObject;
        var playerType = player.GetComponent<PlayerController>().playerType;
        if (barCollider.gameObject == _firstBar)
        {
            _touchingObjects1.Remove(player);
        }
        else
        {
            _touchingObjects2.Remove(player);
        }
    }
}
