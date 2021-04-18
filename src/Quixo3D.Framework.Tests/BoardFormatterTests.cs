using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class BoardFormatterTests : TestsBase
	{
		[TestMethod]
		public void Deserialize()
		{
			var boardData = "<Board><Move><Source><X>0</X><Y>0</Y><Z>0</Z></Source><Destination><X>0</X><Y>4</Y><Z>0</Z></Destination></Move></Board>";

			var formatter = new BoardFormatter();
			Board board = null;

			using(var stream = new MemoryStream((new ASCIIEncoding()).GetBytes(boardData)))
			{
				board = formatter.Deserialize(stream) as Board;
			}
			
			Assert.IsNotNull(board);
			Assert.AreEqual(1, board.MoveHistory.Count);
			Move move = board.MoveHistory[0];
			Assert.AreEqual(new Coordinate(0, 0, 0), move.Source);
			Assert.AreEqual(new Coordinate(0, 4, 0), move.Destination);
		}

		[TestMethod, ExpectedException(typeof(XmlException))]
		public void DeserializeWithInvalidFormat()
		{
			var boardData = "<Board><Move><Source<X>0</X><Y>0</Y><Z>0</Z></Source><Destination><X>0</X><Y>4</Y><Z>0</Z></Destination></Move></Board>";

			var formatter = new BoardFormatter();

			using(var stream = new MemoryStream((new ASCIIEncoding()).GetBytes(boardData)))
			{
				formatter.Deserialize(stream);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void DeserializeWithNullStream()
		{
			new BoardFormatter().Deserialize(null);
		}

		[TestMethod]
		public void Roundtrip()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 0, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 0), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));
			board.MovePiece(new Coordinate(2, 0, 0), new Coordinate(2, 4, 0));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));

			var formatter = new BoardFormatter();
			Board newBoard = null;

			using(var stream = new MemoryStream())
			{
				formatter.Serialize(stream, board);
				stream.Position = 0;
				newBoard = formatter.Deserialize(stream) as Board;
			}
			
			Assert.IsNotNull(newBoard);
			Assert.AreEqual(board.MoveHistory.Count, newBoard.MoveHistory.Count);
			Move move = board.MoveHistory[2];
			Assert.AreEqual(new Coordinate(1, 0, 0), move.Source);
			Assert.AreEqual(new Coordinate(1, 4, 0), move.Destination);
		}

		[TestMethod]
		public void Serialize()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));
			board.MovePiece(new Coordinate(0, 0, 3), new Coordinate(0, 4, 3));
			board.MovePiece(new Coordinate(1, 0, 0), new Coordinate(1, 4, 0));
			board.MovePiece(new Coordinate(1, 0, 3), new Coordinate(1, 4, 3));
			board.MovePiece(new Coordinate(2, 0, 0), new Coordinate(2, 4, 0));
			board.MovePiece(new Coordinate(2, 0, 3), new Coordinate(2, 4, 3));

			var formatter = new BoardFormatter();

			using(var stream = new MemoryStream())
			{
				formatter.Serialize(stream, board);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void SerializeWithNullReference()
		{
			using(var stream = new MemoryStream())
			{
				new BoardFormatter().Serialize(stream, null);
			}
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void SerializeWithNullStream()
		{
			var board = new Board();
			board.MovePiece(new Coordinate(0, 0, 0), new Coordinate(0, 4, 0));

			new BoardFormatter().Serialize(null, board);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void SerializeWithUnsupportedType()
		{
			using(var stream = new MemoryStream())
			{
				new BoardFormatter().Serialize(stream, "bad type");
			}
		}
	}
}
