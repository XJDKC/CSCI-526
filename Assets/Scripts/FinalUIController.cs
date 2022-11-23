using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalUIController : MonoBehaviour
{
    public enum PanelState { Idle, Success, Fail }

    public GameObject successDialog;
    public GameObject failDialog;

    private PanelState _prevPanelState = PanelState.Success;
    private PanelState _currPanelState = PanelState.Idle;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void Update()
    {
        if (_prevPanelState == _currPanelState) return;

        switch (_currPanelState)
        {
            case PanelState.Idle:
                successDialog.SetActive(false);
                failDialog.SetActive(false);
                Time.timeScale = 1;
                break;
            case PanelState.Success:
                successDialog.SetActive(true);
                failDialog.SetActive(false);
                Time.timeScale = 0;
                break;
            case PanelState.Fail:
                failDialog.SetActive(true);
                successDialog.SetActive(false);
                Time.timeScale = 0;
                break;
        }

        _prevPanelState = _currPanelState;
    }

    // Move to the next level button controller
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Restart level controller
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // data manage hook
        DataManager.RestartLevel();
        DataManager.GetStartTime();
    }

    // Return to main
    public void ReturnMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SwitchState(PanelState panelState)
    {
        _currPanelState = panelState;
    }

    public PanelState GetPanelState()
    {
        return _currPanelState;
    }
}
