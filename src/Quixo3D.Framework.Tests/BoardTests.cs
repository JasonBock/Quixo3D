using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class BoardTests : TestsBase
	{
		[TestMethod]
		public void CheckInitialBoardState()
		{
			var board = new Board();
			Assert.AreEqual(Players.X, board.Current);
			Assert.AreEqual(0, board.MoveHistory.Count);
			Assert.AreEqual(0, board.WinningLines.Count);

			for(var x = 0; x < Constants.Dimension; x++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					for(var z = 0; z < Constants.Dimension; z++)
					{
						Assert.AreEqual(Players.None, board.GetPlayer(
							new Coordinate(x, y, z)));
					}
				}
			}
		}

		[TestMethod]
		public void Clone()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 0, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 0), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));

			var clone = board.Clone();
			
			Assert.IsNotNull(clone);
			Assert.AreEqual(board.MoveHistory.Count, clone.MoveHistory.Count);
			Assert.AreEqual(board.Current, clone.Current);
			Assert.AreEqual(board.Winner, clone.Winner);
			Assert.AreEqual(board.WinningLines.Count, clone.WinningLines.Count);			
		}
		
		[TestMethod]
		public void CreateWinningGameForO()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 0, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 0), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));
			board.MovePiece(new Coordinate(2, 0, 0), new Coordinate(2, 4, 0));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 4, 0));
			board.MovePiece(new Coordinate(3, 0, 3), new Coordinate(3, 4, 3));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 4, 0));
			board.MovePiece(new Coordinate(4, 0, 3), new Coordinate(4, 4, 3));

			Assert.AreEqual(Players.O, board.Winner);
			Assert.AreEqual(Players.None, board.Current);	
			Assert.AreEqual(1, board.WinningLines.Count);		
		}

		[TestMethod]
		public void CreateWinningGameForX()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 0, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 0), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));
			board.MovePiece(new Coordinate(2, 0, 0), new Coordinate(2, 4, 0));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 4, 0));
			board.MovePiece(new Coordinate(3, 0, 3), new Coordinate(3, 4, 3));
			board.MovePiece(new Coordinate(4, 0, 0), new Coordinate(4, 4, 0));
			
			Assert.AreEqual(Players.X, board.Winner);
			Assert.AreEqual(Players.None, board.Current);
			Assert.AreEqual(1, board.WinningLines.Count);
		}

		[TestMethod]
		public void GetValidMoves()
		{
			var board = new Board();
			var validMoves = board.GetValidMoves();
			Assert.AreEqual(98, validMoves.Count);

			foreach(var validMove in validMoves)
			{
				switch(validMove.Source.Location)
				{
					case (CoordinateLocation.Corner):
						Assert.AreEqual(3, validMove.Destinations.Count);
						break;
					case (CoordinateLocation.Edge):
						Assert.AreEqual(4, validMove.Destinations.Count);
						break;
					case (CoordinateLocation.Face):
						Assert.AreEqual(5, validMove.Destinations.Count);
						break;
					case (CoordinateLocation.Middle):
						Assert.Fail("Middle pieces are not valid for source coordinates");
						break;
				}

				foreach(var destination in validMove.Destinations)
				{
					Assert.AreNotEqual(CoordinateLocation.Middle, destination.Location);
				}
			}
		}

		[TestMethod]
		public void MoveToCauseOpponentToWin()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 4), new Coordinate(0, 0, 0));
			board.MovePiece(new Coordinate(0, 4, 3), new Coordinate(4, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 4), new Coordinate(1, 0, 0));
			board.MovePiece(new Coordinate(0, 4, 3), new Coordinate(4, 4, 3));
			board.MovePiece(new Coordinate(2, 0, 4), new Coordinate(2, 0, 0));
			board.MovePiece(new Coordinate(0, 4, 3), new Coordinate(4, 4, 3));
			board.MovePiece(new Coordinate(3, 0, 4), new Coordinate(3, 0, 0));
			board.MovePiece(new Coordinate(0, 4, 3), new Coordinate(4, 4, 3));
			board.MovePiece(new Coordinate(0, 0, 1), new Coordinate(4, 0, 1));
			board.MovePiece(new Coordinate(4, 0, 0), new Coordinate(4, 0, 4));
			Assert.AreEqual(Players.X, board.Winner);
			Assert.AreEqual(Players.None, board.Current);
			Assert.AreEqual(1, board.WinningLines.Count);
		}

		[TestMethod]
		public void MovePiecesViaMoveReference()
		{
			var board = new Board();

			var updated = board.MovePiece(
				new Move(new Coordinate(3, 4, 1), new Coordinate(3, 0, 1)));
			Assert.AreEqual(5, updated.Count);
			Assert.AreEqual(new Coordinate(3, 4, 1), updated[0]);
			Assert.AreEqual(new Coordinate(3, 3, 1), updated[1]);
			Assert.AreEqual(new Coordinate(3, 2, 1), updated[2]);
			Assert.AreEqual(new Coordinate(3, 1, 1), updated[3]);
			Assert.AreEqual(new Coordinate(3, 0, 1), updated[4]);

			Assert.AreEqual(1, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 4, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 3, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 2, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 1, 1)));
			Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(3, 0, 1)));
		}
				
		[TestMethod]
		public void MovePieces()
		{
			var board = new Board();

			var updated = board.MovePiece(
				new Coordinate(3, 4, 1), new Coordinate(3, 0, 1));
			Assert.AreEqual(5, updated.Count);
			Assert.AreEqual(new Coordinate(3, 4, 1), updated[0]);
			Assert.AreEqual(new Coordinate(3, 3, 1), updated[1]);
			Assert.AreEqual(new Coordinate(3, 2, 1), updated[2]);
			Assert.AreEqual(new Coordinate(3, 1, 1), updated[3]);
			Assert.AreEqual(new Coordinate(3, 0, 1), updated[4]);

			Assert.AreEqual(1, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 4, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 3, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 2, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 1, 1)));
			Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(3, 0, 1)));
			
			board.MovePiece(new Coordinate(2, 0, 4), new Coordinate(4, 0, 4));
			Assert.AreEqual(2, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(0, 0, 4)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(1, 0, 4)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 0, 4)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 0, 4)));
			Assert.AreEqual(Players.O, board.GetPlayer(new Coordinate(4, 0, 4)));
			
			board.MovePiece(new Coordinate(0, 3, 3), new Coordinate(0, 3, 0));
			Assert.AreEqual(3, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(0, 3, 4)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(0, 3, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(0, 3, 2)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(0, 3, 1)));
			Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(0, 3, 0)));
			
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 0, 4));
			Assert.AreEqual(4, board.MoveHistory.Count);
			Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(3, 0, 0)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 0, 1)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 0, 2)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 0, 3)));
			Assert.AreEqual(Players.O, board.GetPlayer(new Coordinate(3, 0, 4)));

			board.MovePiece(new Coordinate(4, 3, 3), new Coordinate(0, 3, 3));
			Assert.AreEqual(5, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(4, 3, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(3, 3, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 3, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(1, 3, 3)));
			Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(0, 3, 3)));
			
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));
			Assert.AreEqual(6, board.MoveHistory.Count);
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 0, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 1, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 2, 3)));
			Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(2, 3, 3)));
			Assert.AreEqual(Players.O, board.GetPlayer(new Coordinate(2, 4, 3)));
		}
	}
}
