using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Press key R to restart the game
public class Btns : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private string sceneMenu = "Menu";

    // get active scene
    private void Start()
    {
        scene = SceneManager.GetActiveScene().name;
    }

    // reload the same active scence after pressing the button
    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene(scene);
        DataManager.RestartLevel();
        DataManager.GetStartTime();
    }

    // reload the same active scence after pressing the button
    public void OnExitButtonPress()
    {
        SceneManager.LoadScene(sceneMenu);
    }

    // prompt hints
    public void OnHintsButtonPress(){
        var guide = GameObject.FindObjectOfType<Guide>();
        if (guide)
        {
            guide.hintsButton();
        }
    }
}
