using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        int[,] boardMatrix = new int[rows,colls];
        // variables to avoid starting with a match 
        int[] previousLeft = new int[10000];
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
    
}
