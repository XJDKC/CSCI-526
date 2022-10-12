using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRender : MonoBehaviour
{
    public int circleSize = 85;
    public int circleAngle = 295;

    SpriteRenderer spriteRender;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        if (spriteRender == null)
            spriteRender = gameObject.AddComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // if (Input.GetMouseButtonDown(0))
            spriteRender.sprite = CreateSprite();
            transform.Rotate(Vector3.forward, Time.deltaTime * 60);
    }

    /// <summary>
    /// Create Ring，generate Sprite
    /// </summary>
    Sprite CreateSprite()
    {
        // color of empty spaces
        Color emptyColor = new Color(0, 0, 0, 0);
        // color in the circle
        Color circleColor = new Color(1, 0, 0, 1.0f);

        int innerRadius = circleSize;
        int outterRadius = innerRadius + 10;

        //扇形角度
        float halfAngle = circleAngle / 2f;

        //image size
        int spriteSize = outterRadius * 2;

        //exture2D
        Texture2D texture2D = new Texture2D(spriteSize, spriteSize);

        // center position of the image
        Vector2 centerPixel = new Vector2(spriteSize / 2, spriteSize / 2);

        // Create a shape group and add a Circle to it.
        var shapeGroup1 = new PhysicsShapeGroup2D();
        var collidorPoints = new List<Vector2>();

        //
        Vector2 tempPixel;
        float tempAngle, tempDisSqr;

        //traverse the whole map, check which pixel can be in the ring
        for (int x = 0; x < spriteSize; x++)
        {
            for (int y = 0; y < spriteSize; y++)
            {

                tempPixel.x = x - centerPixel.x;
                tempPixel.y = y - centerPixel.y;

                //within radius
                tempDisSqr = tempPixel.sqrMagnitude;
                // if (Math.Abs(tempDisSqr - maxRadius) < 0.1f || Math.Abs(tempDisSqr - minRadius) < 0.1f)
                // {
                //
                //     collidorPoints.Add(new Vector2(x, y));
                //     Debug.Log("in the ring");
                //     // shapeGroup1.AddCircle
                //     // (
                //         // center: tempPixel,
                //         // radius: 1f
                //     // );
                // }
                if (tempDisSqr >= innerRadius * innerRadius && tempDisSqr <= outterRadius * outterRadius)
                {
                    // within the specific angle
                    tempAngle = Vector2.Angle(Vector2.up, tempPixel);
                    if (tempAngle < halfAngle || tempAngle > 360 - halfAngle)
                    {
                        //set color to the ring
                        texture2D.SetPixel(x, y, circleColor);
                        continue;
                    }
                }
                // Debug.Log(collidorPoints);

                texture2D.SetPixel(x, y, emptyColor);
            }
        }

        tempAngle = 360 - circleAngle;
        while (tempAngle <= circleAngle)
        {
            var x = centerPixel.x + outterRadius * Mathf.Cos(tempAngle);
            var y = centerPixel.y + outterRadius * Mathf.Sin(tempAngle);
            collidorPoints.Add(new Vector2(x, y));
            Debug.Log(Mathf.Cos(tempAngle) + ", " + Mathf.Cos(tempAngle));

            if (Math.Abs(tempAngle - circleAngle) < 0.1f) break;
            tempAngle += 10;
            if (tempAngle > circleAngle)
            {
                tempAngle = circleAngle;
            }
        }

        // _polygonCollider2D.points = collidorPoints.ToArray();

        texture2D.Apply();

        //Create Sprite
        return Sprite.Create(texture2D, new Rect(0, 0, spriteSize, spriteSize), new Vector2(0.5f, 0.5f));
    }
}

