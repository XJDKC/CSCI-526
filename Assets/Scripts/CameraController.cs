using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnchorPoints
{
    public Transform upperPoint;
    public Transform rightPoint;
    public Transform lowerPoint;
    public Transform leftPoint;
}

public class CameraController : MonoBehaviour
{
    // Transfroms of the two players, assign these in the Inspector
    public Transform player1Transform;
    public Transform player2Transform;

    // Transform of the background, assign this in the Inspector
    // this is used to restrict the location of the camera

    // anchors to confine the camera;
    public AnchorPoints anchorPoints;


    // Interface: Control the zoom ratio of the Camera, the larger the value, the boarder the view is.
    [Range(1, 3)]
    public float targetZoomRatio = 1;


    // Default size of the camera, should be set in the Inspector
    // by definition, in orthographic mode, the camera size is half-size of screen's height
    private const float defaultCamSize = 6.7f;
    private float customCamSize;

    // Current zoom ratio of the camera
    private float camZoomRatio = 1.0f;



    // A flag to change the defaultCamSize, default value is false
    // If you want to trigger the camera to zoom in/out by your designated size, set it to true to zoom;
    // it will be set back to false when new size is applied
    private Camera cam;
    private Transform camTransform;

    // The ratio of the screen, which is used to calculate the camera's width
    private float aspectRatio;

    private float cameraHeight;
    private float cameraWidth;

    // Threshold Ratio from x and y axises. if either of them reached, the camera start zooming
    private float zoomThresholdRatioX = 0.8f;
    private float zoomThresholdRatioY = 0.8f;



    // the x, y bounds of background, to confine the camera movement
    private float anchorLeftX;
    private float anchorRightX;
    private float anchorUpperY;
    private float anchorLowerY;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = GetComponent<Transform>();
        cam = GetComponent<Camera>();

        // get the aspect ration from Screen
        aspectRatio = Screen.width * 1.0f / Screen.height;
        cameraHeight = defaultCamSize * 2;
        cameraWidth = cameraHeight * aspectRatio;
        customCamSize = defaultCamSize * camZoomRatio;

        targetZoomRatio = camZoomRatio;

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
        if (anchorPoints.leftPoint && anchorPoints.rightPoint && anchorPoints.upperPoint && anchorPoints.lowerPoint)
        {
            anchorLeftX = anchorPoints.leftPoint.position.x;
            anchorRightX = anchorPoints.rightPoint.position.x;
            anchorUpperY = anchorPoints.upperPoint.position.x;
            anchorLowerY = anchorPoints.lowerPoint.position.x;
        }
    }

    private void Update()
    {
        // Debug.Log ("player1Transform.position.x = " + player1Transform.position.x);
        camZoomRatio = Mathf.Lerp(camZoomRatio, targetZoomRatio, 2 * Time.deltaTime);
        customCamSize = defaultCamSize * camZoomRatio;
    }

    void LateUpdate()
    {

        if (cam && player1Transform && player2Transform)
        {

            // 1. update the camera size;
            // get current size of the camera
            float curCamSize = cam.orthographicSize;
            cameraHeight = curCamSize * 2;
            cameraWidth = cameraHeight * aspectRatio;
            float camDiagnose = (float)Math.Sqrt(cameraHeight * cameraHeight + cameraWidth * cameraWidth);
            float ratioHeight2Diagnose = cameraHeight / camDiagnose;

            // get camera and two players's positions
            Vector3 camPos = camTransform.position;
            Vector3 playerPos1 = player1Transform.position;
            Vector3 playerPos2 = player2Transform.position;

            // calculate the ratio of x-distance and y-distance
            // if the ratio of the distance between two players takes zoomThresholdRatio, start zooming
            float nextCamSize = customCamSize;
            float xDistance = Math.Abs(playerPos1.x - playerPos2.x);
            float yDistance = Math.Abs(playerPos1.y - playerPos2.y);
            float xyDistance = (playerPos1 - playerPos2).magnitude;


            float distanceRatioX = xDistance / cameraWidth;
            float distanceRatioY = yDistance / cameraHeight;
            float distanceRatioXY = xyDistance / camDiagnose;
            if (Math.Abs(distanceRatioX - zoomThresholdRatioX) < 0.1f)
            {
                float nextCamWidth = xDistance / zoomThresholdRatioX;
                float nextCamHeight = nextCamWidth / aspectRatio;
                nextCamSize = nextCamHeight / 2;
                // Debug.Log ("distanceRatioX,  zoomThresholdRatioX = " + distanceRatioX + ", " + zoomThresholdRatioX);

            }

            if (Math.Abs(distanceRatioY - zoomThresholdRatioY) < 0.1f)
            {
                float nextCamHeight = yDistance / zoomThresholdRatioY;
                nextCamSize = nextCamHeight / 2;
                // Debug.Log ("distanceRatioY,  zoomThresholdRatioY = " + distanceRatioY + ", " + zoomThresholdRatioY);
            }

            if (Math.Abs(distanceRatioXY - zoomThresholdRatioX) < 0.1f)
            {
                float nextCamDiagnose = xyDistance / zoomThresholdRatioX;
                nextCamSize = nextCamDiagnose * ratioHeight2Diagnose / 2;
                // Debug.Log ("distanceRatioY,  zoomThresholdRatioY = " + distanceRatioY + ", " + zoomThresholdRatioY);
            }

            // camera size could not be smaller than the default size;
            if (nextCamSize < customCamSize)
            {
                // Debug.Log ("Reverse back to default size");
                nextCamSize = customCamSize;
            }

            // cam.orthographicSize = nextCamSize * camZoomRatio;
            cam.orthographicSize = nextCamSize;

            // 2. update the position of camera, (within background if assigned)
            float mid_x = (playerPos1.x + playerPos2.x) / 2;
            float mid_y = (playerPos1.y + playerPos2.y) / 2;


            // camera position could not leave the following bounded area
            if (anchorPoints.leftPoint && anchorPoints.rightPoint && anchorPoints.upperPoint && anchorPoints.lowerPoint)

            {
                if (mid_x < anchorLeftX + cameraWidth / 2)
                {
                    mid_x = anchorLeftX + cameraWidth / 2;
                }

                if (mid_x > anchorRightX - cameraWidth / 2)
                {
                    mid_x = anchorRightX - cameraWidth / 2;

                }

                // for future use, if y-axis is needed

                // if (mid_y > bgUpperY - cameraHeight / 2)
                // {
                //     mid_y = bgUpperY - cameraHeight / 2;
                // }
                //
                // if (mid_y < bgLowerY + cameraHeight / 2)
                // {
                //     mid_y = bgLowerY + cameraHeight / 2;
                // }

            }
            camTransform.position = new Vector3(mid_x, mid_y, camPos.z);
        }
    }

    /**
     * @param: zoomRatio, recommended to be set between 1 to 2.5
     */
    public void setZoomRatio(float zoomRatio)
    {
        targetZoomRatio = zoomRatio;
        // Debug.Log ("Set Zoom Ratio");

    }


}
