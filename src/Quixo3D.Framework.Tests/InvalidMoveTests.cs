using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class InvalidMoveTests : TestsBase
	{
		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveAfterWinningMove()
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
			board.MovePiece(new Coordinate(3, 0, 2), new Coordinate(3, 4, 2));
		}

		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveInvalidDestinationPiece()
		{
			new Board().MovePiece(new Coordinate(0, 2, 2), new Coordinate(2, 2, 2));
		}

		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveInvalidSourcePiece()
		{
			new Board().MovePiece(new Coordinate(2, 2, 2), new Coordinate(0, 2, 2));
		}

		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveMiddlePiece()
		{
			new Board().MovePiece(new Coordinate(3, 3, 2), new Coordinate(3, 0, 2));
		}
		
		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveOpponentsPiece()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 4, 0), new Coordinate(0, 0, 0));		
		}

		[TestMethod, ExpectedException(typeof(InvalidMoveException))]
		public void MoveSourceToDestination()
		{
			new Board().MovePiece(new Coordinate(0, 2, 2), new Coordinate(0, 2, 2));
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void MoveWithNullReference()
		{
			new Board().MovePiece(null);
		}
	}
}
