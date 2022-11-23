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

    private PanelState _panelState = PanelState.Idle;
    private int _sceneNo;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        _sceneNo = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void Update()
    {
        switch (_panelState)
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

    }

    // Move to the next level button controller
    public void NextLevel()
    {
        Debug.Log("Btn clicked");
        SceneManager.LoadScene(_sceneNo);
    }

    // Restart level controller
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //data manage hook
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
        _panelState = panelState;
    }
}
