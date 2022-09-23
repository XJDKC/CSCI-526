using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player1 || !player2) return;
        Vector3 pos = GetComponent<Transform>().position;
        Vector3 pos1 = player1.GetComponent<Transform>().position;
        Vector3 pos2 = player2.GetComponent<Transform>().position;
        float mid_x = (pos1.x + pos2.x) / 2;
        float mid_y = (pos1.y + pos2.y) / 2;
        GetComponent<Transform>().position = new Vector3(mid_x, mid_y, pos.z);
    }
}
