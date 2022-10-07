using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public int startStarQuantity;
    public static int CurrentStarQuantity;
    public static int numsThroughGate;
    public static DateTime startTime;

    // Start is called before the first frame update
    void Start()
    {
        CurrentStarQuantity = startStarQuantity;
        numsThroughGate = 0;
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Score: " + CurrentStarQuantity;
    }
}
