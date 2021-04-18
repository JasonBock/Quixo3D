using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class BoardValidMoveCollectionTests : TestsBase
	{
		[TestMethod]
		public void GetValidMoveEnumerator()
		{
			var board = new Board();
			var count = 0;

			foreach(var move in board.GetValidMoveEnumerator())
			{
				switch(move.Source.Location)
				{
					case (CoordinateLocation.Corner):
						Assert.AreEqual(3, move.Destinations.Count);
						break;
					case (CoordinateLocation.Edge):
						Assert.AreEqual(4, move.Destinations.Count);
						break;
					case (CoordinateLocation.Face):
						Assert.AreEqual(5, move.Destinations.Count);
						break;
					case (CoordinateLocation.Middle):
						Assert.Fail("Middle pieces are not valid for source coordinates");
						break;
				}

				foreach(var destination in move.Destinations)
				{
					Assert.AreNotEqual(CoordinateLocation.Middle, destination.Location);
				}

				count++;
			}

			Assert.AreEqual(98, count);
		}

		[TestMethod]
		public void GetNonGenericValidMoveEnumerator()
		{
			var board = new Board();
			var count = 0;

			var enumerator = ((IEnumerable)board.GetValidMoveEnumerator()).GetEnumerator();
			
			while(enumerator.MoveNext())
			{
				Assert.IsNotNull(enumerator.Current as ValidMove);
				count++;
			}

			Assert.AreEqual(98, count);
		}		
	}
}
