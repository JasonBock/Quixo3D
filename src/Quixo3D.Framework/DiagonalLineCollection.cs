using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	internal sealed class DiagonalLineCollection : IEnumerable<Line>
	{
		private Coordinate target;

		internal DiagonalLineCollection(Coordinate target)
			: base()
		{
			this.target = target;
		}

		private void AddFaceLine(List<Line> diagonals, int xDiff, int yDiff, int zDiff, 
			params CoordinateLocation[] locationsToAvoid)
		{
			var avoidedLocations = new List<CoordinateLocation>(locationsToAvoid);
			
			Coordinate start = this.target, end = this.target;

			do
			{
				if(!avoidedLocations.Contains(start.Location))
				{
					start = new Coordinate(start.X + xDiff, start.Y + yDiff, start.Z + zDiff);
				}

				if(!avoidedLocations.Contains(end.Location))
				{
					end = new Coordinate(end.X - xDiff, end.Y - yDiff, end.Z - zDiff);
				}

			} while(!avoidedLocations.Contains(start.Location) || !avoidedLocations.Contains(end.Location));

			try
			{
				diagonals.Add(new Line(start, end));
			}
			catch(LineException)
			{
			}
		}

		private IEnumerable<Line> GetCornerLines()
		{
			var cornerLines = new List<Line>();
			cornerLines.Add(new Line(this.target, new Coordinate(
				this.target.X, (byte)Math.Abs(this.target.Y - (Constants.Dimension - 1)),
				(byte)Math.Abs(this.target.Z - (Constants.Dimension - 1)))));
			cornerLines.Add(new Line(this.target, new Coordinate(
				(byte)Math.Abs(this.target.X - (Constants.Dimension - 1)), this.target.Y,
				(byte)Math.Abs(this.target.Z - (Constants.Dimension - 1)))));
			cornerLines.Add(new Line(this.target, new Coordinate(
				(byte)Math.Abs(this.target.X - (Constants.Dimension - 1)),
				(byte)Math.Abs(this.target.Y - (Constants.Dimension - 1)),
				this.target.Z)));
			cornerLines.Add(new Line(this.target, new Coordinate(
				(byte)Math.Abs(this.target.X - (Constants.Dimension - 1)),
				(byte)Math.Abs(this.target.Y - (Constants.Dimension - 1)),
				(byte)Math.Abs(this.target.Z - (Constants.Dimension - 1)))));
			return cornerLines;
		}

		private Line GetEdgeLine()
		{
			Line edgeLine;

			if(this.target.X != 0 && this.target.X != (Constants.Dimension - 1))
			{
				edgeLine = new Line(this.target, new Coordinate(
					this.target.X, (byte)Math.Abs(this.target.Y - (Constants.Dimension - 1)),
					(byte)Math.Abs(this.target.Z - (Constants.Dimension - 1))));
			}
			else if(this.target.Y != 0 && this.target.Y != (Constants.Dimension - 1))
			{
				edgeLine = new Line(this.target, new Coordinate(
					(byte)Math.Abs(this.target.X - (Constants.Dimension - 1)), this.target.Y,
					(byte)Math.Abs(this.target.Z - (Constants.Dimension - 1))));
			}
			else
			{
				edgeLine = new Line(this.target, new Coordinate(
					(byte)Math.Abs(this.target.X - (Constants.Dimension - 1)),
					(byte)Math.Abs(this.target.Y - (Constants.Dimension - 1)),
					this.target.Z));
			}

			return edgeLine;
		}

		public IEnumerator<Line> GetEnumerator()
		{
			var location = this.target.Location;

			if(location == CoordinateLocation.Corner)
			{
				foreach(var cornerLine in this.GetCornerLines())
				{
					yield return cornerLine;
				}
			}
			else if(location == CoordinateLocation.Edge)
			{
				yield return this.GetEdgeLine();
			}
			else if(location == CoordinateLocation.Face)
			{
				foreach(var faceLine in this.GetFaceLines())
				{
					yield return faceLine;
				}
			}
			else if(location == CoordinateLocation.Middle)
			{
				foreach(var middleLine in this.GetMiddleLines())
				{
					yield return middleLine;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		
		private IEnumerable<Line> GetFaceLines()
		{
			var diagonals = new List<Line>();

			// if it's a face then it could be involved in 0 - 2 diagonals
			// and it's the face that has the coordinate as 0 or C.D - 1.
			// Expand the point. First, take the other two coordinates and increment the same
			// then take one and increase, the other decrease.
			// since it's a face, you're safe getting at least one piece to expand.
			int xDiff = this.target.X == 0 || this.target.X == Constants.Dimension - 1 ? 0 : 1;
			int yDiff = this.target.Y == 0 || this.target.Y == Constants.Dimension - 1 ? 0 : 1;
			int zDiff = this.target.Z == 0 || this.target.Z == Constants.Dimension - 1 ? 0 : 1;
			this.AddFaceLine(diagonals, xDiff, yDiff, zDiff, CoordinateLocation.Corner, CoordinateLocation.Edge);
			
			xDiff = this.target.X == 0 || this.target.X == Constants.Dimension - 1 ? 0 : -1;
			yDiff = this.target.Y == 0 || this.target.Y == Constants.Dimension - 1 ? 0 : 1;
			zDiff = this.target.Z == 0 || this.target.Z == Constants.Dimension - 1 ? 0 : (xDiff != 0 ? 1 : -1);
			this.AddFaceLine(diagonals, xDiff, yDiff, zDiff, CoordinateLocation.Corner, CoordinateLocation.Edge);

			return diagonals;
		}

		private IEnumerable<Line> GetMiddleLines()
		{
			var diagonals = new List<Line>();

			// if it's a middle then it could be involved in 0-10 diagonals.
			this.AddFaceLine(diagonals, 0, 1, 1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, 0, 1, -1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);

			this.AddFaceLine(diagonals, 1, 0, 1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, 1, 0, -1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);

			this.AddFaceLine(diagonals, 1, 1, 0, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, -1, 1, 0, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);

			this.AddFaceLine(diagonals, 1, 1, -1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, 1, 1, 1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, -1, 1, -1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);
			this.AddFaceLine(diagonals, -1, 1, 1, CoordinateLocation.Corner, CoordinateLocation.Edge, CoordinateLocation.Face);

			return diagonals;
		}
	}
}
