using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class WhiteEnemyConfig : MonoBehaviour
{
    public Object startPrefab = null;
    public int starNumber = 2;
    private float starInitialSpeed = 3;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject parent = transform.parent.gameObject;
            GameObject blackEnemy = parent.transform.GetChild(1).gameObject;
            bool isReversed = false;
            starInitialSpeed = parent.GetComponent<EnemyController>().starInitialSpeed;
            isReversed = parent.GetComponent<EnemyController>().enemyReverse;
            if (startPrefab)
            {
                float angle = 0;
                for (int i = 0; i < starNumber; i++)
                {
                    Quaternion rotation = new Quaternion();
                    angle += 90f / starNumber;
                    Debug.Log(angle);
                    rotation.eulerAngles = new Vector3(0.0f, 0.0f,  angle+45f / starNumber);
                    var star = Instantiate(startPrefab, blackEnemy.transform.position, rotation);
                    star.GetComponent<Rigidbody2D>().gravityScale = isReversed ? -1.0f : 1.0f;
                    star.GetComponent<Rigidbody2D>().velocity = new Vector2(0,
                        isReversed ? -1.0f * starInitialSpeed : 1.0f * starInitialSpeed);
                }
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
