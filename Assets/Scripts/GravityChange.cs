using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class GravityChange : MonoBehaviour
{
    public enum GateMode { BothPlayers, FirstPlayer, SecondPlayer }

    public GateMode gateMode = GateMode.BothPlayers;
    public Color normalColor = Color.white;
    public Color firstPlayerColor = new Color(92, 200, 231);
    public Color secondPlayerColor = new Color(234, 54, 127);
    public float motivateThreshold = 1.0f;
    public float motivateMagnitude = 3.0f;

    private BoxCollider2D _boxCollider2D;
    private Vector3 _upVector = Vector3.up;
    private Dictionary<Collider2D, float> _colliderPrevSide = new Dictionary<Collider2D, float>();

    private void Awake()
    {
        // find box collider
        foreach (var collider2D in GetComponents<BoxCollider2D>())
        {
            if (!collider2D.isTrigger)
            {
                _boxCollider2D = collider2D;
            }
        }

        // calculate up vector
        float anglesZ = transform.rotation.eulerAngles.z;
        _upVector = Quaternion.AngleAxis(anglesZ, Vector3.forward) * Vector3.up;
    }

    private void Start()
    {
        switch (gateMode)
        {
            case GateMode.BothPlayers:
                GetComponent<SpriteRenderer>().color = normalColor;
                break;
            case GateMode.FirstPlayer:
                GetComponent<SpriteRenderer>().color = firstPlayerColor;
                break;
            case GateMode.SecondPlayer:
                GetComponent<SpriteRenderer>().color = secondPlayerColor;
                break;
        }

        IgnoreCollisions();
    }

    private void IgnoreCollisions()
    {
        if (!_boxCollider2D) return;

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var playerType = player.GetComponent<PlayerController>().playerType;
            if (playerType == PlayerController.PlayerType.Player1 && gateMode != GateMode.SecondPlayer ||
                playerType == PlayerController.PlayerType.Player2 && gateMode != GateMode.FirstPlayer)
            {
                foreach (var playerCollider in player.GetComponents<Collider2D>())
                {
                    Physics2D.IgnoreCollision(playerCollider, _boxCollider2D);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        MotivatePlayer(collider);

        // update collider prev side
        _colliderPrevSide[collider] = GetSide(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        MotivatePlayer(collider);

        float currSide = GetSide(collider);

        // compare the previous side with current side vector cross product
        if (_colliderPrevSide.ContainsKey(collider))
        {
            float prevSide = _colliderPrevSide[collider];

            if (currSide * prevSide < 0.0)
            {
                reversibleObject.Reverse();
            }
        }

        // update collider prev side
        _colliderPrevSide[collider] = currSide;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        float currSide = GetSide(collider);

        // compare the previous side with current side vector cross product
        if (_colliderPrevSide.ContainsKey(collider))
        {
            float prevSide = _colliderPrevSide[collider];

            if (currSide * prevSide < 0.0)
            {
                reversibleObject.Reverse();
            }

            //Data collection
            //StarUI.numsThroughGate += 1;
            //Debug.Log(StarUI.numsThroughGate);
        }

        // update collider prev side
        _colliderPrevSide[collider] = currSide;
    }

    private float GetSide(Collider2D collider)
    {
        // find door's center and player's center
        Vector3 center = transform.position;
        Vector3 colliderCenter = collider.bounds.center;

        // vector from door to character: character's center - door's center position
        Vector3 doorToObj = (colliderCenter - center).normalized;
        float side = Vector3.Cross(_upVector, doorToObj).z;

        return side;
    }

    private void MotivatePlayer(Collider2D collider)
    {
        var rigidbody2D = collider.gameObject.GetComponent<Rigidbody2D>();
        if (Mathf.Abs(Vector3.Dot(_upVector, Vector3.up)) < 0.1f)
        {
            if (Mathf.Abs(Vector2.Dot(rigidbody2D.velocity, Vector2.up)) < motivateThreshold)
            {
                bool leftSide = GetSide(collider) > 0.0f;
                var force = (leftSide ? Vector2.down : Vector2.up) * rigidbody2D.mass * motivateMagnitude;
                rigidbody2D.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
