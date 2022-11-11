using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGravityController : MonoBehaviour
{
    public enum PortalMode { BothPlayers, FirstPlayer, SecondPlayer }

    public PortalMode portalMode = PortalMode.BothPlayers;
    public Color normalColor = Color.white;
    public Color firstPlayerColor = new Color(92, 200, 231);
    public Color secondPlayerColor = new Color(234, 113, 189);
    private GameObject _playerObject1;

    private GameObject _playerObject2;

    private

        // Start is called before the first frame update
        void Start()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player1)
            {
                _playerObject1 = player;
            }

            if (player.GetComponent<PlayerController>().playerType == PlayerController.PlayerType.Player2)
            {
                _playerObject2 = player;
            }
        }

        switch (portalMode)
        {
            case PortalMode.BothPlayers:
                GetComponent<SpriteRenderer>().color = normalColor;
                break;
            case PortalMode.FirstPlayer:
                GetComponent<SpriteRenderer>().color = firstPlayerColor;
                break;
            case PortalMode.SecondPlayer:
                GetComponent<SpriteRenderer>().color = secondPlayerColor;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.collider is CapsuleCollider2D)
        {
            if (portalMode == PortalMode.BothPlayers)
            {
                SwitchGravity(_playerObject1, _playerObject2);
            }
            else if (col.gameObject.GetComponent<PlayerController>().playerType ==
                     PlayerController.PlayerType.Player1 && portalMode == PortalMode.FirstPlayer)
            {
                SwitchGravity(_playerObject1, _playerObject2);
            }
            else if (col.gameObject.GetComponent<PlayerController>().playerType ==
                     PlayerController.PlayerType.Player2 && portalMode == PortalMode.SecondPlayer)
            {
                SwitchGravity(_playerObject1, _playerObject2);
            }
        }
    }

    void SwitchGravity(GameObject player1, GameObject player2)
    {
        if (!player1.GetComponent<Rigidbody2D>().gravityScale.Equals(player2.GetComponent<Rigidbody2D>().gravityScale))
        {
            player1.GetComponent<PlayerController>().Reverse();
            player2.GetComponent<PlayerController>().Reverse();
        }
    }
}
