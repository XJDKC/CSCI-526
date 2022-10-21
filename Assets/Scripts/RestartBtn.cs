using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Press key R to restart the game
public class RestartBtn : MonoBehaviour
{
    [SerializeField] private string scene;

    // get active scence
    private void Start() {
        scene = SceneManager.GetActiveScene().name;
    }

    // reload the same active scence after pressing the button
    public void OnButtonPress()
    {
        SceneManager.LoadScene(scene);
        //data manage hook
        DataManager.RestartLevel();
        DataManager.GetStartTime();
    }
}
