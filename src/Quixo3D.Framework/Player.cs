using System;

namespace Quixo3D.Framework
{
	/// <summary>
	/// This enumeration represent the three valid states that a piece can have.
	/// </summary>
	[Flags]
	public enum Players
	{
		/// <summary>
		/// The piece with no claimed player.
		/// </summary>
		None = 0,
		/// <summary>
		/// The piece claimed by the first player (X).
		/// </summary>
		X = 1,
		/// <summary>
		/// The piece claimed by the second player (X).
		/// </summary>
		O = 2
	}
}
