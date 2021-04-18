using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class ValidMoveTests : TestsBase
	{
		[TestMethod]
		public void CheckForEquality()
		{
			var board = new Board();
			var validMoves = board.GetValidMoves();

			Assert.IsFalse(validMoves[0] == validMoves[1]);
			Assert.IsTrue(validMoves[0] != validMoves[1]);
			Assert.IsFalse(validMoves[0].Equals((object)validMoves[1]));
			Assert.IsFalse(validMoves[0].Equals(validMoves[1]));

			Assert.IsTrue(validMoves[0] == validMoves[0]);
			Assert.IsFalse(validMoves[0] != validMoves[0]);
			Assert.IsTrue(validMoves[0].Equals((object)validMoves[0]));
			Assert.IsTrue(validMoves[0].Equals(validMoves[0]));
		}
	}
}
