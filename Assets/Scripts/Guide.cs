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


public class Guide : MonoBehaviour
{
    public GuideInfo[] guideInfos;
    public GameObject player1;
    public GameObject player2;
    //private int countDown0;
    //private int countDown1;
    //private int countDown2;
    //private int countDown3;
    //private int countDown4;
    //public int countDown5;
    //public int countDown6;
    //public int countDown7;
    //public int countDown8;
    // Start is called before the first frame update
    //[System.Serializable]
    void Start()
    {
        //countDown0 = 0;
        //countDown1 = 0;
        //countDown2 = 0;
        //countDown3 = 0;
        //countDown4 = 0;
        //countDown5 = 0;
        //countDown6 = 0;
        //countDown7 = 0;
        //countDown8 = 0;
        //gameObject.transform.alpha = 0;
        //gameObject.transform.position = new Vector3(transform.position.x, -200, transform.position.z);
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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
                if (guideInfos[i].countDown > guideInfos[i].dropStart && guideInfos[i].countDown < 1990 && GetComponent<RectTransform>().offsetMax.y > guideInfos[i].heightUp)
                {
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - guideInfos[i].dropSpeed, transform.position.z);
                }
                if (guideInfos[i].countDown > guideInfos[i].dropEnd && GetComponent<RectTransform>().offsetMax.y < guideInfos[i].heightDown)
                {
                    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + guideInfos[i].dropSpeed, transform.position.z);
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





        //if (mid_x > -30 && mid_x < -12)
        //{
        //    if (countDown0 <= 6000)
        //    {
        //        countDown0++;
        //    }
        //    if (countDown0> 250 && countDown0 < 1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //    }
        //    if (countDown0 > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //    }
        //    countDown1 = 0;
        //    countDown2 = 0;
        //    countDown3 = 0;
        //    countDown4 = 0;
        //    //countDown5 = 0;
        //    //countDown6 = 0;
        //    //countDown7 = 0;
        //    //countDown8 = 0;
        //}

        //if (mid_x > -12 && mid_x < 7)
        //{

        //    if (countDown1 <= 6000)
        //    {
        //        countDown1++;
        //    }
        //    if (countDown1 > 250 && countDown1 < 1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //    }
        //    if (countDown1 > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //    }
        //    countDown0 = 0;
        //    countDown2 = 0;
        //    countDown3 = 0;
        //    countDown4 = 0;
        //    //countDown5 = 0;
        //    //countDown6 = 0;
        //    //countDown7 = 0;
        //    //countDown8 = 0;
        //}
        //if (mid_x > 7 && mid_x < 60)
        //{

        //    if (countDown2 <= 6000)
        //    {
        //        countDown2++;
        //    }
        //    if (countDown2 > 100 && countDown1 < 1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //    }
        //    if (countDown2 > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //    }
        //    countDown0 = 0;
        //    countDown1 = 0;
        //    countDown3 = 0;
        //    countDown4 = 0;
        //    //countDown5 = 0;
        //    //countDown6 = 0;
        //    //countDown7 = 0;
        //    //countDown8 = 0;
        //}
        //if (mid_x > 60 && mid_x < 120)
        //{

        //    if (countDown3 <= 6000)
        //    {
        //        countDown3++;
        //    }
        //    if (countDown3 > 250 && countDown1 < 1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //    }
        //    if (countDown3 > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //    }
        //    countDown0 = 0;
        //    countDown1 = 0;
        //    countDown2 = 0;
        //    countDown4 = 0;
        //    //countDown5 = 0;
        //    //countDown6 = 0;
        //    //countDown7 = 0;
        //    //countDown8 = 0;
        //}
        //if (mid_x > 120 && mid_x < 200)
        //{

        //    if (countDown4 <= 6000)
        //    {
        //        countDown4++;
        //    }
        //    if (countDown4 > 250 && countDown1 < 1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //    }
        //    if (countDown4 > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //    {
        //        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //    }
        //    countDown0 = 0;
        //    countDown1 = 0;
        //    countDown2 = 0;
        //    countDown3 = 0;
        //    //countDown5 = 0;
        //    //countDown6 = 0;
        //    //countDown7 = 0;
        //    //countDown8 = 0;
        //}
        //GetComponent<RectTransform>().offsetMax.y
        //if (countDown>250 && countDown<1990 && GetComponent<RectTransform>().offsetMax.y > -40)
        //{
        //    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        //}

        //if (countDown > 5000 && GetComponent<RectTransform>().offsetMax.y < 50)
        //{
        //    gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //}
    }
}
