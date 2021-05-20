using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHolder : MonoBehaviour
{
    public List<Sprite> gemsSprites;
    public GameObject gemPrefab;
    public int rows;
    public int columns;
    

    public float yOffset;
    public float xOffset;
    
    [HideInInspector]
    public GameObject[,] Gems;

    private Board _board;
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        // create board 
        _board = new Board(gemsSprites.Count, columns, rows);
        _transform = transform;
        
        //populate board 
        PopulateBoard();
    }

    // Populate board with visual gems 
    private void PopulateBoard()
    {   // X distance between sprites 
        float xDistance = 0f;
        
        for (int i = 0; i < _board.rows; i++)
        {
            // Y distance between sprites 
            float yDistance = 0f;
            for (int j = 0; j < _board.cols; j++)
            {
                // discover gem sprite (-1 because the pieces vary in range 1..gemsSprites.Count )
                Sprite gemSprite = gemsSprites[_board.BoardPieces[i, j] - 1];
                Vector2 pos = _transform.position;
                pos.x += xDistance;
                pos.y += yDistance;
                GameObject newGem = Instantiate(gemPrefab, pos, _transform.rotation);
                newGem.GetComponent<SpriteRenderer>().sprite = gemSprite;
                yDistance += yOffset;
            }
    
            xDistance += xOffset;
        }
    }
}
