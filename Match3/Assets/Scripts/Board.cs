using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    
}
