using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public int startStarQuantity;
    //public static int CurrentStarQuantity;
    //public static int numsThroughGate;
    public static DateTime startTime;
    public int minimum;
    public int maximum;
    private Boolean reached;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Score: " + DataManager.currentStarPoints;
    }
}
