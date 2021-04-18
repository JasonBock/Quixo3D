using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;
using System.Collections.Generic;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class LineTests : TestsBase
	{
		[TestMethod]
		public void CheckEquality()
		{
			var lineA = new Line(new Coordinate(0, 4, 4), new Coordinate(0, 0, 0));
			var lineB = new Line(new Coordinate(4, 2, 4), new Coordinate(0, 2, 4));
			var lineC = new Line(new Coordinate(0, 0, 0), new Coordinate(0, 4, 4));
			var lineD = new Line(new Coordinate(4, 4, 4), new Coordinate(0, 0, 0));
			var lineE = new Line(new Coordinate(0, 4, 4), new Coordinate(0, 0, 0));
			
			Assert.AreNotEqual(lineA, lineB);
			Assert.AreEqual(lineA, lineC);
			Assert.AreNotEqual(lineA, lineD);
			Assert.AreEqual(lineA, lineE);
			Assert.AreNotEqual(lineB, lineC);
			Assert.AreNotEqual(lineB, lineD);
			Assert.AreNotEqual(lineB, lineE);
			Assert.AreEqual(lineC, lineE);
			Assert.AreNotEqual(lineC, lineD);
			Assert.AreEqual(lineC, lineE);
			Assert.AreNotEqual(lineD, lineE);

			Assert.IsFalse(lineA == lineB);
			Assert.IsTrue(lineA == lineC);
			Assert.IsFalse(lineA == lineD);
			Assert.IsTrue(lineA == lineE);
			Assert.IsFalse(lineB == lineC);
			Assert.IsFalse(lineB == lineD);
			Assert.IsFalse(lineB == lineE);
			Assert.IsTrue(lineC == lineE);
			Assert.IsFalse(lineC == lineD);
			Assert.IsTrue(lineC == lineE);
			Assert.IsFalse(lineD == lineE);

			Assert.IsTrue(lineA != lineB);
			Assert.IsFalse(lineA != lineC);
			Assert.IsTrue(lineA != lineD);
			Assert.IsFalse(lineA != lineE);
			Assert.IsTrue(lineB != lineC);
			Assert.IsTrue(lineB != lineD);
			Assert.IsTrue(lineB != lineE);
			Assert.IsFalse(lineC != lineE);
			Assert.IsTrue(lineC != lineD);
			Assert.IsFalse(lineC != lineE);
			Assert.IsTrue(lineD != lineE);
		}

		[TestMethod]
		public void CheckHashCode()
		{
			var lineA = new Line(new Coordinate(0, 4, 4), new Coordinate(0, 0, 0));
			var lineB = new Line(new Coordinate(4, 2, 4), new Coordinate(0, 2, 4));
			var lineC = new Line(new Coordinate(0, 4, 4), new Coordinate(0, 0, 0));
			
			Assert.IsFalse(lineA.GetHashCode() == lineB.GetHashCode());
			Assert.IsTrue(lineA.GetHashCode() == lineC.GetHashCode());
		}
		
		[TestMethod]
		public void CheckForContainingCoordinates()
		{
			var line = new Line(new Coordinate(0, 0, 0), new Coordinate(4, 0, 0));
			Assert.IsTrue(line.Contains(new Coordinate(1, 0, 0)));
			Assert.IsFalse(line.Contains(new Coordinate(1, 1, 0)));
		}
				
		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineWithInvalidDistance()
		{
			new Line(new Coordinate(1, 3, 4), new Coordinate(3, 3, 4));
		}

		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineWithPiecesInTheMiddle()
		{
			new Line(new Coordinate(2, 2, 2), new Coordinate(2, 2, 3));
		}

		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineWithStartAndEndTheSame()
		{
			new Line(new Coordinate(0, 0, 0), new Coordinate(0, 0, 0));
		}

		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineCornerToEdge()
		{
			new Line(new Coordinate(0, 0, 0), new Coordinate(0, 2, 4));
		}

		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineCornerToFace()
		{
			new Line(new Coordinate(0, 0, 0), new Coordinate(3, 3, 4));
		}

		[TestMethod, ExpectedException(typeof(LineException))]
		public void CreateLineEdgeToFace()
		{
			new Line(new Coordinate(4, 2, 4), new Coordinate(3, 3, 0));
		}

		[TestMethod]
		public void CreateLineCornerToCorner()
		{
			var line = new Line(new Coordinate(0, 0, 0), new Coordinate(4, 0, 0));
			Assert.AreEqual(new Coordinate(0, 0, 0), line.Start);
			Assert.AreEqual(new Coordinate(4, 0, 0), line.End);

			var positions = new List<Coordinate>(line);
			Assert.AreEqual(5, positions.Count);
			Assert.AreEqual(new Coordinate(0, 0, 0), positions[0]);
			Assert.AreEqual(new Coordinate(1, 0, 0), positions[1]);
			Assert.AreEqual(new Coordinate(2, 0, 0), positions[2]);
			Assert.AreEqual(new Coordinate(3, 0, 0), positions[3]);
			Assert.AreEqual(new Coordinate(4, 0, 0), positions[4]);
		}

		[TestMethod]
		public void CreateLineEdgeToEdge()
		{
			var line = new Line(new Coordinate(4, 2, 4), new Coordinate(0, 2, 4));
			Assert.AreEqual(new Coordinate(4, 2, 4), line.Start);
			Assert.AreEqual(new Coordinate(0, 2, 4), line.End);

			var positions = new List<Coordinate>(line);
			Assert.AreEqual(5, positions.Count);
			Assert.AreEqual(new Coordinate(4, 2, 4), positions[0]);
			Assert.AreEqual(new Coordinate(3, 2, 4), positions[1]);
			Assert.AreEqual(new Coordinate(2, 2, 4), positions[2]);
			Assert.AreEqual(new Coordinate(1, 2, 4), positions[3]);
			Assert.AreEqual(new Coordinate(0, 2, 4), positions[4]);
		}

		[TestMethod]
		public void CreateLineFaceToFace()
		{
			var line = new Line(new Coordinate(3, 3, 0), new Coordinate(3, 3, 4));
			Assert.AreEqual(new Coordinate(3, 3, 0), line.Start);
			Assert.AreEqual(new Coordinate(3, 3, 4), line.End);

			var positions = new List<Coordinate>(line);
			Assert.AreEqual(5, positions.Count);
			Assert.AreEqual(new Coordinate(3, 3, 0), positions[0]);
			Assert.AreEqual(new Coordinate(3, 3, 1), positions[1]);
			Assert.AreEqual(new Coordinate(3, 3, 2), positions[2]);
			Assert.AreEqual(new Coordinate(3, 3, 3), positions[3]);
			Assert.AreEqual(new Coordinate(3, 3, 4), positions[4]);
		}

		[TestMethod]
		public void CreateDiagonalLineCornerToCornerOnFace()
		{
			var line = new Line(new Coordinate(0, 4, 4), new Coordinate(0, 0, 0));
			Assert.AreEqual(new Coordinate(0, 4, 4), line.Start);
			Assert.AreEqual(new Coordinate(0, 0, 0), line.End);

			var positions = new List<Coordinate>(line);
			Assert.AreEqual(5, positions.Count);
			Assert.AreEqual(new Coordinate(0, 4, 4), positions[0]);
			Assert.AreEqual(new Coordinate(0, 3, 3), positions[1]);
			Assert.AreEqual(new Coordinate(0, 2, 2), positions[2]);
			Assert.AreEqual(new Coordinate(0, 1, 1), positions[3]);
			Assert.AreEqual(new Coordinate(0, 0, 0), positions[4]);
		}

		[TestMethod]
		public void CreateDiagonalLineCornerToCornerThroughCube()
		{
			var line = new Line(new Coordinate(4, 4, 4), new Coordinate(0, 0, 0));
			Assert.AreEqual(new Coordinate(4, 4, 4), line.Start);
			Assert.AreEqual(new Coordinate(0, 0, 0), line.End);

			var positions = new List<Coordinate>(line);
			Assert.AreEqual(5, positions.Count);
			Assert.AreEqual(new Coordinate(4, 4, 4), positions[0]);
			Assert.AreEqual(new Coordinate(3, 3, 3), positions[1]);
			Assert.AreEqual(new Coordinate(2, 2, 2), positions[2]);
			Assert.AreEqual(new Coordinate(1, 1, 1), positions[3]);
			Assert.AreEqual(new Coordinate(0, 0, 0), positions[4]);
		}
	}
}
