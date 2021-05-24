using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to retrieve actual screen size 
public class ScreenSize
{
    // get screen Height
    public static float GetScreenToWorldHeight(Camera mainCamera)
    {
        Vector2 topRightCorner = new Vector2(1, 1); 
        Vector2 edgeVector = mainCamera.ViewportToWorldPoint(topRightCorner);            
        float height = edgeVector.y * 2;
        return height;
    }   
    
    // get screen Width
    public static float GetScreenToWorldWidth(Camera mainCamera)
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = mainCamera.ViewportToWorldPoint(topRightCorner);            
        float width = edgeVector.x * 2;
        return width;
    }
}
