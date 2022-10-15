using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonDefault : MonoBehaviour
{
    //private BoxCollider2D button = GetComponents<BoxCollider2D>();
    private bool isOn;
    private GameObject target;
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Player is on");
            //EnemyController target =  GameObject.Find("Player2").GetComponent<EnemyController>();
            // target.enemyReverse = true;
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            var playerType = player.GetComponent<PlayerController>().playerType;
            var playerSelfType = other.GetComponent<PlayerController>().playerType;
            Debug.Log("curr playe is " + playerSelfType);
            Debug.Log("Other player is" + playerType);
            if (playerType!= playerSelfType){
                //player.GetComponent<PlayerController>().jumpSpeed = 12.0f;
                player.transform.localScale = new Vector3(0.5f,0.5f,1);
                }
            // if (playerType == PlayerController.PlayerType.Player1)
            // {
            //     foreach (var playerCollider in player.GetComponents<Collider2D>())
            //     {
            //         Physics2D.IgnoreCollision(playerCollider, _boxCollider2D);
            //     }
            // }
        }
        }
    }
}
