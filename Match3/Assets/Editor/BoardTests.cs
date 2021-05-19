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
   }

   private void PrintBoard(Board board)
   {
      for (int i = 0; i < board.rows; i++)
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
