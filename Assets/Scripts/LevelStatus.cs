using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatus : MonoBehaviour
{
    private static LevelStatus _instance;
    public static LevelStatus Instance { get { return _instance; } }

    public static Hashtable completeStatus = new Hashtable();

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
        if (completeStatus.Count == 0)
        {
            completeStatus.Add("Level1-1", false);
            completeStatus.Add("Level1-2", false);
            completeStatus.Add("Level1-3", false);
            completeStatus.Add("Level1-4", false);
            completeStatus.Add("Level2-1", false);
            completeStatus.Add("Level2-2", false);
            completeStatus.Add("Level2-3", false);
            completeStatus.Add("Level3-1", false);
            completeStatus.Add("Level3-2", false);
            completeStatus.Add("Level3-3", false);
            completeStatus.Add("Level4-1", false);
            completeStatus.Add("Level4-2", false);
            completeStatus.Add("Level4-3", false);
            completeStatus.Add("Level5-1", false);
            completeStatus.Add("Level5-2", false);
            completeStatus.Add("Level5-3", false);
            completeStatus.Add("Level6-1", false);
            completeStatus.Add("Level6-2", false);
            completeStatus.Add("Level7-1", false);
            completeStatus.Add("Level7-2", false);
            completeStatus.Add("Level7-3", false);
        }
    }

    public static void ChangeLevelStatus()
    {
        if (Instance)
        {
            string level = DataManager.levelName;
            Debug.Log(level);
            Debug.Log(completeStatus[level]);
            completeStatus.Remove(level);
            completeStatus.Add(level, true);
            Debug.Log(completeStatus[level]);
        }
        else
        {
            Debug.Log("Not valid instance");
        }
    }
}
