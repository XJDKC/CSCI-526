using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Transfroms of the two players, assign these in the Inspector
    public Transform player1Transform;
    public Transform player2Transform;

    // Transform of the background, assign this in the Inspector
    // this is used to restrict the location of the camera
    public GameObject backgroundObj;

    // Interface: Control the zoom ratio of the Camera, the larger the value, the boarder the view is.
    // [Range(1, 2)]
    // public float camZoomRatio = 1.0f;

    // Default size of the camera, should be set in the Inspector
    // by definition, in orthographic mode, the camera size is half-size of screen's height
    public float defaultCamSize;

    // A flag to change the defaultCamSize, default value is false
    // If you want to trigger the camera to zoom in/out by your designated size, set it to true to zoom;
    // it will be set back to false when new size is applied
    public bool defaultCamSizeTrigger;

    private Camera cam;
    private Transform camTransform;

    // The ratio of the screen, which is used to calculate the camera's width
    private float aspectRatio;

    private float cameraHeight;
    private float cameraWidth;

    // Threshold Ratio from x and y axises. if either of them reached, the camera start zooming
    private float zoomThresholdRatioX = 0.67f;
    private float zoomThresholdRatioY = 0.8f;

    // background's width, height and position
    private float bgWidth;
    private float bgHeight;
    private Vector3 bgPos;

    // the x, y bounds of background, to confine the camera movement
    private float bgLeftX;
    private float bgRightX;
    private float bgUpperY;
    private float bgLowerY;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();
        cam = GetComponent<Camera>();

        // get the aspect ration from Screen
        aspectRatio = Screen.width * 1.0f / Screen.height;
        cameraHeight = defaultCamSize * 2;
        cameraWidth = cameraHeight * aspectRatio;

        defaultCamSizeTrigger = false;

        // initialize the camera attributes
        if (cam)
        {
            // enable the camera
            cam.enabled = true;

            //enables the orthographic mode and set its initial size
            cam.orthographic = true;
            cam.orthographicSize = defaultCamSize;

        }

        // initialize the border of background, for confining camera
        if (backgroundObj)
        {
            bgPos = backgroundObj.GetComponent<Transform>().position;
            bgWidth = backgroundObj.GetComponent<Renderer>().bounds.size.x;
            bgHeight = backgroundObj.GetComponent<Renderer>().bounds.size.y;

            bgLeftX = bgPos.x - (bgWidth / 2);
            bgRightX = bgPos.x + (bgWidth / 2);


            bgUpperY = bgPos.y + (bgHeight / 2);
            bgLowerY = bgPos.y - (bgHeight / 2);
        }



    }

    void LateUpdate()
    {

        if (cam && player1Transform && player2Transform)
        {
            if (defaultCamSizeTrigger)
            {
                cam.orthographicSize = defaultCamSize;
                defaultCamSizeTrigger = false;
                return;
            }

            // 1. update the camera size;
            // get current size of the camera
            float curCamSize = cam.orthographicSize;
            cameraHeight = curCamSize * 2;
            cameraWidth = cameraHeight * aspectRatio;

            // get camera and two players's positions
            Vector3 camPos = camTransform.position;
            Vector3 playerPos1 = player1Transform.position;
            Vector3 playerPos2 = player2Transform.position;

            // calculate the ratio of x-distance and y-distance
            // if the ratio of the distance between two players takes zoomThresholdRatio, start zooming
            float nextCamSize = curCamSize;
            float xDistance = Math.Abs(playerPos1.x - playerPos2.x);
            float yDistance = Math.Abs(playerPos1.y - playerPos2.y);

            float distanceRatioX = xDistance / cameraWidth;
            float distanceRatioY = yDistance / cameraHeight;
            if (Math.Abs(distanceRatioX - zoomThresholdRatioX) < 0.01f)
            {
                float nextCamWidth = xDistance / zoomThresholdRatioX;
                float nextCamHeight = nextCamWidth / aspectRatio;
                nextCamSize = nextCamHeight / 2;
            }

            if (Math.Abs(distanceRatioY - zoomThresholdRatioY) < 0.1f)
            {
                float nextCamHeight = yDistance / zoomThresholdRatioY;
                nextCamSize = nextCamHeight / 2;
            }

            // camera size could not be smaller than the default size;
            if (nextCamSize < defaultCamSize)
            {
                nextCamSize = defaultCamSize;
            }

            // cam.orthographicSize = nextCamSize * camZoomRatio;
            cam.orthographicSize = nextCamSize;

            // 2. update the position of camera, (within background if assigned)
            float mid_x = (playerPos1.x + playerPos2.x) / 2;
            float mid_y = (playerPos1.y + playerPos2.y) / 2;


            // camera position could not leave the following bounded area
            if (backgroundObj)
            {
                if (mid_x < bgLeftX + cameraWidth / 2)
                {
                    mid_x = bgLeftX + cameraWidth / 2;
                }

                if (mid_x > bgRightX - cameraWidth / 2)
                {
                    mid_x = bgRightX - cameraWidth / 2;
                }

                // for future use, if y-axis is needed
                if (mid_y > bgUpperY - cameraHeight / 2)
                {
                    mid_y = bgUpperY - cameraHeight / 2;
                }

                if (mid_y < bgLowerY + cameraHeight / 2)
                {
                    mid_y = bgLowerY + cameraHeight / 2;
                }

            }
            camTransform.position = new Vector3(mid_x, mid_y, camPos.z);
        }


    }


}
