using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class ValidLineCollectionTests : TestsBase
	{
		[TestMethod]
		public void GetLinesForCorner()
		{
			var target = new Coordinate(4, 0, 0);
			Assert.AreEqual(CoordinateLocation.Corner, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}
			
			Assert.AreEqual(7, lineCount);
		}

		[TestMethod]
		public void GetLinesForEdge()
		{
			var target = new Coordinate(4, 3, 4);
			Assert.AreEqual(CoordinateLocation.Edge, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(4, lineCount);
		}

		[TestMethod]
		public void GetLinesForFaceNotInDiagonal()
		{
			var target = new Coordinate(2, 4, 1);
			Assert.AreEqual(CoordinateLocation.Face, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(3, lineCount);
		}

		[TestMethod]
		public void GetLinesForFaceInOneDiagonal()
		{
			var target = new Coordinate(1, 1, 0);
			Assert.AreEqual(CoordinateLocation.Face, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(4, lineCount);
		}

		[TestMethod]
		public void GetLinesForFaceInBothDiagonals()
		{
			var target = new Coordinate(4, 2, 2);
			Assert.AreEqual(CoordinateLocation.Face, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(5, lineCount);
		}

		[TestMethod]
		public void GetLinesForMiddleNotInAnyDiagonal()
		{
			var target = new Coordinate(1, 1, 2);
			Assert.AreEqual(CoordinateLocation.Middle, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(4, lineCount);
		}

		[TestMethod]
		public void GetLinesForMiddleInOneDiagonal()
		{
			var target = new Coordinate(3, 1, 1);
			Assert.AreEqual(CoordinateLocation.Middle, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(7, lineCount);
		}

		[TestMethod]
		public void GetLinesForMiddleInTwoDiagonals()
		{
			var target = new Coordinate(2, 1, 2);
			Assert.AreEqual(CoordinateLocation.Middle, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(5, lineCount);
		}

		[TestMethod]
		public void GetLinesForMiddleInAllDiagonals()
		{
			var target = new Coordinate(2, 2, 2);
			Assert.AreEqual(CoordinateLocation.Middle, target.Location);

			var lineCount = 0;

			foreach(var line in new ValidLineCollection(target))
			{
				lineCount++;
			}

			Assert.AreEqual(13, lineCount);
		}
		
		[TestMethod]
		public void GetTarget()
		{
			var target = new Coordinate(1, 2, 3);
			var validLines = new ValidLineCollection(target);
			Assert.AreEqual(target, validLines.Target);
		}

		[TestMethod]
		public void RunThroughNonGenericGetEnumerator()
		{
			var target = new Coordinate(2, 2, 2);

			var lineCount = 0;

			var validLines = new ValidLineCollection(target);

			var enumerator = ((IEnumerable)validLines).GetEnumerator();
			
			while(enumerator.MoveNext())
			{
				lineCount++;
			}

			Assert.AreEqual(13, lineCount);
		}
	}
}