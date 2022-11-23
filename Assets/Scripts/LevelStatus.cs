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
    public static List<string> levelList = new List<string>();

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
            tutorialLevels.Add("Level8-1");
        }

        if (levelList.Count == 0)
        {
            levelList.Add("Level1-1");
            levelList.Add("Level1-2");
            levelList.Add("Level1-3");
            levelList.Add("Level1-4");
            levelList.Add("Level2-1");
            levelList.Add("Level2-2");
            levelList.Add("Level2-3");
            levelList.Add("Level3-1");
            levelList.Add("Level3-2");
            levelList.Add("Level3-3");
            levelList.Add("Level4-1");
            levelList.Add("Level4-2");
            levelList.Add("Level4-3");
            levelList.Add("Level5-1");
            levelList.Add("Level5-2");
            levelList.Add("Level5-3");
            levelList.Add("Level6-1");
            levelList.Add("Level6-2");
            levelList.Add("Level6-3");
            levelList.Add("Level7-1");
            levelList.Add("Level7-2");
            levelList.Add("Level7-3");
            levelList.Add("Level8-1");
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
