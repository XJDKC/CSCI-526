using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Press key R to restart the game
public class RestartBtn : MonoBehaviour
{
//     public void OnButtonPress(){
//     //Debug.Log("Button restart is being pressed!");
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

    void Update() {

       if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
   }
}
