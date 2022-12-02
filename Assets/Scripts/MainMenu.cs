using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Level1-1");
        DataManager.levelName = "Level1-1";
        DataManager.GetStartTime();
    }

    public void DevelopersList()
    {
        SceneManager.LoadScene("DevelopersList");
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Return()
    {
        SceneManager.LoadScene("StartScene");
    }

}
