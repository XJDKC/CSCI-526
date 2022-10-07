using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class WhiteEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject parent = transform.parent.gameObject;
            GameObject blackEnemy = parent.transform.GetChild(1).gameObject;
            bool isReversed = false;
            isReversed = parent.GetComponent<EnemyController>().enemyReverse;

            if (StarManager.Instance)
            {
                StarManager.Instance.InstanceStars(isReversed, blackEnemy);
            }

            Destroy(transform.parent.gameObject);
        }
    }
}