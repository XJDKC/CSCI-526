using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

// Press key R to restart the game
public class Btns : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private string sceneMenu = "Menu";

    public int minimum;
    public int maximum;
    private Boolean reached;
    private TextMeshProUGUI _textMeshPro;
    // get active scene
    void Start()
    {
        scene = SceneManager.GetActiveScene().name;

        reached = false;
        _textMeshPro = GameObject.Find("ScoreTMP").GetComponent<TextMeshProUGUI>();
        _textMeshPro.color = Color.red;
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



    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     reached = false;
    //     _textMeshPro = GameObject.Find("ScoreTMP").GetComponent<TextMeshProUGUI>();
    //     _textMeshPro.color = Color.red;
    // }

    // Update is called once per frame
    void Update()
    {
        if (DataManager.currentStarPoints >= minimum)
        {
            reached = true;
            _textMeshPro.color = Color.green * 0.8f;
        }

        if (maximum == 0)
        {
            _textMeshPro.text = "Score: " + maximum;
        }
        else
        {
            _textMeshPro.GetComponent<TextMeshProUGUI>().text = "Score: " + DataManager.currentStarPoints + "/" + maximum;
        }
    }

    public Boolean getStatus()
    {
        return reached;
    }
}
