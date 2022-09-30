using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//usage: create GenerateStars object gs and call gs.Crash(GameObject enemy)
public class GenerateStars : MonoBehaviour
{
    private int fragmentNum = 8;

    //explode force
    private float forceMultiply = 10F;
    //disappear delay time
    //private float delaySecond = 5F;

    private Vector2 originalPosition;

    //fragments object list
    private List<GameObject> stars;

    public void Crash(GameObject gameObject)
    {
        stars = new List<GameObject>();
        //Debug.Log(sprite);
        float centerX = gameObject.transform.position.x - gameObject.transform.localScale.x / 2;
        float centerY = gameObject.transform.position.y - gameObject.transform.localScale.y / 2;
        originalPosition = new Vector2(centerX, centerY);
        //get all star objects
        GetStars();
        Debug.Log("---Stars Num: " + stars.Count);
        //ejact stars
        for (int i = 0; i < stars.Count; i++)
            Ejection(stars[i]);
    }

    //Generate fragments
    private void GetStars()
    {
        //Debug.Log("---Generating Stars---");
        //TODO: hard code position, need further improve
        float[] shift = new float[] { -0.8F, -0.6F, -0.3F, -0.1F, 0.1F, 0.3F, 0.5F, 0.7F};
        float centerX = gameObject.transform.position.x - gameObject.transform.localScale.x / 2;
        float centerY = gameObject.transform.position.y - gameObject.transform.localScale.y / 2;
        for (int i = 0; i < fragmentNum; i++)
        {
            Vector2 position = new Vector2(centerX + shift[i], centerY + 1F);
            stars.Add(CreateStar(position));
        }
    }

    //eject a star object
    private void Ejection(GameObject fragment)
    {
        Vector2 start = originalPosition;
        Vector2 end = fragment.transform.position;
        Vector2 direction = end - start;
        fragment.GetComponent<Rigidbody2D>().AddForce(direction * forceMultiply / 2, ForceMode2D.Impulse);
        fragment.GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceMultiply / 5, ForceMode2D.Impulse);
        //Debug.Log("---Ejaction Completed---");
        //fragment.GetComponent<Rigidbody2D>().AddForce(transform.down * forceMultiply, ForceMode2D.Impulse);
    }

    //Create a star object
    private GameObject CreateStar(Vector2 position)
    {
        //Debug.Log("---Creating Stars---");
        GameObject star = Instantiate(Resources.Load("Star", typeof(GameObject))) as GameObject;
        //fragment.layer = LayerMask.NameToLayer("Default");
        star.transform.position = position;
        return star;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start crashing");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
