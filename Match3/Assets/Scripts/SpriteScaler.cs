using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
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
}
