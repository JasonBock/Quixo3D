using System;
using System.Globalization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// This class represents a move within a Quixo game.
	/// </summary>
	[Serializable]
	public class Move
	{
		private const string ToStringFormat = "Source: {0}, Destination: {1}";
				
		/// <summary>
		/// Creates a new <see cref="Move" /> instance.
		/// </summary>
		protected Move()
			: base()
		{
		}

		/// <summary>
		/// Creates a new <see cref="Move" /> instance.
		/// </summary>
		/// <param name="destination">The destination <see cref="Coordinate" />.</param>
		/// <param name="source">The source <see cref="Coordinate" />.</param>
		public Move(Coordinate source, Coordinate destination)
			: this()
		{
			this.Source = source;
			this.Destination = destination;
		}

		/// <summary>
		/// Gets a user-friendly version of a <see cref="Move" /> object.
		/// </summary>
		/// <returns>A string that has the source and destination values.</returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture,
				Move.ToStringFormat, this.Source, this.Destination);
		}
		
		/// <summary>
		/// Gets the destination <see cref="Coordinate" />
		/// </summary>
		public Coordinate Destination
		{
			get;
			private set;
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
