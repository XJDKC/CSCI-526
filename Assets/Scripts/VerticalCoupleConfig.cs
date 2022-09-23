using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCoupleConfig : MonoBehaviour
{
    [SerializeField] private float distanceBetweenEnemies = 4;
    [SerializeField] private int isReverse = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).localPosition = new Vector3(0, -distanceBetweenEnemies * isReverse, 0);
        transform.GetChild(1).localPosition = new Vector3(0, distanceBetweenEnemies * isReverse, 0);
    }
}
