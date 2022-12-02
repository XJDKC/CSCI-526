using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public Object startPrefab = null;
    public int starNumber = 8;
    public float starInitialSpeed = 5;

    private static StarManager _instance;
    public static StarManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void InstanceStars(bool isReversed, GameObject blackEnemy)
    {
        float angle = Random.Range(0.0f, 360.0f);
        for (int i = 0; i < starNumber; i++)
        {
            Quaternion rotation = new Quaternion();
            angle += 90.0f / starNumber;
            rotation.eulerAngles = new Vector3(0.0f, 0.0f, angle);

            var star = Instantiate(startPrefab, blackEnemy.transform.position, rotation);
            star.GetComponent<Rigidbody2D>().gravityScale = isReversed ? -1.0f : 1.0f;
            star.GetComponent<Rigidbody2D>().velocity = new Vector2(0,
                isReversed ? -1.0f * starInitialSpeed : 1.0f * starInitialSpeed);
        }
    }
}
