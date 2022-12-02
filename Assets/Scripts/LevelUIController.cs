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
    private Color _passColor;
    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = new Color(0.85f, 0.4f, 0.38f);
        _passColor = new Color(0.3f, 1f, 0.78f);
        _scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        _scoreText.color = _defaultColor;
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
            _scoreText.color = _passColor;
        }

        _scoreText.text = DataManager.currentStarPoints + "";
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
