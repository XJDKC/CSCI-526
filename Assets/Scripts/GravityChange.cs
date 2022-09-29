using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class GravityChange : MonoBehaviour
{
    private Vector3 _upVector = Vector3.up;
    private Dictionary<Collider2D, float> _colliderEnterSide = new Dictionary<Collider2D, float>();
    private Dictionary<Collider2D, float> _colliderEnterMid = new Dictionary<Collider2D, float>();

    private void Start()
    {
        float anglesZ = transform.rotation.eulerAngles.z;
        _upVector = Quaternion.AngleAxis(anglesZ, Vector3.forward) * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // ignore player with box collider
        if (collider.gameObject.tag == "Player" && collider.GetType() == typeof(BoxCollider2D)) return;

        // if player do not have much velocity when cross the horizontal door, add a force
        Rigidbody2D player = collider.GetComponent<Rigidbody2D>();
        if (Math.Abs(player.velocity.y) < 2)
        {
            if (player.velocity.y > 0)  player.AddForce(new Vector2(0, 300));
            if (player.velocity.y < 0)  player.AddForce(new Vector2(0, -300));
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        // ignore player with box collider
        if (collider.gameObject.tag == "Player" && collider.GetType() == typeof(BoxCollider2D)) return;

        // find door's center and player's center
        Vector3 center = transform.position;
        Vector3 colliderCenter = collider.bounds.center;

        // vector from door to character: character's center - door's center position
        Vector3 doorToObj = (center - colliderCenter).normalized;
        float side = Vector3.Cross(_upVector, doorToObj).z;

        // compare the previous side with current side vector cross product
        if (_colliderEnterMid.ContainsKey(collider)){
            float prevSide = _colliderEnterMid[collider];

            if (side * prevSide < 0.0)
            {
                collider.gameObject.GetComponent<Rigidbody2D>().gravityScale *= -1;
                collider.gameObject.GetComponent<PlayerController>().Reverse();
            }
        }

        _colliderEnterMid[collider] = side;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // ignore player with box collider
        if (collider.gameObject.tag == "Player" && collider.GetType() == typeof(BoxCollider2D)) return;
    }
}
