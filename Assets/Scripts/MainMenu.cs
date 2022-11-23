using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        DataManager.GetSessionID();
    }

    void Start()
    {
        // Transform[] children = GetComponentsInChildren<Transform>();
        // foreach (var child in children)
        // {
        //     Debug.Log(child.name);
        // }
        GameObject[] checks = GameObject.FindGameObjectsWithTag("Check");
        for (var i = 0; i < checks.Length; i++)
        {
            //Debug.Log(checks[i]);
            string levelName = "Level" + checks[i].name;
            Transform check = checks[i].gameObject.transform.GetChild(1);
            Transform tutorial = checks[i].gameObject.transform.GetChild(2);
            Boolean status = LevelStatus.completeLevels.Contains(levelName);
            Boolean isTutorial = LevelStatus.tutorialLevels.Contains(levelName);
            check.gameObject.SetActive(status);
            tutorial.gameObject.SetActive(isTutorial);
        }
    }

    public void PlayLevel1_1()
    {
        SceneManager.LoadScene("Level1-1");
        DataManager.levelName = "Level1-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel1_2()
    {
        SceneManager.LoadScene("Level1-2");
        DataManager.levelName = "Level1-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel1_3()
    {
        SceneManager.LoadScene("Level1-3");
        DataManager.levelName = "Level1-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel1_4()
    {
        SceneManager.LoadScene("Level1-4");
        DataManager.levelName = "Level1-4";
        DataManager.GetStartTime();
    }

    public void PlayLevel2_1()
    {
        SceneManager.LoadScene("Level2-1");
        DataManager.levelName = "Level2-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel2_2()
    {
        SceneManager.LoadScene("Level2-2");
        DataManager.levelName = "Level2-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel2_3()
    {
        SceneManager.LoadScene("Level2-3");
        DataManager.levelName = "Level2-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel3_1()
    {
        SceneManager.LoadScene("Level3-1");
        DataManager.levelName = "Level3-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel3_2()
    {
        SceneManager.LoadScene("Level3-2");
        DataManager.levelName = "Level3-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel3_3()
    {
        SceneManager.LoadScene("Level3-3");
        DataManager.levelName = "Level3-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel3_4()
    {
        SceneManager.LoadScene("Level3-4");
        DataManager.levelName = "Level3-4";
        DataManager.GetStartTime();
    }

    public void PlayLevel4_1()
    {
        SceneManager.LoadScene("Level4-1");
        DataManager.levelName = "Level4-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel4_2()
    {
        SceneManager.LoadScene("Level4-2");
        DataManager.levelName = "Level4-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel4_3()
    {
        SceneManager.LoadScene("Level4-3");
        DataManager.levelName = "Level4-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel5_1()
    {
        SceneManager.LoadScene("Level5-1");
        DataManager.levelName = "Level5-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel5_2()
    {
        SceneManager.LoadScene("Level5-2");
        DataManager.levelName = "Level5-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel5_3()
    {
        SceneManager.LoadScene("Level5-3");
        DataManager.levelName = "Level5-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel6_1()
    {
        SceneManager.LoadScene("Level6-1");
        DataManager.levelName = "Level6-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel6_2()
    {
        SceneManager.LoadScene("Level6-2");
        DataManager.levelName = "Level6-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel6_3()
    {
        SceneManager.LoadScene("Level6-3");
        DataManager.levelName = "Level6-3";
        DataManager.GetStartTime();
    }

    public void PlayLevel7_1()
    {
        SceneManager.LoadScene("Level7-1");
        DataManager.levelName = "Level7-1";
        DataManager.GetStartTime();
    }

    public void PlayLevel7_2()
    {
        SceneManager.LoadScene("Level7-2");
        DataManager.levelName = "Level7-2";
        DataManager.GetStartTime();
    }

    public void PlayLevel7_3()
    {
        SceneManager.LoadScene("Level7-3");
        DataManager.levelName = "Level7-3";
        DataManager.GetStartTime();
    }
}
