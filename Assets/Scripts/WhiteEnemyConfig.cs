using System.Collections;
using System.Collections.Generic;
using System.IO;
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

            if (startPrefab)
            {
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0, 180.0f));
                Instantiate(startPrefab, blackEnemy.transform.position, rotation);
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
