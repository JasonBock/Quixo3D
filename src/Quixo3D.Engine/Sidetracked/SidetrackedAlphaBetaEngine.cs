using Quixo3D.Framework;
using System;
using System.Collections.Generic;

namespace Quixo3D.Engine.Sidetracked
{
	/// <summary>
	/// Generates a move using the min-max (with alpha-beta pruning) algorithm.
	/// </summary>
	public sealed class SidetrackedAlphaBetaEngine : IEngine
	{
		private Move selectedMove;

		private int Evaluate(Board board)
		{
			this.Evaluations++;
			return BoardEvaluator.Evaluate(board);
		}

		/// <summary>
		/// Generates a new move.
		/// </summary>
		/// <param name="board">The <see cref="Board" /> to generate a move for.</param>
		/// <returns>The next move.</returns>
		public Move GenerateMove(Board board)
		{
			this.Evaluations = 0;
			this.Visitations = 0;

			this.AlphaBeta(board, int.MinValue, int.MaxValue, 0, 2);

			return this.selectedMove;
		}

		private long AlphaBeta(Board board, long alpha, long beta, int currentDepth, int maximumDepth)
		{
			var eval = 0L;

			if(currentDepth >= maximumDepth || board.Winner != Players.None)
			{
				eval = this.Evaluate(board);
			}
			else
			{
				foreach(var move in new ValidMovesAsMovesCollection(board.GetValidMoveEnumerator()))
				{
					this.Visitations++;
					board.MovePiece(move);
					var val = -1 * this.AlphaBeta(board, -1 * beta, -1 * alpha, currentDepth + 1, maximumDepth);
					board.Undo();

					if(val >= beta)
					{
						eval = beta;
						
						if(currentDepth == 0)
						{
							this.selectedMove = move;						
						}
						
						break;
					}

					if(val > alpha)
					{
						alpha = val;
						eval = val;

						if(currentDepth == 0)
						{
							this.selectedMove = move;
						}
					}
				}
			}

			return eval;
		}

		/// <summary>
		/// Gets the number of calculations (i.e. evaluations) that were performed with the last move generation.
		/// </summary>
		public long Evaluations
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the number of moves that were visited during the last move generation.
		/// </summary>
		public long Visitations
		{
			get;
			private set;
		}
	}
}
