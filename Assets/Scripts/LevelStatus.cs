using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatus : MonoBehaviour
{
    private static LevelStatus _instance;
    public static LevelStatus Instance { get { return _instance; } }

    public static Hashtable completeStatus = new Hashtable();
    public static HashSet<string> completeLevels = new HashSet<string>();

    void Awake()
    {
        //Initialize instance
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

    public static void ChangeLevelStatus()
    {
        if (Instance)
        {
            string level = DataManager.levelName;
            Debug.Log(level);
            if (!completeLevels.Contains(level))
            {
                completeLevels.Add(level);
            }
        }
        else
        {
            Debug.Log("Not valid instance");
        }
    }
}
