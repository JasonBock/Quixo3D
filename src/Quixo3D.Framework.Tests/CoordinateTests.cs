using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Framework;
using System;

namespace Quixo3D.Framework.Tests
{
	/// <summary>
	/// Contains tests for the <see cref="Coordinate" /> structure.
	/// </summary>
	[TestClass]
	public sealed class CoordinateTests : TestsBase
	{
		[TestMethod]
		public void CheckForEquality()
		{
			var c1 = new Coordinate(1, 2, 3);
			var c2 = new Coordinate(3, 2, 1);
			var c3 = new Coordinate(1, 2, 3);

			Assert.AreNotEqual<Coordinate>(c1, c2);
			Assert.AreEqual<Coordinate>(c1, c3);
			Assert.AreNotEqual<Coordinate>(c2, c3);
		}

		[TestMethod]
		public void CheckForEqualityViaOperators()
		{
			var c1 = new Coordinate(1, 2, 3);
			var c2 = new Coordinate(3, 2, 1);
			var c3 = new Coordinate(1, 2, 3);

#pragma warning disable 1718
			Assert.IsTrue(c1 == c1);
#pragma warning restore 1718
			Assert.IsTrue(c1 != c2);
			Assert.IsTrue(c1 == c3);
			Assert.IsTrue(c2 != c3);
		}

		[TestMethod]
		public void CheckForEqualityWithIncompatibleTypes()
		{
			var c = new Coordinate(1, 2, 3);
			var g = Guid.NewGuid();

			Assert.AreNotEqual(c, g);
		}

		[TestMethod]
		public void CreateCornerCoordinates()
		{
			Assert.AreEqual(CoordinateLocation.Corner, new Coordinate().Location);
			Assert.AreEqual(CoordinateLocation.Corner, new Coordinate(4, 0, 0).Location);
			Assert.AreEqual(CoordinateLocation.Corner, new Coordinate(4, 4, 0).Location);
			Assert.AreEqual(CoordinateLocation.Corner, new Coordinate(0, 4, 0).Location);
		}

		[TestMethod]
		public void CreateEdgeCoordinates()
		{
			Assert.AreEqual(CoordinateLocation.Edge, new Coordinate(0, 1, 0).Location);
			Assert.AreEqual(CoordinateLocation.Edge, new Coordinate(4, 0, 1).Location);
			Assert.AreEqual(CoordinateLocation.Edge, new Coordinate(3, 4, 0).Location);
			Assert.AreEqual(CoordinateLocation.Edge, new Coordinate(2, 0, 0).Location);
		}

		[TestMethod]
		public void CreateFaceCoordinates()
		{
			Assert.AreEqual(CoordinateLocation.Face, new Coordinate(2, 0, 1).Location);
			Assert.AreEqual(CoordinateLocation.Face, new Coordinate(3, 4, 1).Location);
			Assert.AreEqual(CoordinateLocation.Face, new Coordinate(2, 4, 2).Location);
			Assert.AreEqual(CoordinateLocation.Face, new Coordinate(0, 1, 3).Location);
		}

		[TestMethod]
		public void CreateMiddleCoordinates()
		{
			Assert.AreEqual(CoordinateLocation.Middle, new Coordinate(3, 3, 1).Location);
			Assert.AreEqual(CoordinateLocation.Middle, new Coordinate(2, 2, 2).Location);
			Assert.AreEqual(CoordinateLocation.Middle, new Coordinate(1, 3, 2).Location);
			Assert.AreEqual(CoordinateLocation.Middle, new Coordinate(3, 1, 1).Location);
		}

		[TestMethod]
		public void CreateCoordinate()
		{
			var c = new Coordinate(1, 2, 3);
			Assert.AreEqual(1, c.X);
			Assert.AreEqual(2, c.Y);
			Assert.AreEqual(3, c.Z);
			this.TestContext.WriteLine(c.ToString());
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithXValueTooLarge()
		{
			new Coordinate(10, 2, 3);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithXValueTooSmall()
		{
			new Coordinate(-10, 2, 3);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithYValueTooLarge()
		{
			new Coordinate(1, 20, 3);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithYValueTooSmall()
		{
			new Coordinate(1, -20, 3);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithZValueTooLarge()
		{
			new Coordinate(1, 2, 30);
		}

		[TestMethod, ExpectedException(typeof(ArgumentException))]
		public void CreateCoordinateWithZValueTooSmall()
		{
			new Coordinate(1, 2, -30);
		}

		[TestMethod]
		public void CreateDefaultCoordinate()
		{
			var c = new Coordinate();
			Assert.AreEqual(0, c.X);
			Assert.AreEqual(0, c.Y);
			Assert.AreEqual(0, c.Z);
		}
	}
}
