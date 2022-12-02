using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class WhiteEnemy : MonoBehaviour
{
    private Animator _whiteEnemy;
    private bool _isCollided;

    void Start()
    {
        _whiteEnemy = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && _isCollided == false)
        {
            _isCollided = true;
            GameObject parent = transform.parent.gameObject;
            GameObject blackEnemy = parent.transform.GetChild(1).gameObject;
            bool isReversed = false;
            isReversed = parent.GetComponent<EnemyController>().enemyReverse;

            if (StarManager.Instance)
            {
                StarManager.Instance.InstanceStars(isReversed, blackEnemy);
            }

            Destroy(transform.parent.gameObject);
            AudioController.Instance.Play("EnemyCollide");
        }
        else if (_whiteEnemy != null)
        {
            var isCollidedId = Animator.StringToHash("isCollided");
            _whiteEnemy.SetBool(isCollidedId, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (_whiteEnemy != null)
        {
            var isCollidedId = Animator.StringToHash("isCollided");
            _whiteEnemy.SetBool(isCollidedId, false);
        }
    }
}
