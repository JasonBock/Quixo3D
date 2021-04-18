using System;
using System.Globalization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Defines the X, Y, and Z positions of a piece in the cube.
	/// </summary>
	[Serializable]
	public struct Coordinate : IEquatable<Coordinate>
	{
		private const string ErrorValueTooLarge = "The value of {0}, {1}, is too large.";
		private const string ErrorValueTooSmall = "The value of {0}, {1}, is too small.";
		private const string ToStringFormat = "{0}, {1}, {2}";
						
		/// <summary>
		/// Creates a new <see cref="Coordinate" /> instance.
		/// </summary>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="z">The z value.</param>
		/// <exception cref="ArgumentException">
		/// Thrown if any of the arguments are greater that or equal to <see cref="Constants.Dimension" />.
		/// </exception>
		public Coordinate(int x, int y, int z)
			: this()
		{			
			Coordinate.CheckValue(x, "x");
			Coordinate.CheckValue(y, "y");
			Coordinate.CheckValue(z, "z");
			
			this.X = x;
			this.Y = y;
			this.Z = z;
			
			var isXEdge = (this.X == 0 || this.X == Constants.Dimension - 1);
			var isYEdge = (this.Y == 0 || this.Y == Constants.Dimension - 1);
			var isZEdge = (this.Z == 0 || this.Z == Constants.Dimension - 1);
			
			if(isXEdge && isYEdge && isZEdge)
			{
				this.Location = CoordinateLocation.Corner;
			}
			else if((isXEdge && isYEdge) ||
				(isXEdge && isZEdge) ||
				(isYEdge && isZEdge))
			{
				this.Location = CoordinateLocation.Edge;
			}
			else if(isXEdge || isYEdge || isZEdge)
			{
				this.Location = CoordinateLocation.Face;
			}
			else
			{
				this.Location = CoordinateLocation.Middle;
			}
		}

		/// <summary>
		/// Determines whether two specified <see cref="Coordinate" /> objects have the same value. 
		/// </summary>
		/// <param name="a">A <see cref="Coordinate" /> or a null reference.</param>
		/// <param name="b">A <see cref="Coordinate" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator ==(Coordinate a, Coordinate b)
		{
			return a.Equals(b);
		}

		/// <summary>
		/// Determines whether two specified <see cref="Coordinate" /> objects have different value. 
		/// </summary>
		/// <param name="a">A <see cref="Coordinate" /> or a null reference.</param>
		/// <param name="b">A <see cref="Coordinate" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator !=(Coordinate a, Coordinate b)
		{
			return !(a == b);
		}

		private static void CheckValue(int value, string valueName)
		{
			if(value >= Constants.Dimension)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
					Coordinate.ErrorValueTooLarge, valueName, value));			
			}
			else if(value < 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
					Coordinate.ErrorValueTooSmall, valueName, value));
			}
		}

		/// <summary>
		/// Checks to see if the given object is equal to the current <see cref="Coordinate" /> instance.
		/// </summary>
		/// <param name="obj">The object to check for equality.</param>
		/// <returns>Returns <c>true</c> if the objects are equals; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			var areEqual = false;
			
			if(obj is Coordinate)
			{
				areEqual = this.Equals((Coordinate)obj);			
			}
			
			return areEqual;
		}

		/// <summary>
		/// Provides a type-safe equality check.
		/// </summary>
		/// <param name="other">The object to check for equality.</param>
		/// <returns>Returns <c>true</c> if the objects are equals; otherwise, <c>false</c>.</returns>
		public bool Equals(Coordinate other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		/// <summary>
		/// Gets a hash code based on the 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		/// <summary>
		/// Returns a meaningful string representation of the current <see cref="Coordinate" /> instance.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, Coordinate.ToStringFormat, 
				this.X, this.Y, this.Z);
		}

		/// <summary>
		/// Gets the location.
		/// </summary>
		public CoordinateLocation Location
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the x value.
		/// </summary>
		public int X
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the y value.
		/// </summary>
		public int Y
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the z value.
		/// </summary>
		public int Z
		{
			get;
			private set;
		}
	}
}
