using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonDefault : MonoBehaviour
{
    private bool isOn;
    private GameObject target;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Player is on");
            getSmall(other);
            isOn = true;
        }
        
    }
    private void getSmall(Collider2D other){
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var playerType = player.GetComponent<PlayerController>().playerType;
            var playerSelfType = other.GetComponent<PlayerController>().playerType;
            if (playerType!= playerSelfType){
                player.transform.localScale = new Vector3(0.5f,0.5f,1);
                return;               
                }
        }
        return;
    }

    private void getLarge(Collider2D other){
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var playerType = player.GetComponent<PlayerController>().playerType;
            var playerSelfType = other.GetComponent<PlayerController>().playerType;
            if (playerType!= playerSelfType){
                player.transform.localScale = new Vector3(1,1,1);
                return;               
                }
        }
        return;
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            getLarge(other);
            isOn = false;
        }
    }
    void Update(){
        Debug.Log("current state is " + isOn);

    }

}
