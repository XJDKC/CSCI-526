using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Collecting stars and updaing points
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") &&
            collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            StarUI.CurrentStarQuantity += 1;
            Debug.Log(StarUI.CurrentStarQuantity);
            Destroy(gameObject);
        }
    }
}
