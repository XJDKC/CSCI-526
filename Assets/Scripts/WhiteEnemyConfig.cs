using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteEnemyConfig : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        { Debug.Log("aaa"+this.transform.parent.name);
            Destroy(GameObject.Find(this.transform.parent.name));
        }
    }
}
