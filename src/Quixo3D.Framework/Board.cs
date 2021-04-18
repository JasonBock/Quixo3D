using Spackle.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Represents a Quixo 3D board.
	/// </summary>
	[Serializable]
	public sealed class Board
	{
		private const string ErrorIdenticalPiece = "The source and destination locations are the same.";
		private const string ErrorInvalidSourcePiece = "The player {0} cannot move the piece at position {1}.";
		private const string ErrorInvalidDestinationPosition = "The player {0} cannot move a piece from {1} to {2}.";
		private const string ErrorMoveCausesOpponentToWin = "The current move, {0} to {1}, would allow the opponent to win.";
		private const string ErrorOutOfRange = "Position \\{{0}, {1}\\} is out of range.";
		private const string ErrorWinner = "The game has been won by {0} - no more moves can be made.";

		private delegate void AdjustLoopOperator(ref int sweep);
		private delegate bool CheckLoopOperator(int sweep, int checkPoint);
		private delegate int NextPieceOperator(int position);

		private long[] layers = new long[Constants.Dimension];
		private List<MoveHistory> moveHistory = new List<MoveHistory>();
		private List<Line> winningLines = new List<Line>();

		/// <summary>
		/// Creates a new <see cref="Board" /> instance.
		/// </summary>
		public Board()
			: base()
		{
			this.Current = Players.X;
			this.Winner = Players.None;
		}

		private void CheckMoveCoordinates(Coordinate source, Coordinate destination)
		{
			if(this.Winner != Players.None)
			{
				throw new InvalidMoveException(string.Format(CultureInfo.CurrentCulture,
					Board.ErrorWinner, this.Winner));
			}

			if(source == destination)
			{
				throw new InvalidMoveException(Board.ErrorIdenticalPiece);
			}

			if(source.Location == CoordinateLocation.Middle)
			{
				throw new InvalidMoveException(string.Format(CultureInfo.CurrentCulture,
					Board.ErrorInvalidSourcePiece, this.Current, source));
			}

			ValidMove foundValidMove = null;
			
			foreach(var validMove in this.GetValidMoveEnumerator())
			{
				if(validMove.Source == source)
				{
					foundValidMove = validMove;
					break;
				}
			}
			
			if(foundValidMove == null)
			{
				throw new InvalidMoveException(string.Format(CultureInfo.CurrentCulture,
					Board.ErrorInvalidSourcePiece, this.Current, source));
			}
			else if(!foundValidMove.Destinations.Contains(destination))
			{
				throw new InvalidMoveException(string.Format(CultureInfo.CurrentCulture,
					Board.ErrorInvalidDestinationPosition, this.Current, source, destination));
			}
		}

		/// <summary>
		/// Creates a new <see cref="Board" /> that is a copy of the current instance. 
		/// </summary>
		/// <returns>A cloned <see cref="Board" />.</returns>
		public Board Clone()
		{
			var clone = new Board();

			clone.Current = this.Current;
			clone.Winner = this.Winner;
			clone.moveHistory = this.moveHistory.ConvertAll<MoveHistory>((move) =>
			{
				return move.Clone();
			});
			clone.layers = (long[])this.layers.Clone();

			return clone;
		}

		private static void Decrement(ref int sweep)
		{
			sweep--;
		}

		private static int GetShiftOut(int x, int y)
		{
			return 10 * x + 2 * y;
		}

		/// <summary>
		/// Returns the <see cref="Players"/> at the specified co-ordinates.
		/// </summary>
		/// <param name="coordinate">The location of the desired <see cref="Players" />.</param>
		/// <returns>
		/// The <see cref="Players"/> object at the requested position.
		/// </returns>
		public Players GetPlayer(Coordinate coordinate)
		{
			var shiftOut = Board.GetShiftOut(coordinate.X, coordinate.Y);
			return (Players)((this.layers[coordinate.Z] >> shiftOut) & 3L);
		}

		/// <summary>
		/// Gets all of the valid moves for the current player.
		/// </summary>
		/// <returns>A read-only collection of <see cref="ValidMove" />.</returns>
		public ReadOnlyCollection<ValidMove> GetValidMoves()
		{
			return this.GetMoves().AsReadOnly();
		}

		/// <summary>
		/// Gets an enumerator to iterate over the valid moves.
		/// </summary>
		/// <returns>An enumerator of valid moves.</returns>
		public BoardValidMoveCollection GetValidMoveEnumerator()
		{
			return new BoardValidMoveCollection(this);
		}

		private List<ValidMove> GetValidMoves(byte[] xValues, byte[] yValues, byte[] zValues)
		{
			var validMoves = new List<ValidMove>();

			for(var x = 0; x < xValues.Length; x++)
			{
				for(var y = 0; y < yValues.Length; y++)
				{
					for(var z = 0; z < zValues.Length; z++)
					{
						var source = new Coordinate(xValues[x], yValues[y], zValues[z]);
						var player = this.GetPlayer(source);

						if(player == Players.None || player == this.Current)
						{
							validMoves.Add(new ValidMove(
								source, Board.GetValidDestinations(source)));
						}
					}
				}
			}

			return validMoves;
		}

		private List<ValidMove> GetMoves()
		{
			var validMoves = new List<ValidMove>();

			// Get x 0 and 4, y 0 to 4, z 0 to 4
			validMoves.AddRange(this.GetValidMoves(
				new byte[] { 0, 4 }, new byte[] { 0, 1, 2, 3, 4 }, new byte[] { 0, 1, 2, 3, 4 }));
			// Get x 1 to 3, y 0 and 4, z 0 to 4
			validMoves.AddRange(this.GetValidMoves(
				new byte[] { 1, 2, 3 }, new byte[] { 0, 4 }, new byte[] { 0, 1, 2, 3, 4 }));
			// Get x 1 to 3, y 1 to 3, z 0 to 4
			validMoves.AddRange(this.GetValidMoves(
				new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }, new byte[] { 0, 4 }));

			return validMoves;
		}

		internal static List<Coordinate> GetValidDestinations(Coordinate source)
		{
			var destinations = new List<Coordinate>();

			if(source.X == 0 || source.X == (Constants.Dimension - 1))
			{
				destinations.Add(new Coordinate((byte)Math.Abs(source.X - (Constants.Dimension - 1)),
					source.Y, source.Z));
			}
			else
			{
				destinations.Add(new Coordinate(0, source.Y, source.Z));
				destinations.Add(new Coordinate((Constants.Dimension - 1), source.Y, source.Z));
			}

			if(source.Y == 0 || source.Y == (Constants.Dimension - 1))
			{
				destinations.Add(new Coordinate(source.X,
					(byte)Math.Abs(source.Y - (Constants.Dimension - 1)), source.Z));
			}
			else
			{
				destinations.Add(new Coordinate(source.X, 0, source.Z));
				destinations.Add(new Coordinate(source.X, (Constants.Dimension - 1), source.Z));
			}

			if(source.Z == 0 || source.Z == (Constants.Dimension - 1))
			{
				destinations.Add(new Coordinate(source.X,
					source.Y, (byte)Math.Abs(source.Z - (Constants.Dimension - 1))));
			}
			else
			{
				destinations.Add(new Coordinate(source.X, source.Y, 0));
				destinations.Add(new Coordinate(source.X, source.Y, (Constants.Dimension - 1)));
			}

			return destinations;
		}

		private static bool IsLessThan(int sweep, int checkPoint)
		{
			return sweep < checkPoint;
		}

		private static bool IsGreaterThan(int sweep, int checkPoint)
		{
			return sweep > checkPoint;
		}

		private static void Increment(ref int sweep)
		{
			sweep++;
		}

		/// <summary>
		/// Moves the piece from a source to a destination, specified by the given <see cref="Move" />.
		/// </summary>
		/// <param name="move">The desired move.</param>
		/// <returns>A list of <see cref="Coordinate" />s that represent the positions affected by the move.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="move"/> is null.</exception>
		/// <exception cref="InvalidMoveException">Thrown if the given move is invalid.</exception>
		public ReadOnlyCollection<Coordinate> MovePiece(Move move)
		{
			move.CheckParameterForNull("move");
			return this.MovePiece(move.Source, move.Destination);
		}

		/// <summary>
		/// Moves the piece from <paramref name="source"/> to <paramref name="destination"/>.
		/// </summary>
		/// <param name="source">The source piece.</param>
		/// <param name="destination">The destination piece.</param>
		/// <returns>A list of <see cref="Coordinate" />s that represent the positions affected by the move.</returns>
		/// <exception cref="InvalidMoveException">Thrown if <paramref name="source"/> cannot be moved
		/// by the current player, or if <paramref name="destination"/> is invalid</exception>
		public ReadOnlyCollection<Coordinate> MovePiece(Coordinate source, Coordinate destination)
		{
			var currentPlayer = this.Current;
			var currentLayers = new long[Constants.Dimension];
			var currentWinningLines = this.winningLines;
			Array.Copy(this.layers, currentLayers, this.layers.Length);

			this.CheckMoveCoordinates(source, destination);
			var updated = this.Update(source, destination).AsReadOnly();

			var wins = new WinningLines(this);

			if((this.Current == Players.X && wins.XCount > 0) ||
				(this.Current == Players.O && wins.XCount > 0))
			{
				this.Winner = Players.X;
				this.winningLines = wins.XWinningLines;
			}
			else if((this.Current == Players.O && wins.OCount > 0) ||
				(this.Current == Players.X && wins.OCount > 0))
			{
				this.Winner = Players.O;
				this.winningLines = wins.OWinningLines;
			}

			if(this.Winner != Players.None)
			{
				this.Current = Players.None;
			}
			else
			{
				if(this.Current == Players.X)
				{
					this.Current = Players.O;
				}
				else
				{
					this.Current = Players.X;
				}
			}

			this.moveHistory.Add(new MoveHistory(source, destination, currentLayers, currentPlayer, currentWinningLines));
			return updated;
		}

		private static int NextPieceBack(int position)
		{
			return --position;
		}

		private static int NextPieceForward(int position)
		{
			return ++position;
		}

		private void SetPlayer(Coordinate position, Players newValue)
		{
			var shiftOut = Board.GetShiftOut(position.X, position.Y);
			this.layers[position.Z] &= ~((long)3 << shiftOut);

			if(newValue != Players.None)
			{
				this.layers[position.Z] |= ((long)newValue << shiftOut);
			}
		}

		/// <summary>
		/// Reverts the last move.
		/// </summary>
		public void Undo()
		{
			if(this.moveHistory.Count >= 1)
			{
				var lastMove = this.moveHistory[this.moveHistory.Count - 1];
				this.moveHistory.RemoveAt(this.moveHistory.Count - 1);
				this.layers = lastMove.BoardState;
				this.Current = lastMove.Current;
				this.winningLines = lastMove.WinningLines;
				this.Winner = Players.None;
			}
		}

		/// <summary>
		/// Performs a number of <see cref="Board.Undo()" /> operations.
		/// </summary>
		/// <param name="count">The number of moves to undo.</param>
		/// <remarks>
		/// If <paramref name="count"/> is greater than the number of moves made in the game,
		/// the board resets itself to the beginning.
		/// </remarks>
		public void Undo(uint count)
		{
			if(count >= this.moveHistory.Count)
			{
				this.moveHistory.Clear();
				this.layers = new long[Constants.Dimension];
				this.Current = Players.X;
				this.Winner = Players.None;
			}
			else
			{
				for(uint i = 0; i < count; i++)
				{
					this.Undo();
				}
			}
		}

		private List<Coordinate> Update(Coordinate source, Coordinate destination)
		{
			var updated = new List<Coordinate>();
			
			int sweep = 0, startPoint = 0, endPoint = 0;
			bool isXFixed = (source.X == destination.X) ? true : false;
			bool isYFixed = (source.Y == destination.Y) ? true : false;

			AdjustLoopOperator loopOp;
			CheckLoopOperator checkOp;
			NextPieceOperator nextPieceOp;

			if(source.X > destination.X || source.Y > destination.Y || source.Z > destination.Z)
			{
				loopOp = Board.Decrement;
				checkOp = Board.IsGreaterThan;
				nextPieceOp = Board.NextPieceBack;
			}
			else
			{
				loopOp = Board.Increment;
				checkOp = Board.IsLessThan;
				nextPieceOp = Board.NextPieceForward;
			}

			startPoint = !isXFixed ? source.X : (!isYFixed ? source.Y : source.Z);
			endPoint = !isXFixed ? destination.X : (!isYFixed ? destination.Y : destination.Z);

			for(sweep = startPoint; checkOp(sweep, endPoint); loopOp(ref sweep))
			{
				Coordinate positionToUpdate;
				
				if(!isXFixed)
				{
					positionToUpdate = new Coordinate(sweep, destination.Y, destination.Z);	
					this.SetPlayer(positionToUpdate,
						this.GetPlayer(new Coordinate(nextPieceOp(sweep), destination.Y, destination.Z)));
				}
				else if(!isYFixed)
				{
					positionToUpdate = new Coordinate(destination.X, sweep, destination.Z);
					this.SetPlayer(positionToUpdate, 
						this.GetPlayer(new Coordinate(destination.X, nextPieceOp(sweep), destination.Z)));
				}
				else
				{
					positionToUpdate = new Coordinate(destination.X, destination.Y, sweep);
					this.SetPlayer(new Coordinate(destination.X, destination.Y, sweep),
						this.GetPlayer(new Coordinate(destination.X, destination.Y, nextPieceOp(sweep))));
				}
				
				updated.Add(positionToUpdate);
			}

			this.SetPlayer(destination, this.Current);
			updated.Add(destination);
			
			return updated;
		}

		/// <summary>
		/// Gets a read-only collection of the current moves in the game.
		/// </summary>
		public ReadOnlyCollection<Move> MoveHistory
		{
			get
			{
				return this.moveHistory.ConvertAll<Move>((target) =>
					{
						return new Move(target.Source, target.Destination);
					}).AsReadOnly();
			}
		}
		
		/// <summary>
		/// Gets all of the winning lines for the current board.
		/// </summary>
		/// <remarks>
		/// The count will be zero if <see cref="Winner"/> is equal to <see cref="Players.None" />;
		/// otherwise, the count will be greater than zero.
		/// </remarks>
		public ReadOnlyCollection<Line> WinningLines
		{
			get
			{
				return this.winningLines.AsReadOnly();
			}
		}

		/// <summary>
		/// Gets the current player.
		/// </summary>
		public Players Current
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the winning player (if one exists).
		/// </summary>
		public Players Winner
		{
			get;
			private set;
		}
	}
}
