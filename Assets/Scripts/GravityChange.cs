using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class GravityChange : MonoBehaviour
{
    private Vector3 _upVector = Vector3.up;
    private Dictionary<Collider2D, float> _colliderEnterSide = new Dictionary<Collider2D, float>();

    private void Start()
    {
        float anglesZ = transform.rotation.eulerAngles.z;
        _upVector = Quaternion.AngleAxis(anglesZ, Vector3.forward) * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Vector3 center = transform.position;
            Vector3 colliderCenter = collider.bounds.center;

            // Vector from door to character: character's center - door's center position
            Vector3 doorToObj = (center - colliderCenter).normalized;
            float enterSide = Vector3.Cross(_upVector, doorToObj).z;
            _colliderEnterSide[collider] = enterSide;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Vector3 center = transform.position;
            Vector3 colliderCenter = collider.bounds.center;

            // Vector from door to character: character's center - door's center position
            Vector3 doorToObj = (center - colliderCenter).normalized;
            float leaveSide = Vector3.Cross(_upVector, doorToObj).z;

            if (_colliderEnterSide.ContainsKey(collider))
            {
                float enterSide = _colliderEnterSide[collider];
                if (enterSide * leaveSide < 0.0)
                {
                    collider.gameObject.GetComponent<Rigidbody2D>().gravityScale *= -1;
                    collider.gameObject.GetComponent<PlayerController>().Reverse();
                }
            }
        }
    }
}
