using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public int minimum;
    public int maximum;
    private Boolean reached;


    // Start is called before the first frame update
    void Start()
    {
        reached = false;
        GetComponent<TextMeshProUGUI>().color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (DataManager.currentStarPoints >= minimum)
        {
            reached = true;
            GetComponent<TextMeshProUGUI>().color = Color.green * 0.8f;
        }

        if (maximum == 0)
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + maximum;
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "Score: " + DataManager.currentStarPoints + "/" + maximum;
        }
    }

    public Boolean getStatus()
    {
        return reached;
    }
}
