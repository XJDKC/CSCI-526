using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Press key R to restart the game
public class LevelUIController : MonoBehaviour
{
    public string levelSelectSceneName = "Menu";
    public int minimum;
    public int maximum;

    private bool _reachedTargetScore = false;
    private string _currSceneName;
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _scoreText.color = Color.red;
    }

    // get active scene
    private void Start()
    {
        _currSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    private void Update()
    {
        if (DataManager.currentStarPoints >= minimum)
        {
            _reachedTargetScore = true;
            _scoreText.color = Color.green * 0.8f;
        }

        if (maximum == 0)
            _scoreText.text = "Score: " + maximum;
        else
            _scoreText.text = "Score: " + DataManager.currentStarPoints + "/" + maximum;
    }

    public bool GetStatus()
    {
        return _reachedTargetScore;
    }

    // reload the same active scence after pressing the button
    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene(_currSceneName);
        DataManager.RestartLevel();
        DataManager.GetStartTime();
    }

    // reload the same active scence after pressing the button
    public void OnExitButtonPress()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }

    // prompt hints
    public void OnHintsButtonPress()
    {
        var guideUIController = FindObjectOfType<GuideUIController>();
        if (guideUIController) guideUIController.hintsButton();
    }
}
