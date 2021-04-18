using Quixo3D.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quixo3D.Engine
{
	/// <summary>
	/// A Quixo3D engine that does nothing but generate random moves.
	/// </summary>
	public sealed class RandomEngine : IEngine
	{
		/// <summary>
		/// Generates a random move.
		/// </summary>
		/// <param name="board">The <see cref="Board" /> to generate a move for.</param>
		/// <returns>A random <see cref="Move" />.</returns>
		public Move GenerateMove(Board board)
		{
			var random = new Random();

			var moves = board.GetValidMoves();
			var sourceIndex = random.Next(moves.Count);
			var move = moves[sourceIndex];

			var source = move.Source;

			var destinationIndex = random.Next(move.Destinations.Count);
			var destination = move.Destinations[destinationIndex];

			return new Move(source, destination);
		}

		/// <summary>
		/// Gets the number of evaluations that were performed with the last move generation,
		/// which is always 1.
		/// </summary>
		public long Evaluations
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Gets the number of move visitations that were performed with the last move generation,
		/// which is always 0.
		/// </summary>
		public long Visitations
		{
			get
			{
				return 0;
			}
		}
	}
}
