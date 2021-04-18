using System;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Specifies where a <see cref="Coordinate" /> is on the <see cref="Board" />.
	/// </summary>
	public enum CoordinateLocation
	{
		/// <summary>
		/// Corner location.
		/// </summary>
		Corner,
		/// <summary>
		/// Edge location.
		/// </summary>
		Edge,
		/// <summary>
		/// Face location.
		/// </summary>
		Face,
		/// <summary>
		/// Middle location.
		/// </summary>
		Middle
	}
}
