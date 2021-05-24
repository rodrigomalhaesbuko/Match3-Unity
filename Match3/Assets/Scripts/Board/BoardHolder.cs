using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardHolder : MonoBehaviour
{
    public List<Sprite> gemsSprites;
    public GameObject gemPrefab;
    public int rows;
    public int columns;
    

    public float yOffset;
    public float xOffset;
    
    [HideInInspector]
    public GemComponent[,] Gems;

    [HideInInspector] public bool paused; 

    private Board _board;
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        // create board 
        _board = new Board(gemsSprites.Count, rows, columns);
        _transform = transform;
        
        //populate board 
        PopulateBoard();
    }

    // Populate board with visual gems 
    private void PopulateBoard()
    {   // X distance between sprites 
        GemComponent[,] gemComponents = new GemComponent[_board.rows,_board.cols];
        float xDistance = 0f;
        
        for (int i = 0; i < _board.cols; i++)
        {
            // Y distance between sprites 
            float yDistance = 0f;
            for (int j = 0; j < _board.rows; j++)
            {
                // discover gem sprite (-1 because the pieces vary in range 1..gemsSprites.Count )
                Sprite gemSprite = gemsSprites[_board.BoardPieces[j, i] - 1];
                Vector2 pos = _transform.position;
                pos.x += xDistance;
                pos.y += yDistance;
                GameObject newGem = Instantiate(gemPrefab, pos, _transform.rotation);
                newGem.GetComponent<SpriteRenderer>().sprite = gemSprite;
                GemComponent newGemComponent = newGem.GetComponent<GemComponent>();
                newGemComponent.BoardHolder = this;
                newGemComponent.positionInBoard = new Point(j, i);
                gemComponents[j, i] = newGemComponent;
                yDistance += yOffset;
            }
    
            xDistance += xOffset;
        }

        // receive reference of all gemComponents 
        Gems = gemComponents;
    }

    // Check if will be a Match3 and handle 
    public void CheckMatch3(GemComponent gemSelected, GemComponent otherGem)
    {
        List<Point> possiblePoints = _board.VerifySwap(gemSelected.positionInBoard, otherGem.positionInBoard);
        if (possiblePoints == null)
        {
            // return gem to original pos and deselect
            gemSelected.ChangeSelected(false);
            // make fail sound 
        }
        else
        {
            // happens a Match3 
            SwapGems(gemSelected,otherGem);
            // unable to play the game until all machs are resolved.
            paused = true;
            // Swap in  board model 
            _board.Swap(gemSelected.positionInBoard,otherGem.positionInBoard);
            // erase 
            StartCoroutine(CascadeEffect(possiblePoints));
        }
    }

    // Swap gems in the game 
    public void SwapGems(GemComponent gemSelected, GemComponent otherGem)
    {
        // get temporary position 
        Vector3 originalGemSelectedPosTemp = gemSelected._originalPos;
        gemSelected._originalPos = otherGem._originalPos;
        otherGem._originalPos = originalGemSelectedPosTemp;
        
        // update Gems 
        GemComponent tempGem = Gems[gemSelected.positionInBoard.row, gemSelected.positionInBoard.col];
        Gems[gemSelected.positionInBoard.row, gemSelected.positionInBoard.col] = otherGem;
        Gems[otherGem.positionInBoard.row, otherGem.positionInBoard.col] = tempGem;
        
        // change position 
        StartCoroutine(otherGem.ReturnToOriginalPos());
        gemSelected.ChangeSelected(false);
    }
    
    public IEnumerator CascadeEffect(List<Point> points)
    {
        yield return new WaitForSeconds(0.5f);
        // foreach (Point point in points)
        // {
        //     Gems[point.row, point.col].gameObject.GetComponent<SpriteRenderer>().enabled = false; 
        //     Gems[point.row, point.col].gameObject.GetComponent<CircleCollider2D>().enabled = false;
        //     
        //     Debug.Log("Points");
        //     Debug.Log(point.row);
        //     Debug.Log(point.col);
        // }
        if (points[0].col == points[1].col)
        {
            
            int higherRow = 0;
            int lowerRow = Int32.MaxValue;
            // column match    
            // get higher row and lower row of the match3 
            foreach (Point point in points)
            {
                if (point.row > higherRow)
                {
                    higherRow = point.row;
                }
        
                if (lowerRow > point.row)
                {
                    lowerRow = point.row;
                }
                
                Gems[point.row, point.col].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Gems[point.row, point.col].gameObject.GetComponent<CircleCollider2D>().enabled = false;
                
            }
        
            // replace the match values 
            int above = lowerRow + 3;
            int currentRow = lowerRow;
            foreach (Point point in points)
            {
                if (above < rows)
                {
                    // Gems[currentRow, point.col] = Gems[above, point.col];
                    //pointsToChange.Add(new PointsToChange(new Point(currentRow,point.col), new Point(above, point.col)));
                    SwapGems(Gems[currentRow, point.col],Gems[above, point.col] );
                }
                else
                {
                    //Gems[currentRow, point.col] = allPieces[Random.Range(0, allPieces.Count)];
                    //newPoints.Add(new Point(currentRow,point.col));
                }
        
                above += 1;
                currentRow++;
            }
        
            int row;
            // all points in match have the same col in this case 
            int communColumn = points[0].col;
            
            // update the model for the cascade effect 
            for (row = higherRow + 1; row < rows - 1; row++)
            {
                //BoardPieces[row, communColumn] = BoardPieces[row + 1, communColumn];
                SwapGems(Gems[currentRow, communColumn],Gems[above, communColumn] );
                //pointsToChange.Add(new PointsToChange(new Point(row,communColumn), new Point(row + 1, communColumn)));
            }
            
            //BoardPieces[row, communColumn] = allPieces[Random.Range(0, allPieces.Count)];
            //newPoints.Add(new Point(row,communColumn));
        }
        else if (points[0].row == points[1].row)
        {
            // row match
            foreach (Point point in points)
            { 
                // empty space
                int row;
                Gems[point.row, point.col].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                Gems[point.row, point.col].gameObject.GetComponent<CircleCollider2D>().enabled = false;
                for (row = point.row; row < rows - 1; row++)
                {
                    //BoardPieces[row, point.col] = BoardPieces[row + 1, point.col];
                    //pointsToChange.Add(new PointsToChange(new Point(row,point.col), new Point(row + 1, point.col)));
                    SwapGems(Gems[row, point.col],Gems[row + 1, point.col]);
                }
            
                // add in the last row an random piece 
                //BoardPieces[row, point.col] = allPieces[Random.Range(0, allPieces.Count)];
                //newPoints.Add(new Point(row,point.col));
            }
        }
    }

}
