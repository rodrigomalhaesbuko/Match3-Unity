using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

// Aux struct to pass positional data 
public struct Point
{
    public int row;
    public int col;

    public Point(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
}

public class Board
{
    public int[,] BoardPieces; // model of the game board 
    private readonly int _numberOfPieces; // total number of different game pieces 
    public int rows;
    public int cols;

    public Board(int totalPieces, int rowsNumber, int colsNumber)
    {
        _numberOfPieces = totalPieces;
        rows = rowsNumber;
        cols = colsNumber;
        CreateBoard();
    }

    void CreateBoard()
    {
        // check if its possible to create board 
        if(_numberOfPieces <= 2)
        {
            Debug.LogWarning("You cant create an board with only 2 pieces");
            BoardPieces = null;
            return;
        }
        
        int[,] boardMatrix = new int[rows,cols];
        // variables to avoid starting with a match 
        int[] previousLeft = new int[rows];
        int previousBelow = 0;
        
        // all pieces representations 
        List<int> allPieces = Enumerable.Range(1, _numberOfPieces).ToList();
        
        //Create the model of the game board with integers 
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // prevent match 3 
                List<int> possiblePieces = new List<int>();
                possiblePieces.AddRange(allPieces);
                possiblePieces.Remove(previousLeft[j]);
                possiblePieces.Remove(previousBelow);
                
                // discover piece 
                int piece = possiblePieces[Random.Range(0, possiblePieces.Count)];
                // add piece in board matrix 
                boardMatrix[j, i] = piece;
                previousLeft[j] = piece;
                previousBelow = piece;
            }
        }

        BoardPieces = boardMatrix;
        
        // Check if the board is impossible to play. If it is create another board. 
        if (CheckImpossibleBoard())
        {
            CreateBoard();
        }
    }

    // return the list of points of the FIRST match3 found. Return null if NO match3 was found 
    public List<Point> CheckMatch3()
    {
        List<Point> points = new List<Point>();
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // Can exclude the corners 
                if ((j == 0 && i == 0) || (j == rows -1 && i == cols-1) || (j == rows-1 && i == 0) || (j == 0 && i == cols-1))
                    continue;
                
                // Check horizontal match 3 
                if (i != 0 && i != cols - 1)
                {
                    int middlePiece = BoardPieces[j, i];
                    int leftPiece = BoardPieces[j, i - 1];
                    int rightPiece = BoardPieces[j, i + 1];
                    
                    if ( middlePiece == leftPiece && middlePiece == rightPiece)
                    {
                        // its a match 3 
                        points.Add(new Point(j,i-1));
                        points.Add(new Point(j,i));
                        points.Add(new Point(j,i + 1));
                        
                        // check for a bigger match in left 
                        for (int k = i-2; k >= 0; k--)
                        {
                            if(BoardPieces[j,k] == middlePiece)
                                points.Add(new Point(j,k));
                            else
                                break;
                        }
                        
                        // check for a bigger match in right 
                        for (int k = i+2; k < cols; k++)
                        {
                            if(BoardPieces[j,k] == middlePiece)
                                points.Add(new Point(j,k));
                            else
                                break;
                        }
                        return points;
                    }
                }
                    
                
                // Check vertical match 3 
                if (j != 0 && j != rows - 1)
                {
                    int middlePiece = BoardPieces[j, i];
                    int upPiece = BoardPieces[j + 1, i];
                    int downPiece = BoardPieces[j - 1, i];

                    if ( middlePiece == upPiece && middlePiece == downPiece)
                    {
                        // its a match 3 
                        points.Add(new Point(j - 1,i));
                        points.Add(new Point(j,i));
                        points.Add(new Point(j + 1,i));
                        // check for a bigger match in left 
                        for (int k = j-2; k >= 0; k--)
                        {
                            if (BoardPieces[k, i] == middlePiece)
                                points.Add(new Point(j,k));
                            else
                                break;
                            

                        }
                        
                        // check for a bigger match in right 
                        for (int k = j+2; k < rows; k++)
                        {
                            if(BoardPieces[k,i] == middlePiece)
                                points.Add(new Point(k,i));
                            else
                                break;
                        }
                        return points;
                    }
                }
            }
        }

        return null;

    }

    // Verify if there is an possible move for the player 
    public bool CheckImpossibleBoard()
    {
        // Its possible to avoid the boarder in this logic because the all boarder pieces will be covered with the middle pieces 
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Point actualPoint = new Point(j, i);
                List<Point> points = new List<Point>();
                // up
                if(j != rows -1)
                    points.Add(new Point(j + 1, i));
                //down
                if(j != 0)
                    points.Add(new Point(j - 1, i));
                //left
                if(i != 0 )
                    points.Add(new Point(j , i - 1));
                //right
                if(i != cols -1)
                    points.Add(new Point(j, i + 1));

                foreach (Point point in points)
                {
                    if (VerifySwap(actualPoint, point) != null)
                    {
                        return false; 
                    }
                }
            }
        }

        return true; 
    }

    // Verify if swapping two points a Match3 occurs
    public List<Point> VerifySwap(Point a, Point b)
    {
        int aux = BoardPieces[a.row,a.col];
        // SWAP
        BoardPieces[a.row, a.col] = BoardPieces[b.row, b.col];
        BoardPieces[b.row, b.col] = aux;
        List<Point> possiblePoints = CheckMatch3();
        
        // SWAP AGAIN 
        int auxB = BoardPieces[a.row, a.col];
        BoardPieces[a.row, a.col] = aux;
        BoardPieces[b.row, b.col] = auxB;

        // can be null or the Match3 points 
        return possiblePoints;
    }
    
    // Swap to pieces in the model board 
    public void Swap(Point a, Point b)
    {
        int aux = BoardPieces[a.row,a.col];
        // SWAP
        BoardPieces[a.row, a.col] = BoardPieces[b.row, b.col];
        BoardPieces[b.row, b.col] = aux;
    }

    // Erase pieces in specific points in board and apply the cascade effect 
    public List<Point> ErasePieces(List<Point> pointsToErase)
    {
        // only erase if is 3 or mre pieces to erase
        if(pointsToErase.Count < 3)
            return null;
        
        // position of the new pieces created 
        List<Point> newPoints = new List<Point>();
        
        // all pieces possibilities 
        List<int> allPieces = Enumerable.Range(1, _numberOfPieces).ToList();
        
        // discover if its a row match3 or a column match3
        if (pointsToErase[0].col == pointsToErase[1].col)
        {
            int higherRow = 0;
            int lowerRow = Int32.MaxValue;
            // column match    
            // get higher row and lower row of the match3 
            foreach (Point point in pointsToErase)
            {
                if (point.row > higherRow)
                {
                    higherRow = point.row;
                }

                if (lowerRow > point.row)
                {
                    lowerRow = point.row;
                }
            }

            // replace the match values 
            int above = lowerRow + pointsToErase.Count;
            int currentRow = lowerRow;
            foreach (Point point in pointsToErase)
            {
                if (above < rows)
                {
                    BoardPieces[currentRow, point.col] = BoardPieces[above, point.col];
                }
                else
                {
                    BoardPieces[currentRow, point.col] = allPieces[Random.Range(0, allPieces.Count - 1)];
                    newPoints.Add(new Point(currentRow,point.col));
                }

                above += 1;
                currentRow++;
            }

            // update pieces above match3 vertical 
            // all points in match have the same col in this case 
            int commonColumn = pointsToErase[0].col;
            
            if (higherRow < rows - 1) 
            {
                for (int row = higherRow + 1 ; row < rows; row++)
                {
                    if (row + pointsToErase.Count > rows - 1)
                    {
                        BoardPieces[row, commonColumn] = allPieces[Random.Range(0, allPieces.Count - 1)];
                        newPoints.Add(new Point(row,commonColumn));
                    }
                    else
                    {
                        BoardPieces[row, commonColumn] = BoardPieces[row + pointsToErase.Count , commonColumn];
                    }
                    
                }
            }
        }
        else if (pointsToErase[0].row == pointsToErase[1].row)
        {
            // row match
            foreach (Point point in pointsToErase)
            { 
                // empty space
                int row;
                for (row = point.row; row < rows - 1; row++)
                {
                    BoardPieces[row, point.col] = BoardPieces[row + 1, point.col];
                }
            
                // add in the last row an random piece 
                BoardPieces[row, point.col] = allPieces[Random.Range(0, allPieces.Count - 1)];
                newPoints.Add(new Point(row,point.col));
            }
        }

        return newPoints;

    }


}
