using Quixo3D.Framework;
using System;
using System.Collections.Generic;

namespace Quixo3D.Engine.Sidetracked
{
	/// <summary>
	/// Evaluates the board based on connectivity within lines that contain non-blank pieces.
	/// </summary>
	public static class BoardEvaluator
	{
		/// <summary>
		/// Evaluates a <see cref="Board" />.
		/// </summary>
		/// <param name="board">The <see cref="Board" /> to evaluate.</param>
		/// <returns>A numeric value indicating the strenght of the position for the current player.</returns>
		public static int Evaluate(Board board)
		{
			var evaluation = 0;

			if(board.Winner != Players.None)
			{
				if(board.Winner == board.Current)
				{
					evaluation = int.MaxValue;
				}
				else
				{
					evaluation = int.MinValue;
				}
			}
			else
			{
				var visitedLines = new List<Line>();

				for(var x = 0; x < Constants.Dimension; x++)
				{
					for(var y = 0; y < Constants.Dimension; y++)
					{
						for(var z = 0; z < Constants.Dimension; z++)
						{
							var position = new Coordinate(x, y, z);

							if(board.GetPlayer(position) != Players.None)
							{
								foreach(var line in new ValidLineCollection(position))
								{
									if(!visitedLines.Contains(line))
									{
										evaluation += BoardEvaluator.Evaluate(line, board);
										visitedLines.Add(line);
									}
								}							
							}
						}
					}
				}			
			}
			
			return evaluation;
		}
		
		private static int Evaluate(Line line, Board board)
		{
			var evaluation = 0;
			var last = Players.None;
			var current = board.Current;
			var continuationFactor = 1;

			foreach(var position in line)
			{
				var atPosition = board.GetPlayer(position);
				
				if(atPosition != Players.None)
				{
					if(atPosition == last)
					{
						evaluation += continuationFactor * (atPosition == current ? 1 : -1);
						continuationFactor += BoardEvaluator.UpdateContinuation(continuationFactor);
					}
					else
					{
						continuationFactor = 1;
						evaluation += continuationFactor * (atPosition == current ? 1 : -1);
					}
				}
				
				last = atPosition;
			}

			var start = board.GetPlayer(line.Start);

			if(start != Players.None && start == board.GetPlayer(line.End))
			{
				evaluation += (current == start ? 2 : -2);
			}
			
			return evaluation;
		}

		private static int UpdateContinuation(int currentContinuationFactor)
		{
			return (currentContinuationFactor * currentContinuationFactor) * 4;
		}
	}
}
