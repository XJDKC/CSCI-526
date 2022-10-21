using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    private int startStarQuantity;
    public static int CurrentStarQuantity;
    public static int numsThroughGate;
    public static DateTime startTime;
    public int minimum;
    public int maximum;
    private Boolean reached;


    // Start is called before the first frame update
    void Start()
    {
        reached = false;
        GetComponent<Text>().color = Color.red;
        CurrentStarQuantity = startStarQuantity;
        numsThroughGate = 0;
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentStarQuantity >= minimum)
        {
            reached = true;
            GetComponent<Text>().color = Color.black;
        }
        GetComponent<Text>().text = "Score: " + DataManager.currentStarPoints + "/" + maximum;
    }

    public Boolean getStatus()
    {
        return reached;
    }
}
