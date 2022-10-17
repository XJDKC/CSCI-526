using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityReverse : MonoBehaviour
{
    // public GameObject player1;
    // public GameObject player2;
    public int countDown;
    private float _x;
    private float _y;
    private Boolean _xPositive;
    private Boolean _yPositive;
    private Text _text;

    private void Awake()
    {
        var position = gameObject.transform.position;
        _x = position.x;
        _y = position.y;
        _xPositive = true;
        _yPositive = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        _text = gameObject.GetComponentInChildren<Text>();
        // countDown = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > _x + 0.03)
        {
            _xPositive = false;
        }
        if (gameObject.transform.position.x < _x - 0.03)
        {
            _xPositive = true;
        }
        if (gameObject.transform.position.y > _y + 0.1)
        {
            _yPositive = false;
        }
        if (gameObject.transform.position.y < _y - 0.1)
        {
            _yPositive = true;
        }
        if (_xPositive)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.001f,gameObject.transform.position.y,gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.001f,gameObject.transform.position.y,gameObject.transform.position.z);
        }
        if (_yPositive)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 0.0005f,gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 0.0005f,gameObject.transform.position.z);
        }
        _text.text = countDown + "";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            countDown--;
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                // var playerType = player.GetComponent<PlayerController>().playerType;
                if (collider.GetComponent<PlayerController>().playerType == player.GetComponent<PlayerController>().playerType)
                {
                    collider.GetComponent<PlayerController>().Reverse();
                }
            }

            if (countDown <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
