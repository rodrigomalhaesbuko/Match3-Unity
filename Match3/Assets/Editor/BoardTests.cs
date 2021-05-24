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
      //PrintBoard(board);// debug option to show the board 
      
      // VERIFY BOUNDS OF BOARD 
      Assert.That(board.rows, Is.EqualTo(10));
      Assert.That(board.cols, Is.EqualTo(5));
      
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
      // PrintBoard(board); // debug option to show the board 
      // Dont match3 test 
      Assert.That(board.CheckMatch3(), Is.EqualTo(null));
      
      // Horizontal test 
      int[,] testA =  { { 1, 1, 1 }, { 1, 4, 2 }, { 5, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = testA;
      board.cols = 3;
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
      board.cols = 3;
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
      board.cols = 3;
      board.rows = 3;
      
      Assert.That(board.CheckImpossibleBoard, Is.EqualTo(true));
      
      // Impossible board test
      int[,] testB = { { 2, 1, 1 }, { 1, 4, 2 }, { 4, 6, 9 } };
      board.BoardPieces = testB;
      board.cols = 3;
      board.rows = 3;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(board.CheckImpossibleBoard, Is.EqualTo(false));
      
      // Swap horizontal 
      Board boardC = new Board(3, 4, 4);
      int[,] testC = { { 1, 2, 1, 1 }, { 1, 4, 2 , 9}, { 4, 6, 13, 5 }, { 7, 8, 9, 5 } };
      boardC.BoardPieces = testC;
      boardC.cols = 4;
      boardC.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(boardC.CheckImpossibleBoard, Is.EqualTo(false));
      
   }

   [Test]
   public void VerifySwap_Test()
   {
      // Swap vertical 
      Board board = new Board(3, 4, 4);
      int[,] test = { { 2, 1, 1 }, { 1, 4, 2 }, { 4, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = test;
      board.cols = 3;
      board.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(board.VerifySwap(new Point(0,0),new Point(1,0)) != null, Is.EqualTo(true));
      //PrintBoard(board); // debug option to show the board 
      
      // Swap horizontal 
      Board boardB = new Board(3, 4, 4);
      int[,] testB = { { 1, 2, 1, 1 }, { 1, 4, 2 , 9}, { 4, 6, 13, 5 }, { 7, 8, 9, 5 } };
      boardB.BoardPieces = testB;
      boardB.cols = 4;
      boardB.rows = 4;
      
      //PrintBoard(board); // debug option to show the board 
      Assert.That(boardB.VerifySwap(new Point(0,0) ,new Point(0,1)) != null, Is.EqualTo(true));
   }

   [Test]
   public void ErasePiece_Test()
   {
      // verify if the erased points give theirs places to theirs upper neighbors 
      Board board = new Board(9, 4, 4);
      int[,] test = { { 1, 1, 1 }, { 7, 4, 2 }, { 4, 6, 9 }, { 7, 8, 9 } };
      board.BoardPieces = test;
      board.cols = 3;
      board.rows = 4;
      List<Point> points = new List<Point>();
      points.Add(new Point(0,0));
      points.Add(new Point(0,1));
      points.Add(new Point(0,2));
      List<Point> newPoints = board.ErasePieces(points);
      
      //PrintBoard(board); // debug option to show the board 
      
      int[,] expect = {{7, 4, 2}}; // second row that will be the first one 
      int[,] real = {{board.BoardPieces[0,0],board.BoardPieces[0,1] , board.BoardPieces[0,2]}}; // second row that will be the first one
      
      // row test
      Assert.That(real, Is.EqualTo(expect));
      
      //expected new points 
      List<Point> expectedNewPoints = new List<Point>();
      expectedNewPoints.Add(new Point(3,0));
      expectedNewPoints.Add(new Point(3,1));
      expectedNewPoints.Add(new Point(3,2));
      
      // newPoints test
      Assert.That(newPoints, Is.EqualTo(expectedNewPoints));
      
      
      // verify vertical erase of points 
      Board boardB = new Board(9, 4, 4);
      int[,] testB = { { 1, 1, 2 }, { 2, 4, 2 }, { 3, 6, 9 }, { 4, 8, 9 } };
      boardB.BoardPieces = testB;
      boardB.cols = 3;
      boardB.rows = 4;
      List<Point> pointsB = new List<Point>();
      pointsB.Add(new Point(0,0));
      pointsB.Add(new Point(1,0));
      pointsB.Add(new Point(2,0));
      
      List<Point> newPointsB = boardB.ErasePieces(pointsB);
      int[,] expectBrow = {{4, 1, 2}}; // test the first row
      int[,] realBrow = {{boardB.BoardPieces[0,0],boardB.BoardPieces[0,1] , boardB.BoardPieces[0,2]}}; // test the first row 
      
      // row test
      Assert.That(realBrow, Is.EqualTo(expectBrow));
      
      //expected new points 
      List<Point> expectedNewPointsB = new List<Point>();
      expectedNewPointsB.Add(new Point(1,0));
      expectedNewPointsB.Add(new Point(2,0));
      expectedNewPointsB.Add(new Point(3,0));
      
      // newPoints test
      Assert.That(newPointsB, Is.EqualTo(expectedNewPointsB));

   }

   // HELPER FUNCTION TO SHOW THE BOARD IN CONSOLE 
   private void PrintBoard(Board board)
   {
      for (int i = board.rows - 1; i >= 0; i--)
      {
         List<int> row = new List<int>() ;
         for (int j = 0; j < board.cols; j++)
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
