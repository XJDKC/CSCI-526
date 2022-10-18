using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel1()
    {
        SceneManager.LoadScene(5);
    }

    public void PlayTutorialGravityPortal()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayTutorialKillMonsters()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTutorialDoubleJump()
    {
        SceneManager.LoadScene(3);
    }

    public void PlayTutorialHighPlatform()
    {
        SceneManager.LoadScene(4);
    }

    public void PlayTutorialButtons()
    {
        SceneManager.LoadScene(6);
    }

    public void PlayTutorialGravityCookie()
    {
        SceneManager.LoadScene(7);
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene(8);
    }

    public void PlayTutorialBounceBelt()
    {
        SceneManager.LoadScene(9);
    }
}
