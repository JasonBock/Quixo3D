using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Defines a line (horizontal, vertical, or diagonal) on a <see cref="Board" />.
	/// </summary>
	public sealed class Line : IEquatable<Line>, IEnumerable<Coordinate>
	{
		private const string ToStringFormat = "Start: {0}, End: {1}";
		
		/// <summary>
		/// Creates a new <see cref="Line" /> instance.
		/// </summary>
		/// <param name="start">The start of the line.</param>
		/// <param name="end">The end of the line.</param>
		public Line(Coordinate start, Coordinate end)
			: base()
		{
			Line.CheckCoordinates(start, end);
			this.Start = start;
			this.End = end;
		}

		/// <summary>
		/// Determines whether two specified <see cref="Line" /> objects have the same value. 
		/// </summary>
		/// <param name="a">A <see cref="Line" /> or a null reference.</param>
		/// <param name="b">A <see cref="Line" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator ==(Line a, Line b)
		{
			bool areEqual = false;

			if(object.ReferenceEquals(a, b))
			{
				areEqual = true;
			}

			if((object)a != null && (object)b != null)
			{
				areEqual = a.Equals(b);
			}

			return areEqual;
		}

		/// <summary>
		/// Determines whether two specified <see cref="Line" /> objects have different value. 
		/// </summary>
		/// <param name="a">A <see cref="Line" /> or a null reference.</param>
		/// <param name="b">A <see cref="Line" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator !=(Line a, Line b)
		{
			return !(a == b);
		}
		
		private static void CheckCoordinates(Coordinate start, Coordinate end)
		{
			if(start == end)
			{
				throw new LineException("The start and end cannot be equal.");
			}
			
			if(start.Location != end.Location)
			{
				throw new LineException("The start and end location types cannot be different.");
			}
			
			if(start.Location == CoordinateLocation.Middle)
			{
				throw new LineException("The starting coordinate cannot be in the middle.");
			}

			if(Math.Max(Math.Max(Math.Abs(start.X - end.X), Math.Abs(start.Y - end.Y)), Math.Abs(start.Z - end.Z)) != 
				Constants.Dimension - 1)
			{
				throw new LineException("The distance is incorrect.");			
			}
		}
		
		/// <summary>
		/// Determines if a position exists within the line.
		/// </summary>
		/// <param name="position">The <see cref="Coordinate" /> to check.</param>
		/// <returns>Returns <c>true</c> if the position is in the line; otherwise, <c>false</c>.</returns>
		public bool Contains(Coordinate position)
		{
			bool contains = false;
			
			foreach(var linePosition in this)
			{
				if(linePosition == position)
				{
					contains = true;
					break;
				}
			}
			
			return contains;
		}

		/// <summary>
		/// Determines whether this instance of <see cref="Line" /> and a 
		/// specified <see cref="Line" /> object have the same value. 
		/// </summary>
		/// <param name="other">A <see cref="Line" />.</param>
		/// <returns><b>true</b> if <paramref name="other"/> is a <see cref="Line" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public bool Equals(Line other)
		{
			var areEqual = false;

			if(other != null)
			{
				areEqual = (this.Start == other.Start && this.End == other.End) ||
					(this.Start == other.End && this.End == other.Start);
			}

			return areEqual;
		}

		/// <summary>
		/// Determines whether this instance of <see cref="Line" /> and a specified object, 
		/// which must also be a <see cref="Line" /> object, have the same value. 
		/// </summary>
		/// <param name="obj">An <see cref="Object" />.</param>
		/// <returns><b>true</b> if <paramref name="obj"/> is a <see cref="Line" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Line);
		}

		/// <summary>
		/// An enumerator for all of the positions within the line.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Coordinate> GetEnumerator()
		{
			yield return this.Start;

			var xDiff = Math.Sign((int)this.End.X - (int)this.Start.X);
			var yDiff = Math.Sign((int)this.End.Y - (int)this.Start.Y);
			var zDiff = Math.Sign((int)this.End.Z - (int)this.Start.Z);
			
			for(var i = 1; i < Constants.Dimension - 1; i++)
			{
				yield return new Coordinate((byte)(this.Start.X + xDiff),
					(byte)(this.Start.Y + yDiff), (byte)(this.Start.Z + zDiff));
				xDiff += Math.Sign(xDiff);
				yDiff += Math.Sign(yDiff);
				zDiff += Math.Sign(zDiff);
			}
			
			yield return this.End;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Returns the hash code for this <see cref="Line" />.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>		
		public override int GetHashCode()
		{
			return this.Start.GetHashCode() ^ this.End.GetHashCode();
		}

		/// <summary>
		/// Gets a user-friendly version of a <see cref="Line" /> object.
		/// </summary>
		/// <returns>A string that has the start and end values.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, Line.ToStringFormat, this.Start, this.End);
		}
		
		/// <summary>
		/// Gets the end of the line.
		/// </summary>
		public Coordinate End
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the start of the line.
		/// </summary>
		public Coordinate Start
		{
			get;
			private set;
		}
	}
}
