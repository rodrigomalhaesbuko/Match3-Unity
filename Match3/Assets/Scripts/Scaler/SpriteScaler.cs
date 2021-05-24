using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    [Tooltip("Scale background to fit the screen")] 
    public bool isBackground;

    private void Start()
    {
        if(isBackground)
            ScaleBackground();
    }

    public void ScaleSprite(int rows, int cols, int rowsOffset)
    {
        Camera mainCamera = Camera.main;
        float width = ScreenSize.GetScreenToWorldWidth(mainCamera);
        float height = ScreenSize.GetScreenToWorldHeight(mainCamera);
        
        Vector3 newScale = transform.localScale;
        newScale.x *= width / (cols) ;
        newScale.y *= height / (rows + rowsOffset);
        gameObject.GetComponent<GemComponent>()._originalLocalScale = newScale;
        transform.localScale = newScale;
    }

    public void ScaleBackground()
    {
        Camera mainCamera = Camera.main;
        float width = ScreenSize.GetScreenToWorldWidth(mainCamera);
        float height = ScreenSize.GetScreenToWorldHeight(mainCamera);
        
        Vector3 newScale = transform.localScale;
        newScale.x *= width / gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        newScale.y *= height / gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        transform.localScale = newScale;
    }
}
