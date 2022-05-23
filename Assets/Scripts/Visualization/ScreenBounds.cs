using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    static public ScreenBounds S;
    public Vector2 cachedScale { get; private set; }
    Camera cam;
    float cachedOrthographicSize, cachedAspect;

    public void AwakeCall()
    {
        Awake();
    }

    private void Awake()
    {
        S = this;
        cam = Camera.main;
        ScaleSelf();
        ScreenWrapping.screenSize = new System.Numerics.Vector2(cachedScale.x, cachedScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        ScaleSelf();
        ScreenWrapping.GetRundomScreenLoc();
    }

    private void ScaleSelf()
    {
        if (cam.orthographicSize != cachedOrthographicSize || cam.aspect != cachedAspect)
        {
            cachedScale = CalculateScale();
        }
    }


    private Vector2 CalculateScale()
    {
        cachedOrthographicSize = cam.orthographicSize;
        cachedAspect = cam.aspect;

        Vector2 scaleDesired;

        scaleDesired.y = cam.orthographicSize * 2;
        scaleDesired.x = scaleDesired.y * cam.aspect;

        return scaleDesired;
    }
}
