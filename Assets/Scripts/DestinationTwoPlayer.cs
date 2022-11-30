using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestinationTwoPlayer : MonoBehaviour
{
    public float validateTime = 0.25f;

    private bool _player1Arr;
    private bool _player2Arr;
    private GameObject _player1Des;
    private GameObject _player2Des;
    private float _lastArriveTime = -1.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                // Debug.Log("Player1 reached");
                _player1Arr = true;
            }
            else
            {
                // Debug.Log("Player2 reached");
                _player2Arr = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                // Debug.Log("Player1 stayed");
                _player1Arr = true;
            }
            else
            {
                // Debug.Log("Player2 stayed");
                _player2Arr = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                // Debug.Log("Player1 leaved");
                _player1Arr = false;
            }
            else
            {
                //Debug.Log("Player2 leaved");
                _player2Arr = false;
            }
        }
    }

    private void Awake()
    {
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
        _player1Des.SetActive(_player1Arr);
        _player2Des.SetActive(_player2Arr);
        if (_player1Arr && _player2Arr)
        {
            if (_lastArriveTime < 0.0f) _lastArriveTime = Time.realtimeSinceStartup;
            if (Time.realtimeSinceStartup - _lastArriveTime < validateTime) return;

            var btns = FindObjectOfType<Btns>();
            var finalUIController = FindObjectOfType<FinalUIController>();
            if (finalUIController.GetPanelState() != FinalUIController.PanelState.Idle) return;

            if (btns && !btns.getStatus())
            {
                // Fail state
                var guide = FindObjectOfType<Guide>();
                if (guide)
                {
                    guide.mininumScorePanel();
                }

                if (finalUIController)
                {
                    finalUIController.SwitchState(FinalUIController.PanelState.Fail);
                }
            }
            else
            {
                // Success state
                if (finalUIController)
                {
                    finalUIController.SwitchState(FinalUIController.PanelState.Success);
                }

                DataManager.CompleteLevel();
            }
        }
        else
        {
            _lastArriveTime = -1.0f;
        }
    }
}
