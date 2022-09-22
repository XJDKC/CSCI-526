using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GravityChange : MonoBehaviour
{
    PlayerController pc;
    Vector3 yStandVect = new Vector3(0.0f, 1.0f, 0.0f);
    Vector3 xStandVect = new Vector3(1.0f, 0.0f, 0.0f);
    Dictionary<Collider2D, float> valueXDict = new Dictionary<Collider2D, float>();
    Dictionary<Collider2D, float> valueYDict = new Dictionary<Collider2D, float>();
    //Dictionary<Collider2D, float> collToVect = new Dictionary<Collider2D, float>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //get the corner coordinates of the door(the door must be rectangular)
            //you have to add rect transform component in the door object
            //RectTransform rt = GetComponent<RectTransform>();

            //Vector3[] corners = new Vector3[4];
            //rt.GetWorldCorners(corners);

            //Vector3 edge1 = (corners[0] + corners[1]) / 2;
            //Vector3 edge2 = (corners[2] + corners[3]) / 2;

            //Vector3 door = edge1 - edge2;

            pc = collision.gameObject.GetComponent<PlayerController>();

            Vector3 center = this.transform.position;
            Vector3 contactPoint = collision.bounds.center;

            //vector from door to character: character's center - door's center position
            Vector3 doorToChar = center - contactPoint;

            float comeX = Vector3.Cross(doorToChar, xStandVect).z;
            float comeY = Vector3.Cross(doorToChar, yStandVect).z;

            valueXDict.Add(collision, comeX);
            valueYDict.Add(collision, comeY);

            //float come = Vector3.Cross(doorToChar, door).z;
            //collToVect.Add(collision, come);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //RectTransform rt = GetComponent<RectTransform>();

            //Vector3[] corners = new Vector3[4];
            //rt.GetWorldCorners(corners);

            //Vector3 edge1 = (corners[0] + corners[1]) / 2;
            //Vector3 edge2 = (corners[2] + corners[3]) / 2;

            //Vector3 door = edge1 - edge2;


            pc = collision.gameObject.GetComponent<PlayerController>();

            Vector3 center = this.transform.position;
            Vector3 contactPoint = collision.bounds.center;

            //vector from door to character: character's center - door's center position
            //if cross value z is negative, character is from left(maybe)
            Vector3 doorToChar = center - contactPoint;
            float leaveX = Vector3.Cross(doorToChar, xStandVect).z;
            float leaveY = Vector3.Cross(doorToChar, yStandVect).z;

            //float leave = Vector3.Cross(doorToChar, door).z;

            if (valueXDict.ContainsKey(collision) && valueYDict.ContainsKey(collision))
            {
                float comeX = valueXDict[collision];
                float comeY = valueYDict[collision];
                //float come = collToVect[collision];

                //if (leave * come < 0)
                //{
                //    collision.gameObject.GetComponent<Rigidbody2D>().gravityScale *= -1;
                //    pc.changeReversed();
                //}

                //collToVect.Remove(collision);


                //come and leave in different position
                if (leaveY * comeY < 0 || leaveX * comeX < 0)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().gravityScale *= -1;
                    pc.Reverse();

                }

                valueXDict.Remove(collision);
                valueYDict.Remove(collision);
            }
        }
    }
}
