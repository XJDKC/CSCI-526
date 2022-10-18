using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackEnemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D col)
    {
        //DataPost dp = GetComponent<DataPost>();
        if (col.gameObject.tag == "Player")
        {
            //data collect hook
            Debug.Log(gameObject);
            DataManager.Instance.GetDeathReason(gameObject);
            Die();
        }
    }

    private void Die()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
