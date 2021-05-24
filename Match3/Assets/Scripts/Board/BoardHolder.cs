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

    [Tooltip("Time that the game will be paused waiting for the next match in a chain of matches (in seconds)")] 
    public float timeBetweenMatches;
    
    [Tooltip("Number of rows to be offset in y position")] 
    public int rowsOffset;
    
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
        
        // adjust BoardHolder position 
        ResolvePosition();
        
        //populate board 
        PopulateBoard();
    }

    // Make board fit in screen size 
    private void ResolvePosition()
    {
        Camera mainCamera = Camera.main;
      
        float screenHeight = ScreenSize.GetScreenToWorldHeight(mainCamera);
        float screenWidth = ScreenSize.GetScreenToWorldWidth(mainCamera);
        Vector3 newBoardHolderPos = _transform.position;
        
        // Assumes that Camera pos and boardHolder pos in x :0  y:0
        newBoardHolderPos.y -= screenHeight * 0.5f;
        newBoardHolderPos.x -= screenWidth * 0.5f;
        
        // padding 
        newBoardHolderPos.y +=  0.5f * screenHeight/(rows + rowsOffset) ;
        newBoardHolderPos.x +=  0.5f * screenWidth/columns;
        
        // rows offset 
        newBoardHolderPos.y += rowsOffset * screenHeight/(rows + rowsOffset);
        _transform.position = newBoardHolderPos;
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
            Vector2 spriteSize = Vector2.zero;
            for (int j = 0; j < _board.rows; j++)
            {
                // discover gem sprite (-1 because the pieces vary in range 1..gemsSprites.Count )
                Sprite gemSprite = gemsSprites[_board.BoardPieces[j, i] - 1];
                Vector2 pos = _transform.position;
                pos.x += xDistance;
                pos.y += yDistance;
                GameObject newGem = Instantiate(gemPrefab, pos, _transform.rotation);
                newGem.GetComponent<SpriteScaler>().ScaleSprite(rows,columns,rowsOffset);
                newGem.GetComponent<SpriteRenderer>().sprite = gemSprite;
                GemComponent newGemComponent = newGem.GetComponent<GemComponent>();
                spriteSize = newGem.GetComponent<SpriteRenderer>().bounds.size;
                newGemComponent.BoardHolder = this;
                newGemComponent.positionInBoard = new Point(j, i);
                gemComponents[j, i] = newGemComponent;
                yDistance += spriteSize.y;
            }
    
            xDistance += spriteSize.x;
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
        Vector3 originalGemSelectedPosTemp = gemSelected.originalPos;
        gemSelected.originalPos = otherGem.originalPos;
        otherGem.originalPos = originalGemSelectedPosTemp;
        
        // update Gems 
        GemComponent tempGem = Gems[gemSelected.positionInBoard.row, gemSelected.positionInBoard.col];
        Gems[gemSelected.positionInBoard.row, gemSelected.positionInBoard.col] = otherGem;
        Gems[otherGem.positionInBoard.row, otherGem.positionInBoard.col] = tempGem;
        
        // change position in board 
        Point positionInBoardTemp = gemSelected.positionInBoard;
        gemSelected.positionInBoard = otherGem.positionInBoard;
        otherGem.positionInBoard = positionInBoardTemp;
        
        // change position 
        StartCoroutine(otherGem.ReturnToOriginalPos());
        gemSelected.ChangeSelected(false);
    }
    
    public IEnumerator CascadeEffect(List<Point> points)
    {
        // wait piece to move to original position
        yield return new WaitForSeconds(0.5f);
        
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
                
                Gems[point.row, point.col].sp.enabled = false;
                Gems[point.row, point.col].CircleCollider.enabled = false;
                Gems[point.row, point.col].dragSpeed = 100f; // in order to rearrange fast invisible gems 

            }
        
            // replace the match values 
            int above = lowerRow + points.Count;
            int currentRow = lowerRow;
            foreach (Point point in points)
            {
                if (above < rows)
                {
                    SwapGems(Gems[currentRow, point.col],Gems[above, point.col] );
                }

                above += 1;
                currentRow++;
            }
        
            int row;
            // all points in match have the same col in this case 
            int commonColumn = points[0].col;
            
            // update the model for the cascade effect 
            if (higherRow < rows - 1)
            {
                for (row = higherRow + 1; row < rows; row++)
                {
                    if (row + points.Count <= rows - 1)
                    {
                        SwapGems(Gems[row, commonColumn],Gems[row+points.Count, commonColumn] );
                    }
                   
                }
            }
  
        }
        else if (points[0].row == points[1].row)
        {
            // row match
            foreach (Point point in points)
            { 
                // empty space
                int row;
                Gems[point.row, point.col].sp.enabled = false;
                Gems[point.row, point.col].CircleCollider.enabled = false;
                
                for (row = point.row; row < rows - 1; row++)
                {
                    SwapGems(Gems[row, point.col],Gems[row + 1, point.col]);
                }
            }
        }
        
        // update Model
        List<Point> newPoints = _board.ErasePieces(points);

        yield return new WaitForSeconds(0.5f);
        
        // position y of the highest row 
        float yDistance = Gems[rows-1,columns-1].originalPos.y;

        // replace the erased pieces with new sprites based on model 
        foreach (Point newPoint in newPoints)
        {
            Sprite gemSprite = gemsSprites[_board.BoardPieces[newPoint.row, newPoint.col] - 1];
            GemComponent newGem = Gems[newPoint.row, newPoint.col];
                        
            // update y distance 
            yDistance += newGem.sp.bounds.size.y;
            Vector3 posUp = newGem.originalPos;
            posUp.y += yDistance;
            newGem.transform.position = posUp;
            
            // enable gem into the game 
            newGem.sp.enabled = true;
            newGem.sp.sprite = gemSprite;
            newGem.CircleCollider.enabled = true;
            newGem.dragSpeed = newGem.dragSpeedDeafaultValue; // Reset to default value 
            newGem.ChangeSelected(false);

        }
        
        yield return new WaitForSeconds(timeBetweenMatches);
        EndRound();
    }

    // method to know if other match3 happend 
    private void EndRound()
    {
        List<Point> possiblePoints = _board.CheckMatch3();
        if (possiblePoints != null)
        {
            StartCoroutine(CascadeEffect(possiblePoints));
        }
        else
        {
            // check if the game has possible moves 
            if (_board.CheckImpossibleBoard())
            {
                // create another board and change sprites 
                _board = new Board(gemsSprites.Count, rows, columns);
                PopulateGemsSprites();
            }
            // unpause the game 
            paused = false;
        }
    }

    private void PopulateGemsSprites()
    {
        for (int i = 0; i < _board.cols; i++)
        {
            // Y distance between sprites 
            for (int j = 0; j < _board.rows; j++)
            {
                // discover gem sprite (-1 because the pieces vary in range 1..gemsSprites.Count )
                Sprite gemSprite = gemsSprites[_board.BoardPieces[j, i] - 1];
                Gems[j, i].sp.sprite = gemSprite;
            }
            
        }
    }
    

}
