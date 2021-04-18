using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class UndoTests : TestsBase
	{
		[TestMethod]
		public void UndoMoveAfterMoveCausesWin()
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

			var current = board.Current;

			board.MovePiece(new Coordinate(4, 0, 3), new Coordinate(4, 4, 3));

			Assert.IsTrue(board.Current == Players.None);
			Assert.IsTrue(board.Winner != Players.None);
			Assert.IsTrue(board.WinningLines.Count > 0);

			board.Undo();

			Assert.AreEqual(current, board.Current);
			
			Assert.IsTrue(board.Current != Players.None);
			Assert.IsTrue(board.Winner == Players.None);
			Assert.IsTrue(board.WinningLines.Count == 0);
		}

		[TestMethod]
		public void UndoMoves()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(3, 4, 1), new Coordinate(3, 0, 1));
			board.MovePiece(new Coordinate(2, 0, 4), new Coordinate(4, 0, 4));
			board.Undo();

			Assert.AreEqual(1, board.MoveHistory.Count);

			for(var x = 0; x < Constants.Dimension; x++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					for(var z = 0; z < Constants.Dimension; z++)
					{
						if(x == 3 && y == 0 && z == 1)
						{
							Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(x, y, z)));
						}
						else
						{
							Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(x, y, z)));
						}
					}
				}
			}

			board.Undo();

			Assert.AreEqual(0, board.MoveHistory.Count);

			for(var x = 0; x < Constants.Dimension; x++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					for(var z = 0; z < Constants.Dimension; z++)
					{
						Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(x, y, z)));
					}
				}
			}

			board.Undo();

			Assert.AreEqual(0, board.MoveHistory.Count);
		}

		[TestMethod]
		public void UndoMoveWithNoMoveHistory()
		{
			new Board().Undo();
		}

		[TestMethod]
		public void UndoMultipleMoves()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(3, 4, 1), new Coordinate(3, 0, 1));
			board.MovePiece(new Coordinate(2, 0, 4), new Coordinate(4, 0, 4));
			board.MovePiece(new Coordinate(0, 3, 3), new Coordinate(0, 3, 0));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 0, 4));
			board.MovePiece(new Coordinate(4, 3, 3), new Coordinate(0, 3, 3));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));

			board.Undo(3);
			Assert.AreEqual(3, board.MoveHistory.Count);

			board.Undo(2);
			Assert.AreEqual(1, board.MoveHistory.Count);

			for(var x = 0; x < Constants.Dimension; x++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					for(var z = 0; z < Constants.Dimension; z++)
					{
						if(x == 3 && y == 0 && z == 1)
						{
							Assert.AreEqual(Players.X, board.GetPlayer(new Coordinate(x, y, z)));
						}
						else
						{
							Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(x, y, z)));
						}
					}
				}
			}
		}

		[TestMethod]
		public void UndoMultipleMovesWithNoMoveHistory()
		{
			new Board().Undo(5);
		}

		[TestMethod]
		public void UndoTooManyMoves()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(3, 4, 1), new Coordinate(3, 0, 1));
			board.MovePiece(new Coordinate(2, 0, 4), new Coordinate(4, 0, 4));
			board.MovePiece(new Coordinate(0, 3, 3), new Coordinate(0, 3, 0));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 0, 4));
			board.MovePiece(new Coordinate(4, 3, 3), new Coordinate(0, 3, 3));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));
			board.Undo(10);

			Assert.AreEqual(0, board.MoveHistory.Count);

			for(var x = 0; x < Constants.Dimension; x++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					for(var z = 0; z < Constants.Dimension; z++)
					{
						Assert.AreEqual(Players.None, board.GetPlayer(new Coordinate(x, y, z)));
					}
				}
			}
		}
	}
}
