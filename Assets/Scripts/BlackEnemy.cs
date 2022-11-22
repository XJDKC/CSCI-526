using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackEnemy : MonoBehaviour
{
    private Animator _blackEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _blackEnemy = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D col)
    {
        //DataPost dp = GetComponent<DataPost>();
        if (col.gameObject.tag.Equals("Player"))
        {
            //data collect hook
            DataManager.GetDeathReason(gameObject);
            Die();
        }
        else if (_blackEnemy != null)
        {
            var isCollidedId = Animator.StringToHash("isCollided");
            _blackEnemy.SetBool(isCollidedId, true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (_blackEnemy != null)
        {
            var isCollidedId = Animator.StringToHash("isCollided");
            _blackEnemy.SetBool(isCollidedId, false);
        }
    }

    private void Die()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        DataManager.GetStartTime();
    }
}
