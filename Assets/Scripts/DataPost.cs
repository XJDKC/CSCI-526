using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataPost : MonoBehaviour
{
    [SerializeField] private string url;
    //private long sessionID;
    //public static DataPost Sender;

    //Updating when player dies(IN BlackEnemyConfig.cs)
    public string deathReason;

    public void Awake()
    {
        //sessionID = DateTime.Now.Ticks;
        //Send();
    }

    public void Send()
    {
        Debug.Log("--Collecting data--");
        int starQuantity = StarUI.CurrentStarQuantity;
        //Debug.Log("starQuantity: " + starQuantity);
        StartCoroutine(Post(starQuantity.ToString(), deathReason));
        //PostData(starQuantity.ToString());
    }

    //arg1: coinQuantity(read from STarUI), arg2: deathReason(read from BlackEnemyConfig, get parent.name)
    private IEnumerator Post(string coinQuantity, string death)
    {
        Debug.Log("--Posting data--");
        WWWForm form = new WWWForm();
        //Change according to google form html
        form.AddField("entry.431904651", coinQuantity);
        form.AddField("entry.63869635", death);
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            //Debug.Log("--Sending request--");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data upload complete");
            }
        }
    }

    void Start()
    {
        url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSc2_3lUNjf_LrLXkZq4bnL_1r7bl3CenBpxwVnB6e0eVjFZJg/formResponse";
    }
}
