using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This button changes the size of the other player
// to help the other player get through narrow tunnels
public class ButtonDefault : MonoBehaviour
{
    private GameObject target;
    private Animator _myAnimator;

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        _myAnimator.SetBool("isDown", true);

        if(other.CompareTag("Player")){
            getSmall(other);
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
        _myAnimator.SetBool("isDown", false);

        if(other.CompareTag("Player")){
            getLarge(other);
        }
    }
    void Update(){

    }

}
