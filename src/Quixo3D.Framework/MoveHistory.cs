using System;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	[Serializable]
	internal sealed class MoveHistory : Move
	{
		internal MoveHistory(Coordinate source, Coordinate destination, long[] boardState, Players current, List<Line> winningLines)
			 : base(source, destination)
		{
			this.BoardState = boardState;
			this.Current = current;
			this.WinningLines = winningLines;
		}

		internal MoveHistory Clone()
		{
			return new MoveHistory(this.Source, this.Destination, 
				this.BoardState, this.Current, this.WinningLines);
		}
		
		internal long[] BoardState
		{
			get;
			private set;
		}
		
		internal Players Current
		{
			get;
			private set;
		}
		
		internal List<Line> WinningLines
		{
			get;
			private set;
		}
	}
}
