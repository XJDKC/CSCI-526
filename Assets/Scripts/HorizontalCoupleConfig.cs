using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalCoupleConfig : MonoBehaviour
{
    public float speed = 1;
    public float moveDistancce = 1;
    public float gapBetweenEnemy = 1;
    public bool enemyReverse = false;
    private GameObject whiteEnemy;
    private GameObject blackEnemy;
    private int isReverse;
    private bool direction = true;

    // Start is called before the first frame update
    void Start()
    {
        isReverse = enemyReverse == false ? 1 : -1;
        whiteEnemy = transform.GetChild(0).gameObject;
        blackEnemy = transform.GetChild(1).gameObject;
        whiteEnemy.transform.localPosition = new Vector3(0, -gapBetweenEnemy * isReverse, 0);
        blackEnemy.transform.localPosition = new Vector3(0, +gapBetweenEnemy * isReverse, 0);
        whiteEnemy.GetComponent<Rigidbody2D>().gravityScale = -1 * isReverse;
        blackEnemy.GetComponent<Rigidbody2D>().gravityScale = 1 * isReverse;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.IsDestroyed())
        {
            if (direction == true && whiteEnemy.transform.position.x - transform.position.x >= moveDistancce)
            {
                direction = false;
            }

            if (direction == false && transform.position.x - whiteEnemy.transform.position.x >= moveDistancce)
            {
                direction = true;
            }

            if (direction == true)
            {
                whiteEnemy.transform.position =
                    new Vector3(whiteEnemy.transform.position.x + Time.deltaTime * speed,
                        isReverse * whiteEnemy.transform.position.y * isReverse, 0);
                blackEnemy.transform.position =
                    new Vector3(blackEnemy.transform.position.x + Time.deltaTime * speed,
                        isReverse * blackEnemy.transform.position.y * isReverse, 0);
            }
            else
            {
                whiteEnemy.transform.position =
                    new Vector3(whiteEnemy.transform.position.x - Time.deltaTime * speed,
                        isReverse * whiteEnemy.transform.position.y * isReverse, 0);
                blackEnemy.transform.position =
                    new Vector3(blackEnemy.transform.position.x - Time.deltaTime * speed,
                        isReverse * blackEnemy.transform.position.y * isReverse, 0);
            }
        }
    }
}
