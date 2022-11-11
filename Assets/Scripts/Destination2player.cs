using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destination2player : MonoBehaviour
{
    private bool player1Arr;
    private bool player2Arr;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                // Debug.Log("Player1 reached");
                player1Arr = true;
            }
            else
            {
                // Debug.Log("Player2 reached");
                player2Arr = true;
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
                player1Arr = true;
            }
            else
            {
                // Debug.Log("Player2 stayed");
                player2Arr = true;
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
                player1Arr = false;
            }
            else
            {
                //Debug.Log("Player2 leaved");
                player2Arr = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Arr && player2Arr)
        {
            var starUI = GameObject.FindObjectOfType<StarUI>();
            if (starUI && !starUI.getStatus())
            {
                var guide = GameObject.FindObjectOfType<Guide>();
                var guideMessage = GameObject.FindObjectOfType<GuideMessage>();
                if (guide && guideMessage)
                {
                    guide.mininumScorePanel();
                    guideMessage.minimumScoreText();
                }
            }
            else
            {
                SceneManager.LoadScene("Menu");
                DataManager.CompleteLevel();
            }
        }
    }
}
