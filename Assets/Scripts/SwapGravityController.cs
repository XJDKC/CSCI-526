using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapGravityController : MonoBehaviour
{
    public enum PortalMode { BothPlayers, FirstPlayer, SecondPlayer }

    public PortalMode portalMode = PortalMode.BothPlayers;
    public Sprite bothSwapIcon;
    public Sprite firstPlayerSwapIcon;
    public Sprite secondSwapIcon;
    private GameObject _playerObject1;

    private GameObject _playerObject2;
    private GameObject _ban;

    private

        // Start is called before the first frame update
        void Start()
    {
        _ban = transform.GetChild(0).gameObject;
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
                GetComponent<SpriteRenderer>().sprite = bothSwapIcon;
                break;
            case PortalMode.FirstPlayer:
                GetComponent<SpriteRenderer>().sprite = firstPlayerSwapIcon;
                break;
            case PortalMode.SecondPlayer:
                GetComponent<SpriteRenderer>().sprite = secondSwapIcon;
                break;
        }
    }

    private void Update()
    {
        var isBaned = _playerObject1.GetComponent<Rigidbody2D>().gravityScale
            .Equals(_playerObject2.GetComponent<Rigidbody2D>().gravityScale);
        _ban.SetActive(isBaned);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col is CapsuleCollider2D)
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
