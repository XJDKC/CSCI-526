using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public int startStarQuantity;
    public static int CurrentStarQuantity;

    // Start is called before the first frame update
    void Start()
    {
        CurrentStarQuantity = startStarQuantity;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Score: " + CurrentStarQuantity;
    }
}
