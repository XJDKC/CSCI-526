using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[Serializable]
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

[Serializable]
public struct Message
{
    public string text;
    public int start;
    public int end;
}

//Default value of Prompt Guide:
//countDown: 0
//dropStart: 120
//dropEnd: 5000
//dropSpeed: 0.4
//heightUp: -60
//heightDown: 70

//Default value of Prompt Text of Level 1:
//Text1: "Player1: W S A D.  Player2: Up Down Left Right.      ENJOY!"
//Text2: "Try to go through the door to change the gravity."
//Text3: "Kill enemies by hitting their white spirits, \n and don't forget to collect stars from other side."
//Text4: "How about....build a 'Human Ladder?'"
//Text5: "Wait... Isn't it too high even for 'Human Ladder'?            Maybe it's a good idea to use the platform over there."
//Change point: -30, -12, 7, 60, 120, 200

public class GuideUIController : MonoBehaviour
{
    public GuideInfo[] guideInfos;
    public Message[] messages;
    private GameObject _player1;
    private GameObject _player2;
    private Boolean reach;
    public int hU;
    public float dS;
    private Boolean hints;


    void Start()
    {
        reach = false;
        hints = false;
        if (hU == 0)
        {
            hU = 0;
        }

        if (dS == 0)
        {
            dS = 0.4f;
        }

        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                _player1 = player;
            }

            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player2)
            {
                _player2 = player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player1 != null || _player2 != null)
        {
            if (!reach)
            {
                Vector3 pos1 = _player1.transform.position;
                Vector3 pos2 = _player2.transform.position;
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

                for (int i = 0; i < messages.Length; i++)
                {
                    if (mid_x > messages[i].start && mid_x < messages[i].end)
                    {
                        GameObject.Find("TextPrompt").GetComponent<TextMeshProUGUI>().text = messages[i].text;
                    }
                }
            }
        }

        if (hints)
        {
            if (GetComponent<RectTransform>().offsetMax.y > -60)
            {
                gameObject.transform.position = new Vector3(transform.position.x,
                    transform.position.y - 0.6f, transform.position.z);
            }
        }
        else
        {
            if (GetComponent<RectTransform>().offsetMax.y < 90)
            {
                gameObject.transform.position = new Vector3(transform.position.x,
                    transform.position.y + 0.6f, transform.position.z);
            }
        }
    }

    public void hintsButton()
    {
        if (hints == true)
        {
            hints = false;
        }
        else
        {
            hints = true;
        }
    }

    public void minimumScorePanel()
    {
        reach = true;
        if (GetComponent<RectTransform>().offsetMax.y > hU)
        {
            gameObject.transform.position = new Vector3(transform.position.x,
                transform.position.y - dS, transform.position.z);
        }
    }
}
