using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string url;

    public static int currentStarPoints;
    public static long sessionId;
    public static string endStatus;
    public static long startTime;
    public static long endTime;
    public static string levelName;
    public static float gameSeconds;
    public static int portalTimes;
    public static int springTimes;
    public static int cookieTimes;

    //point for a single star
    private int _starPoint = 1;

    private static DataManager _instance;
    public static DataManager Instance { get { return _instance;  } }

    void Awake()
    {
        //sessionId = DateTime.Now.Ticks;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStarPoints = 0;
        url = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdLMoxH4aXAOXu_kRxxODsHIG14is4OP-7tnWGqBNDjygMTZw/formResponse";
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update current starPoints
    public void AddStarPoints()
    {
        currentStarPoints += _starPoint;
        //Debug.Log("points: " + currentStarPoints);
    }

    public void GetDeathReason(GameObject Enemy)
    {
        string parent = Enemy.transform.parent.name;
        Debug.Log("enemy: " + parent);
        endStatus = parent;
        GetEndTime();
        //TODO: call data post
        if (_instance == null)
            Debug.Log("Null Instance1");
        Send();
    }

    //get start time when a level starts
    public void GetStartTime()
    {
        startTime = DateTime.Now.Ticks;
    }

    //get end time when a level ends(die or complete)
    public void GetEndTime()
    {
        endTime = DateTime.Now.Ticks;
    }

    public void CompleteLevel()
    {
        endStatus = "Completed";
        GetEndTime();
        //TODO: call data post
        Send();
    }

    public void RestartLevel()
    {
        endStatus = "Restart";
        GetEndTime();
        Send();
    }

    public void Send()
    {
        Debug.Log("--Collecting data--");
        //Debug.Log("starQuantity: " + starQuantity);
        gameSeconds = (endTime - startTime) / 10000000;
        //Debug.Log(gameSeconds);
        StartCoroutine(Post());
        currentStarPoints = 0;
        //PostData(starQuantity.ToString());
    }

    //arg1: coinQuantity(read from STarUI), arg2: deathReason(read from BlackEnemyConfig, get parent.name)
    private IEnumerator Post()
    {
        Debug.Log("--Posting data--");
        WWWForm form = new WWWForm();
        //Change according to google form html
        form.AddField("entry.244983340", sessionId.ToString());
        form.AddField("entry.1862673262", endStatus);
        form.AddField("entry.1387275365", currentStarPoints.ToString());
        form.AddField("entry.1737131638", levelName);
        form.AddField("entry.232040088", gameSeconds.ToString());
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
}
