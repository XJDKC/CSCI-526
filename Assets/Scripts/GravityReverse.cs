using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityReverse : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public int countDown;
    private float x;
    private float y;
    private Boolean xPositive;
    private Boolean yPositive;

    // Start is called before the first frame update
    void Start()
    {
        // countDown = 5;
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.y;
        xPositive = true;
        yPositive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > x + 0.03)
        {
            xPositive = false;
        }
        if (gameObject.transform.position.x < x - 0.03)
        {
            xPositive = true;
        }
        if (gameObject.transform.position.y > y + 0.1)
        {
            yPositive = false;
        }
        if (gameObject.transform.position.y < y - 0.1)
        {
            yPositive = true;
        }

        if (xPositive)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.001f,gameObject.transform.position.y,gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.001f,gameObject.transform.position.y,gameObject.transform.position.z);

        }

        if (yPositive)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 0.0005f,gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 0.0005f,gameObject.transform.position.z);
        }




        // gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.01f,gameObject.transform.position.y - 0.01f,gameObject.transform.position.z);
        gameObject.GetComponentInChildren<Text>().text = countDown + "";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            countDown--;
            player1.GetComponent<PlayerController>().Reverse();
            player2.GetComponent<PlayerController>().Reverse();
            if (countDown <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


}
