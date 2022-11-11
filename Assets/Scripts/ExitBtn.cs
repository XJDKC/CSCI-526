using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBtn : MonoBehaviour
{

    [SerializeField] private string scene = "Menu";

    // reload the same active scence after pressing the button
    public void OnButtonPress(){
       SceneManager.LoadScene(scene);
       DataManager.ClearStarPoints();
    }
}
