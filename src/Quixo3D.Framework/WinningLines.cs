using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Determines the number of winning lines in a given <see cref="Board" />.
	/// </summary>
	internal sealed class WinningLines
	{
		private static readonly Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));

		private enum Direction
		{
			X,
			Y,
			Z
		}

		private const string ErrorInvalidLineCount = "The total line count should be {0} but it was {1}.";
		private const int TotalLineCount = (((Constants.Dimension * 2) + 2) * 15) + 4;

		private Board board;
		private int noneCount;
		
		internal WinningLines(Board board)
			: base()
		{
			this.board = board;
			this.CalculateWinningLines();

			var winningLineCount = this.XCount + this.OCount + this.noneCount;

			if(winningLineCount != WinningLines.TotalLineCount)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
					WinningLines.ErrorInvalidLineCount, WinningLines.TotalLineCount, winningLineCount));
			}
		}

		private void CalculateWinningLines()
		{
			this.CalculateWinningLinesInLayers();
			this.CalculateWinningDiagonalLinesThroughCube();
		}

		private void CalculateWinningDiagonalLinesThroughCube()
		{
			var start = new Coordinate(0, 0, 0);
			var lineState = this.board.GetPlayer(start);

			var end = new Coordinate();

			for(var x = 1; x < Constants.Dimension; x++)
			{
				end = new Coordinate(x, x, x);
				var currentPiece = this.board.GetPlayer(end);

				if(currentPiece == Players.None || currentPiece != lineState)
				{
					lineState = Players.None;
					break;
				}
			}

			this.UpdatePlayerWinCount(lineState, start, end);

			start = new Coordinate(0, 0, Constants.Dimension - 1);
			lineState = this.board.GetPlayer(start);

			for(var x = 1; x < Constants.Dimension; x++)
			{
				end = new Coordinate(x, x, (Constants.Dimension - 1 - x));
				var currentPiece = this.board.GetPlayer(end);

				if(currentPiece == Players.None || currentPiece != lineState)
				{
					lineState = Players.None;
					break;
				}
			}

			this.UpdatePlayerWinCount(lineState, start, end);

			start = new Coordinate(0, Constants.Dimension - 1, 0);
			lineState = this.board.GetPlayer(start);

			for(var x = 1; x < Constants.Dimension; x++)
			{
				end = new Coordinate(x, (Constants.Dimension - 1 - x), x);
				var currentPiece = this.board.GetPlayer(end);

				if(currentPiece == Players.None || currentPiece != lineState)
				{
					lineState = Players.None;
					break;
				}
			}

			this.UpdatePlayerWinCount(lineState, start, end);

			start = new Coordinate(0, Constants.Dimension - 1, Constants.Dimension - 1);
			lineState = this.board.GetPlayer(start);

			for(var x = 1; x < Constants.Dimension; x++)
			{
				end = new Coordinate(x, Constants.Dimension - 1 - x, Constants.Dimension - 1 - x);
				var currentPiece = this.board.GetPlayer(end);

				if(currentPiece == Players.None || currentPiece != lineState)
				{
					lineState = Players.None;
					break;
				}
			}

			this.UpdatePlayerWinCount(lineState, start, end);
		}

		private void CalculateWinningLinesInLayers()
		{
			foreach(var direction in WinningLines.directions)
			{
				this.CalculateHorizontalWinners(direction);
				this.CalculateVerticalWinners(direction);
				this.CalculateDiagonalWinners(direction);
			}
		}

		private void CalculateHorizontalWinners(Direction direction)
		{
			for(var z = 0; z < Constants.Dimension; z++)
			{
				for(var y = 0; y < Constants.Dimension; y++)
				{
					var start = this.GetPlayer(direction, 0, y, z);
					var lineState = start.Player;

					var end = new PlayerAtCoordinate();
					for(var x = 1; x < Constants.Dimension; x++)
					{
						end = this.GetPlayer(direction, x, y, z);

						if(end.Player == Players.None || end.Player != lineState)
						{
							lineState = Players.None;
							break;
						}
					}

					this.UpdatePlayerWinCount(lineState, start.Coordinate, end.Coordinate);
				}
			}
		}

		private void CalculateVerticalWinners(Direction direction)
		{
			for(var z = 0; z < Constants.Dimension; z++)
			{
				for(var x = 0; x < Constants.Dimension; x++)
				{
					var start = this.GetPlayer(direction, x, 0, z);
					var lineState = start.Player;
					var end = new PlayerAtCoordinate();

					for(var y = 1; y < Constants.Dimension; y++)
					{
						end = this.GetPlayer(direction, x, y, z);

						if(end.Player == Players.None || end.Player != lineState)
						{
							lineState = Players.None;
							break;
						}
					}

					this.UpdatePlayerWinCount(lineState, start.Coordinate, end.Coordinate);
				}
			}
		}

		private void CalculateDiagonalWinners(Direction direction)
		{
			for(var z = 0; z < Constants.Dimension; z++)
			{
				var start = this.GetPlayer(direction, 0, 0, z);
				var lineState = start.Player;
				var end = new PlayerAtCoordinate();

				for(var x = 1; x < Constants.Dimension; x++)
				{
					end = this.GetPlayer(direction, x, x, z);

					if(end.Player == Players.None || end.Player != lineState)
					{
						lineState = Players.None;
						break;
					}
				}

				this.UpdatePlayerWinCount(lineState, start.Coordinate, end.Coordinate);

				start = this.GetPlayer(direction, 0, Constants.Dimension - 1, z);
				lineState = start.Player;

				for(var x = 1; x < Constants.Dimension; x++)
				{
					end = this.GetPlayer(direction, x, (Constants.Dimension - 1 - x), z);

					if(end.Player == Players.None || end.Player != lineState)
					{
						lineState = Players.None;
						break;
					}
				}

				this.UpdatePlayerWinCount(lineState, start.Coordinate, end.Coordinate);
			}
		}

		private PlayerAtCoordinate GetPlayer(Direction direction, int x, int y, int z)
		{
			var player = Players.None;
			var coordinate = new Coordinate();

			switch(direction)
			{
				case Direction.X:
					coordinate = new Coordinate(y, z, x);
					break;
				case Direction.Y:
					coordinate = new Coordinate(z, x, y);
					break;
				case Direction.Z:
					coordinate = new Coordinate(x, y, z);
					break;
			}

			player = this.board.GetPlayer(coordinate);
			return new PlayerAtCoordinate(player, coordinate);
		}

		private void UpdatePlayerWinCount(Players lineWinner, Coordinate lineStart, Coordinate lineEnd)
		{
			if(lineWinner != Players.None)
			{
				var winningLine = new Line(lineStart, lineEnd);
				
				if(lineWinner == Players.X)
				{
					this.XCount++;

					if(!this.XWinningLines.Contains(winningLine))
					{
						this.XWinningLines.Add(winningLine);
					}
				}
				else
				{
					this.OCount++;

					if(!this.OWinningLines.Contains(winningLine))
					{
						this.OWinningLines.Add(winningLine);
					}
				}			
			}
			else
			{
				this.noneCount++;
			}
		}

		internal int OCount
		{
			get;
			private set;
		}

		internal List<Line> OWinningLines
		{
			get;
			private set;
		}
		
		internal int XCount
		{
			get;
			private set;
		}

		internal List<Line> XWinningLines
		{
			get;
			private set;
		}

		private struct PlayerAtCoordinate
		{
			internal PlayerAtCoordinate(Players player, Coordinate coordinate)
				: this()
			{
				this.Player = player;
				this.Coordinate = coordinate;
			}

			internal Coordinate Coordinate
			{
				get;
				private set;
			}

			internal Players Player
			{
				get;
				private set;
			}
		}
	}
}
