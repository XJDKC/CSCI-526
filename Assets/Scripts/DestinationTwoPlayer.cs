using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestinationTwoPlayer : MonoBehaviour
{
    public float validateTime = 0.25f;
    private string easterEggSceneName = "EasterEgg";

    private bool _player1Arr;
    private bool _player2Arr;
    private GameObject _player1Des;
    private GameObject _player2Des;
    private float _lastArriveTime = -1.0f;

    private LevelUIController _levelUIController;
    private FinalUIController _finalUIController;
    private GuideUIController _guideUIController;


    private void Awake()
    {
        _levelUIController = FindObjectOfType<LevelUIController>();
        _finalUIController = FindObjectOfType<FinalUIController>();
        _guideUIController = FindObjectOfType<GuideUIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _player1Des = transform.GetChild(0).gameObject;
        _player2Des = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player1Des && _player1Des)
        {
            _player1Des.SetActive(_player1Arr);
            _player2Des.SetActive(_player2Arr);
        }

        if (!_player1Arr || !_player2Arr)
        {
            _lastArriveTime = -1.0f;
            return;
        }

        if (_lastArriveTime < 0.0f) _lastArriveTime = Time.realtimeSinceStartup;
        if (Time.realtimeSinceStartup - _lastArriveTime < validateTime) return;
        if (!_levelUIController || !_finalUIController) return;
        if (_finalUIController.GetPanelState() != FinalUIController.PanelState.Idle) return;
        if (!_levelUIController.GetStatus())
        {
            // Fail state
            if (_guideUIController) _guideUIController.minimumScorePanel();
            if (_finalUIController) _finalUIController.SwitchState(FinalUIController.PanelState.Fail);
        }
        else
        {
            // Success State
            bool prevState = LevelStatus.CompleteAllLevels();
            DataManager.CompleteLevel();
            bool currState = LevelStatus.CompleteAllLevels();
            DataManager.CompleteLevel();
            if (!prevState && currState)
            {
                SceneManager.LoadScene(easterEggSceneName);
            }
            else if (_finalUIController)
            {
                _finalUIController.SwitchState(FinalUIController.PanelState.Success);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        UpdateArriveState(other, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        UpdateArriveState(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        UpdateArriveState(other, false);
    }

    private void UpdateArriveState(Collider2D other, bool status)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                _player1Arr = status;
            }
            else
            {
                _player2Arr = status;
            }
        }
    }
}
