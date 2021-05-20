using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using NUnit.Framework;
public class BoardTests 
{
   [Test]
   public void CreateBoard_Test()
   {
      // VERIFY IF STARTS WITH A MATCH3 
      Board board = new Board(3, 10, 5);
      PrintBoard(board);
      
      // VERIFY BOUNDS OF BOARD 
      Assert.That(board.rows, Is.EqualTo(10));
      Assert.That(board.colls, Is.EqualTo(5));
      
      // verify if its possible to create the board 
      Board brokenBoard = new Board(2, 10, 5);
      Assert.That(brokenBoard.BoardPieces, Is.EqualTo(null));
      
      // verify that is impossible to create impossible game board 
      Board notImpossibleBoard = new Board(4, 100, 500);
      Assert.That(notImpossibleBoard.CheckImpossibleBoard, Is.EqualTo(false));
      
   }

   [Test]
   public void CheckMatch3_Test()
   {
      Board board = new Board(3, 4, 4);
      PrintBoard(board);
      // Dont match3 test 
      Assert.That(board.CheckMatch3(), Is.EqualTo(null));
      
      // Horizontal test 
      int[,] testA =  { { 1, 1, 1 }, { 1, 4, 2 }, { 5, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = testA;
      board.colls = 3;
      board.rows = 4;
      List<Point> points = new List<Point>();
      points.Add(new Point(0,0));
      points.Add(new Point(0,1));
      points.Add(new Point(0,2));
      // PrintBoard(board); // debug option to show the board 
      Assert.That(board.CheckMatch3(), Is.EqualTo(points));
      
      // Vertical test
      int[,] testB = { { 1, 2, 1 }, { 1, 4, 2 }, { 1, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = testB;
      board.colls = 3;
      board.rows = 4;
      List<Point> pointsB = new List<Point>();
      pointsB.Add(new Point(0,0));
      pointsB.Add(new Point(1,0));
      pointsB.Add(new Point(2,0));
      // PrintBoard(board); // debug option to show the board 
      Assert.That(board.CheckMatch3(), Is.EqualTo(pointsB));
   }

   [Test]
   public void CheckImpossibleBoard_Test()
   {
      // check if the board created is always possible to play
      Board board = new Board(3, 10, 5);
      
      Assert.That(board.CheckImpossibleBoard, Is.EqualTo(false));
      
      // Impossible board test
      int[,] testA = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }};
      board.BoardPieces = testA;
      board.colls = 3;
      board.rows = 3;
      
      Assert.That(board.CheckImpossibleBoard, Is.EqualTo(true));
      
      // Impossible board test
      int[,] testB = { { 2, 1, 1 }, { 1, 4, 2 }, { 4, 6, 9 } };
      board.BoardPieces = testB;
      board.colls = 3;
      board.rows = 3;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(board.CheckImpossibleBoard, Is.EqualTo(false));
      
      // Swap horizontal 
      Board boardC = new Board(3, 4, 4);
      int[,] testC = { { 1, 2, 1, 1 }, { 1, 4, 2 , 9}, { 4, 6, 13, 5 }, { 7, 8, 9, 5 } };
      boardC.BoardPieces = testC;
      boardC.colls = 4;
      boardC.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(boardC.CheckImpossibleBoard, Is.EqualTo(false));
      
   }

   [Test]
   public void Swap_Test()
   {
      // Swap vertical 
      Board board = new Board(3, 4, 4);
      int[,] test = { { 2, 1, 1 }, { 1, 4, 2 }, { 4, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = test;
      board.colls = 3;
      board.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(board.Swap(new Point(0,0),new Point(1,0)), Is.EqualTo(true));
      //PrintBoard(board); // debug option to show the board 
      
      // Swap horizontal 
      Board boardB = new Board(3, 4, 4);
      int[,] testB = { { 1, 2, 1, 1 }, { 1, 4, 2 , 9}, { 4, 6, 13, 5 }, { 7, 8, 9, 5 } };
      boardB.BoardPieces = testB;
      boardB.colls = 4;
      boardB.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(boardB.Swap(new Point(0,0),new Point(0,1)), Is.EqualTo(true));
      
   }
   

   // HELPER FUNCTION TO SHOW THE BOARD IN CONSOLE 
   private void PrintBoard(Board board)
   {
      for (int i = board.rows - 1; i >= 0; i--)
      {
         List<int> row = new List<int>() ;
         for (int j = 0; j < board.colls; j++)
         {
            row.Add(board.BoardPieces[i,j]);
         } 
         
         string message = "";
         foreach (var item in row)
         {
            message += item.ToString() + "";
         }
         Debug.Log(message);
      }
   }
}
