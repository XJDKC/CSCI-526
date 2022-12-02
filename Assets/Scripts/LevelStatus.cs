using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStatus : MonoBehaviour
{
    private static LevelStatus _instance;
    public static LevelStatus Instance { get { return _instance; } }

    public static int TotalLevelCount = 25;
    public static HashSet<string> CompleteLevels = new HashSet<string>();

    public static readonly HashSet<string> TutorialLevels = new HashSet<string>()
    {
        "Level1-1",
        "Level1-2",
        "Level1-3",
        "Level1-4",
        "Level2-1",
        "Level3-1",
        "Level4-1",
        "Level5-1",
        "Level6-1",
        "Level7-1",
        "Level8-1"
    };

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
            string level = SceneManager.GetActiveScene().name;
            Debug.Log(level);
            if (!CompleteLevels.Contains(level))
            {
                CompleteLevels.Add(level);
            }
        }
        else
        {
            Debug.Log("Not valid instance");
        }
    }

    public static bool CompleteAllLevels()
    {
        if (Instance)
        {
            return CompleteLevels.Count == 3;
        }

        return false;
    }
}
