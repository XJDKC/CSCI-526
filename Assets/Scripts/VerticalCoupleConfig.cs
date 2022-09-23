using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCoupleConfig : MonoBehaviour
{
    [SerializeField] private float distanceBetweenEnemies = 4;
    [SerializeField] private bool EnemyReverse = false;

    private GameObject whiteEnemy;
    private GameObject blackEnemy;
    private int isReverse;

    // Start is called before the first frame update
    void Start()

    {
        isReverse = EnemyReverse == false ? 1 : -1;
        whiteEnemy = transform.GetChild(0).gameObject;
        blackEnemy = transform.GetChild(1).gameObject;
        whiteEnemy.transform.localPosition = new Vector3(0, -(distanceBetweenEnemies * isReverse), 0);
        blackEnemy.transform.localPosition = new Vector3(0, +(distanceBetweenEnemies * isReverse), 0);
        whiteEnemy.GetComponent<Rigidbody2D>().gravityScale = -1 * isReverse;
        blackEnemy.GetComponent<Rigidbody2D>().gravityScale = 1 * isReverse;
    }
}
