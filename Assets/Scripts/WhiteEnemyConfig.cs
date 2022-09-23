using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class WhiteEnemyConfig : MonoBehaviour
{
    public Object startPrefab = null;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject parent = transform.parent.gameObject;
            GameObject blackEnemy = parent.transform.GetChild(1).gameObject;
            bool isReversed = false;
            if (parent.GetComponent<VerticalCoupleConfig>())
            {
                isReversed = parent.GetComponent<VerticalCoupleConfig>().enemyReverse;
            }

            if (parent.GetComponent<HorizontalCoupleConfig>())
            {
                isReversed = parent.GetComponent<HorizontalCoupleConfig>().enemyReverse;
            }

            if (startPrefab)
            {
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0, 180.0f));
                var star = Instantiate(startPrefab, blackEnemy.transform.position, rotation);
                star.GetComponent<Rigidbody2D>().gravityScale = isReversed ? -1.0f : 1.0f;
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
