using Quixo3D.Framework;
using System;

namespace Quixo3D.Engine
{
	/// <summary>
	/// Defines the engine specification.
	/// </summary>
	public interface IEngine
	{
		/// <summary>
		/// Generates a new <see cref="Move" /> for the given <see cref="Board" />.
		/// </summary>
		/// <param name="board">A <see cref="Board" />.</param>
		/// <returns>A <see cref="Move" />.</returns>
		Move GenerateMove(Board board);
		
		/// <summary>
		/// Gets the number of evaluations that were performed with the last move generation.
		/// </summary>
		long Evaluations
		{
			get;
		}

		/// <summary>
		/// Gets the number of  moves visited with the last move generation.
		/// </summary>
		long Visitations
		{
			get;
		}
	}
}
