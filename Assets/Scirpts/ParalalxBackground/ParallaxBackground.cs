using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera maincamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;
    
    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        maincamera = Camera.main;  //assign a variable to follow the movement of the camera
        cameraHalfWidth = maincamera.orthographicSize * maincamera.aspect;
        CalculateBackgroundWidth();
    }

    private void FixedUpdate()
    {
        float curCameraPositionX = maincamera.transform.position.x;  //assign a variable to get the position that the camera at each frame 
        float distanceMove = curCameraPositionX - lastCameraPositionX;   //distance of the position of the camera last frame vs current frame
        lastCameraPositionX = curCameraPositionX;  //update the last frame position to the current frame position 

        float cameraRightEdge = curCameraPositionX + cameraHalfWidth;
        float cameraLeftEdge = curCameraPositionX - cameraHalfWidth;
        
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceMove);
            layer.loopBackGround(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void CalculateBackgroundWidth()
    {
        foreach (ParallaxLayer layer in backgroundLayers) layer.CalculateImageWidth();
    }
}
