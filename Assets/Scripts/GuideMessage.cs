using System;
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

//Default value of Level 1:
//Text1: "Player1: W S A D.  Player2: Up Down Left Right.      ENJOY!"
//Text2: "Try to go through the door to change the gravity."
//Text3: "Kill enemies by hitting their white spirits, \n and don't forget to collect stars from other side."
//Text4: "How about....build a 'Human Ladder?'"
//Text5: "Wait... Isn't it too high even for 'Human Ladder'?            Maybe it's a good idea to use the platform over there."
//Change point: -30, -12, 7, 60, 120, 200

public class GuideMessage : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Message[] messages;
    private Boolean reach;
    // Start is called before the first frame update
    void Start()
    {
        reach = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reach)
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
        }
    }
    public void minimumScoreText()
    {
        reach = true;
        GetComponent<Text>().text = "Minimum score requirement not reached. \n Try to get more stars!";
    }
}
