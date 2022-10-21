using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destination2player : MonoBehaviour
{
    private bool player1Arr=false;
    private bool player2Arr=false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(other.GetComponent<PlayerController>().playerType ==PlayerController.PlayerType.Player1){
                //Debug.Log("player1 reached");
                player1Arr = true;
            }else{
                //Debug.Log("Player2 reached");
                player2Arr = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(other.GetComponent<PlayerController>().playerType ==PlayerController.PlayerType.Player1){
                //Debug.Log("player1 reached");
                player1Arr = false;
            }else{
                //Debug.Log("Player2 reached");
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
        if(player1Arr&&player2Arr){
            if (!GameObject.Find("TextScore").GetComponent<StarUI>().getStatus())
            {
                GameObject.Find("PanelPrompt").GetComponent<Guide>().mininumScorePanel();
                GameObject.Find("TextPrompt").GetComponent<GuideMessage>().minimumScoreText();
            }
            else
            {
                SceneManager.LoadScene("Menu");
                DataManager.CompleteLevel();
            }
        }
    }
}
