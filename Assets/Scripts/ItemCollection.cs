using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("WhiteTag"))
        {    Debug.Log(col.gameObject.transform.parent);
            // Destroy(col.gameObject);
            Destroy(col.gameObject);
            Destroy(GameObject.Find(col.gameObject.transform.parent.name));
        }
    }
    
}
