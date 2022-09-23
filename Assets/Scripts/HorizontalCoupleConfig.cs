using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HorizontalCoupleConfig : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float moveDistancce = 1;
    [SerializeField] private float gapBetweenEnemy = 1;
    [SerializeField] private int isReverse = 1;
    private Transform whiteEnemy;
    private Transform blackEnemy;

    private bool direction = true;

    // Start is called before the first frame update
    void Start()
    {
        whiteEnemy = transform.GetChild(0);
        blackEnemy = transform.GetChild(1);
        whiteEnemy.localPosition = new Vector3(0, -gapBetweenEnemy*isReverse, 0);
        blackEnemy.localPosition = new Vector3(0, +gapBetweenEnemy*isReverse, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.IsDestroyed())
        {
            if (direction == true && whiteEnemy.position.x - transform.position.x >= moveDistancce)
            {
                direction = false;
            }

            if (direction == false && transform.position.x - whiteEnemy.position.x >= moveDistancce)
            {
                direction = true;
            }

            if (direction == true)
            {
                whiteEnemy.position =
                    new Vector3(whiteEnemy.position.x + Time.deltaTime * speed, isReverse*whiteEnemy.position.y, 0);
                blackEnemy.position =
                    new Vector3(blackEnemy.position.x + Time.deltaTime * speed, isReverse*blackEnemy.position.y, 0);
            }
            else
            {
                whiteEnemy.position =
                    new Vector3(whiteEnemy.position.x - Time.deltaTime * speed, isReverse*whiteEnemy.position.y, 0);
                blackEnemy.position =
                    new Vector3(blackEnemy.position.x - Time.deltaTime * speed, isReverse*blackEnemy.position.y, 0);
            }
        }
    }
}
