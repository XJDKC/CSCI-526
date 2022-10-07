using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class GravityChange : MonoBehaviour
{
    private Vector3 _upVector = Vector3.up;
    private Dictionary<Collider2D, float> _colliderPrevSide = new Dictionary<Collider2D, float>();

    private void Start()
    {
        float anglesZ = transform.rotation.eulerAngles.z;
        _upVector = Quaternion.AngleAxis(anglesZ, Vector3.forward) * Vector3.up;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var reversibleObject = collider.gameObject.GetComponent<IReversible>();
        if (reversibleObject == null) return;

        // ignore player with box collider
        if (collider.gameObject.CompareTag("Player") && collider is BoxCollider2D) return;

        // update collider prev side
        _colliderPrevSide[collider] = GetSide(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
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
            StarUI.numsThroughGate += 1;
            Debug.Log(StarUI.numsThroughGate);
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
}
