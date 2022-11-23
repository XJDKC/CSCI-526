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
    public static HashSet<string> tutorialLevels = new HashSet<string>();

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
        if (tutorialLevels.Count == 0)
        {
            tutorialLevels.Add("Level1-1");
            tutorialLevels.Add("Level1-2");
            tutorialLevels.Add("Level1-3");
            tutorialLevels.Add("Level1-4");
            tutorialLevels.Add("Level2-1");
            tutorialLevels.Add("Level3-1");
            tutorialLevels.Add("Level4-1");
            tutorialLevels.Add("Level5-1");
            tutorialLevels.Add("Level6-1");
            tutorialLevels.Add("Level7-1");
        }
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
