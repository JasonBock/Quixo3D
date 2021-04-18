using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Engine;
using Quixo3D.Engine.Sidetracked;
using Quixo3D.Framework;
using Quixo3D.Framework.Tests;
using System;
using System.Globalization;

namespace Quixo3D.Engine.Tests
{
	[TestClass]
	public abstract class EngineTests : TestsBase
	{
		public void CreateGameCode()
		{
			var board = new Board();
			var engine = new RandomEngine();

			for(var i = 0; i < 50; i++)
			{
				try
				{
					board.MovePiece(engine.GenerateMove(board));
				}
				catch(InvalidMoveException)
				{
					break;
				}
			}

			this.TestContext.WriteLine("Board board = new Board();");

			foreach(var move in board.MoveHistory)
			{
				this.TestContext.WriteLine(string.Format(CultureInfo.CurrentCulture,
					"board.MovePiece(new Coordinate({0}, {1}, {2}), new Coordinate({3}, {4}, {5}));",
					move.Source.X, move.Source.Y, move.Source.Z,
					move.Destination.X, move.Destination.Y, move.Destination.Z));
			}
		}

		protected static Board GenerateComplexGame()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 2, 4), new Coordinate(0, 2, 0));
			board.MovePiece(new Coordinate(4, 1, 4), new Coordinate(4, 1, 0));
			board.MovePiece(new Coordinate(4, 1, 4), new Coordinate(4, 1, 0));
			board.MovePiece(new Coordinate(4, 2, 0), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 4), new Coordinate(4, 1, 0));
			board.MovePiece(new Coordinate(4, 2, 0), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 1), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(4, 2, 2), new Coordinate(4, 2, 4));
			board.MovePiece(new Coordinate(4, 1, 3), new Coordinate(4, 1, 4));
			board.MovePiece(new Coordinate(3, 0, 0), new Coordinate(3, 4, 0));
			return board;
		}

		protected static Board GenerateGameWithEvenPosition()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(4, 4, 4), new Coordinate(0, 4, 4));
			board.MovePiece(new Coordinate(1, 4, 4), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(0, 4, 1), new Coordinate(4, 4, 1));
			board.MovePiece(new Coordinate(4, 4, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(4, 4, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(3, 4, 4), new Coordinate(3, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));
			board.MovePiece(new Coordinate(4, 2, 4), new Coordinate(0, 2, 4));
			board.MovePiece(new Coordinate(4, 4, 2), new Coordinate(0, 4, 2));
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(4, 0, 0));
			return board;
		}

		protected static Board GenerateGameWithPlayerAboutToLose()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 4, 4), new Coordinate(4, 4, 4));
			board.MovePiece(new Coordinate(3, 4, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 4, 4), new Coordinate(4, 4, 4));
			board.MovePiece(new Coordinate(3, 4, 2), new Coordinate(0, 4, 2));
			board.MovePiece(new Coordinate(0, 4, 4), new Coordinate(4, 4, 4));
			board.MovePiece(new Coordinate(0, 4, 2), new Coordinate(4, 4, 2));
			board.MovePiece(new Coordinate(0, 4, 4), new Coordinate(4, 4, 4));		
			return board;
		}

		protected static Board GenerateSimpleGame()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			return board;
		}
	}
}
