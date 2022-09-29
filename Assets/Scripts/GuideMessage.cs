using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public struct Message
{
    public string text;
    public int start;
    public int end;


}

public class GuideMessage : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public Message[] messages;


    //public List<string> listString;
    //public int a;
    //public List<int[]> listPos;
    //public int[][] a = new int[2][10];
    //public int startStarQuantity;
    //public static int CurrentStarQuantity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos1 = player1.GetComponent<Transform>().position;
        Vector3 pos2 = player2.GetComponent<Transform>().position;
        float mid_x = (pos1.x + pos2.x) / 2;
        for (int i = 0; i < messages.Length; i++)
        {
            if (mid_x > messages[i].start && mid_x < messages[i].end)
            {
                GetComponent<Text>().text = messages[i].text;
            }
        }
            //print(startStarQuantity);
            //GetComponent<RectTransform>().offsetMax.y
        //if (mid_x > -30 && mid_x < -12)
        //{
        //    GetComponent<Text>().text = messages[0].text;
        //}
        //if (mid_x > -12 && mid_x < 7)
        //{
        //    GetComponent<Text>().text = listString[1];
        //}
        //if (mid_x > 7 && mid_x < 60)
        //{
        //    if (StarUI.CurrentStarQuantity == 0)
        //    {
        //        GetComponent<Text>().text = "Kill enemies by hitting their white spirits, \n and don't forget to collect stars from enemys.";
        //    }
        //    else
        //    {
        //        GetComponent<Text>().text = "Hooray!!! You got them!!";
        //    }
        //}
        //if (mid_x > 60 && mid_x < 120)
        //{
        //    GetComponent<Text>().text = "How about....'double jump' on each other?";
        //}
        //if (mid_x > 120 && mid_x < 200)
        //{
        //    GetComponent<Text>().text = "Wait... Isn't it too high even for 'double jump'? \n Maybe it's a good idea to use the platform over there.";
        //}
    }
}
