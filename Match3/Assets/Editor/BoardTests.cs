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
   }

   [Test]
   public void CheckMatch3_Test()
   {
      Board board = new Board(3, 10, 5);
      // PrintBoard(board);
      // Dont match3 test 
      Assert.That(board.CheckMatch3(), Is.EqualTo(null));
      
      // Horizontal test 
      int[,] testA =  { { 1, 1, 1 }, { 3, 4, 2 }, { 5, 6, 9 }, { 7, 8, 9 } };
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
      // Horizontal test 
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
