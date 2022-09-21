using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Playerlife : MonoBehaviour
{
    [SerializeField]private Animator anim;
    private Rigidbody2D _rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("BlackTag"))
        {   Debug.Log("death");
            Die();
        }
    }

    private void Die()
    {
        BoxCollider2D boxCollider2D = new BoxCollider2D();
        _rigidbody2D.bodyType = RigidbodyType2D.Static; 
        anim.SetTrigger("death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
