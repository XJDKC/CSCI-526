using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct GuideInfo
{
    public int countDown;
    public int start;
    public int end;
    public int dropStart;
    public int dropEnd;
    public float dropSpeed;
    public int heightUp;
    public int heightDown;
}

//Default value of Prompt Guide:
//countDown: 0
//dropStart: 120
//dropEnd: 5000
//dropSpeed: 0.4
//heightUp: -60
//heightDown: 50

public class Guide : MonoBehaviour
{
    public GuideInfo[] guideInfos;
    public GameObject player1;
    public GameObject player2;
    private Boolean reach;
    public int hU;
    public float dS;


    void Start()
    {
        reach = false;
        if (hU == 0)
        {
            hU = -60;
        }
        if (dS == 0)
        {
            dS = 0.4f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!reach)
        {
            Vector3 pos1 = player1.GetComponent<Transform>().position;
            Vector3 pos2 = player2.GetComponent<Transform>().position;
            float mid_x = (pos1.x + pos2.x) / 2;
            for (int i = 0; i < guideInfos.Length; i++)
            {
                if (mid_x > guideInfos[i].start && mid_x < guideInfos[i].end)
                {
                    if (guideInfos[i].countDown <= 6000)
                    {
                        guideInfos[i].countDown = guideInfos[i].countDown + 1;
                    }

                    if (guideInfos[i].countDown > guideInfos[i].dropStart && guideInfos[i].countDown < 1990 &&
                        GetComponent<RectTransform>().offsetMax.y > guideInfos[i].heightUp)
                    {
                        gameObject.transform.position = new Vector3(transform.position.x,
                            transform.position.y - guideInfos[i].dropSpeed, transform.position.z);
                    }

                    if (guideInfos[i].countDown > guideInfos[i].dropEnd &&
                        GetComponent<RectTransform>().offsetMax.y < guideInfos[i].heightDown)
                    {
                        gameObject.transform.position = new Vector3(transform.position.x,
                            transform.position.y + guideInfos[i].dropSpeed, transform.position.z);
                    }

                    for (int j = 0; j < guideInfos.Length; j++)
                    {
                        if (j != i)
                        {
                            guideInfos[j].countDown = 0;
                        }
                    }
                }
            }
        }
    }

    public void mininumScorePanel()
    {
        reach = true;
        if (GetComponent<RectTransform>().offsetMax.y > hU)
        {
            gameObject.transform.position = new Vector3(transform.position.x,
                transform.position.y - dS, transform.position.z);
        }
    }
}
