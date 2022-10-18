using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel1()
    {
        SceneManager.LoadScene(6);
    }

    public void PlayTutorialLevel0()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayTutorialLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTutorialLevel2()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayTutorialLevel3()
    {
        SceneManager.LoadScene(4);
    }

    public void PlayTutorialLevel4()
    {
        SceneManager.LoadScene(5);
    }
    
    public void PlayTutorialLevel5()
    {
        SceneManager.LoadScene(6);
    }
}
