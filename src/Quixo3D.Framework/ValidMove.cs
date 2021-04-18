using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Defines a valid move: the source <see cref="Coordinate" /> value and
	/// the destination <see cref="Coordinate" /> values.
	/// </summary>
	public sealed class ValidMove : IEquatable<ValidMove>
	{
		private List<Coordinate> destinations;
		
		/// <summary>
		/// Creates a new <see cref="ValidMove"/> instance.
		/// </summary>
		/// <param name="source">The source <see cref="Coordinate" /> value.</param>
		/// <param name="destinations">The destination <see cref="Coordinate" /> values.</param>
		internal ValidMove(Coordinate source, List<Coordinate> destinations) 
			: base() 
		{
			this.Source = source;
			this.destinations = destinations;
		}

		/// <summary>
		/// Determines whether two specified <see cref="ValidMove" /> objects have the same value. 
		/// </summary>
		/// <param name="a">A <see cref="ValidMove" /> or a null reference.</param>
		/// <param name="b">A <see cref="ValidMove" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator ==(ValidMove a, ValidMove b)
		{
			var areEqual = false;

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
		/// Determines whether two specified <see cref="ValidMove" /> objects have different value. 
		/// </summary>
		/// <param name="a">A <see cref="ValidMove" /> or a null reference.</param>
		/// <param name="b">A <see cref="ValidMove" /> or a null reference.</param>
		/// <returns><b>true</b> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <b>false</b>. </returns>
		public static bool operator !=(ValidMove a, ValidMove b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Determines whether this instance of <see cref="ValidMove" /> and a specified object, 
		/// which must also be a <see cref="ValidMove" /> object, have the same value. 
		/// </summary>
		/// <param name="obj">An <see cref="Object" />.</param>
		/// <returns><b>true</b> if <paramref name="obj"/> is a <see cref="ValidMove" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ValidMove);
		}

		/// <summary>
		/// Determines whether this instance of <see cref="ValidMove" /> and a 
		/// specified <see cref="ValidMove" /> object have the same value. 
		/// </summary>
		/// <param name="other">A <see cref="ValidMove" />.</param>
		/// <returns><b>true</b> if <paramref name="other"/> is a <see cref="ValidMove" /> and its value 
		/// is the same as this instance; otherwise, <b>false</b>.</returns>
		public bool Equals(ValidMove other)
		{
			var areEqual = false;
			
			if(other != null)
			{
				areEqual = this.Source == other.Source;	
			}
			
			return areEqual;
		}

		/// <summary>
		/// Returns the hash code for this <see cref="ValidMove" />.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return this.Source.GetHashCode();
		}

		/// <summary>
		/// Gets the valid destination <see cref="Coordinate" /> values.
		/// </summary>
		public ReadOnlyCollection<Coordinate> Destinations
		{
			get
			{
				return this.destinations.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the source <see cref="Coordinate" />
		/// </summary>
		public Coordinate Source
		{
			get;
			private set;
		}
	}
}
