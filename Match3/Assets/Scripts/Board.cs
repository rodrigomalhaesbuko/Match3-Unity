using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Aux struct to pass positional data 
public struct Point
{
    public int row;
    public int coll;

    public Point(int row, int coll)
    {
        this.row = row;
        this.coll = coll;
    }
}

public class Board
{
    public int[,] BoardPieces; // model of the game board 
    readonly int _numberOfPieces; // total number of diferent game pieces 
    public int rows;
    public int colls;

    public Board(int totalPieces, int rowsNumber, int collsNumber)
    {
        _numberOfPieces = totalPieces;
        rows = rowsNumber;
        colls = collsNumber;
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
        
        int[,] boardMatrix = new int[rows,colls];
        // variables to avoid starting with a match 
        int[] previousLeft = new int[rows];
        int previousBelow = 0;
        
        // all pieces representations 
        List<int> allPieces = Enumerable.Range(1, _numberOfPieces).ToList();
        
        //Create the model of the game board with integers 
        for (int i = 0; i < colls; i++)
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
       
        for (int i = 0; i < colls; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                // Can exclude the corners 
                if ((j == 0 && i == 0) || (j == rows -1 && i == colls-1) || (j == rows-1 && i == 0) || (j == 0 && i == colls-1))
                    continue;
                
                // Check horizontal match 3 
                if (i != 0 && i != colls - 1)
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
        // Its possible to avoid the boarder in this logic because the all boarder pices will be covered with the middle pieces 
        for (int i = 0; i < colls; i++)
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
                if(i != colls -1)
                    points.Add(new Point(j, i + 1));

                foreach (Point point in points)
                {
                    if (Swap(actualPoint, point) != null)
                    {
                        return false; 
                    }
                }
            }
        }

        return true; 
    }

    // Verify if swaping two points a Match3 occurs
    public List<Point> Swap(Point a, Point b)
    {
        int[,] originalBoard = BoardPieces;
        int aux = BoardPieces[a.row,a.coll];
        // SWAP
        BoardPieces[a.row, a.coll] = BoardPieces[b.row, b.coll];
        BoardPieces[b.row, b.coll] = aux;
        List<Point> possiblePoints = CheckMatch3();
        
        // SWAP AGAIN 
        int auxB = BoardPieces[a.row, a.coll];
        
        BoardPieces[a.row, a.coll] = aux;
        BoardPieces[b.row, b.coll] = auxB;

        // can be null or the Match3 points 
        return possiblePoints;
    }

    // Erase pieces in specific points in board and apply the cascade effect 
    public void ErasePieces(List<Point> pointsToErase)
    {
        foreach (Point point in pointsToErase)
        { 
            // empty space
            int row;
            for (row = point.row; row < rows - 1; row++)
            {
                BoardPieces[row, point.coll] = BoardPieces[row + 1, point.coll];
            }
            
            // add in the last row an random piece 
            List<int> allPieces = Enumerable.Range(1, _numberOfPieces).ToList();
            BoardPieces[row, point.coll] = allPieces[Random.Range(0, allPieces.Count)];;
        }
    }

    
}
