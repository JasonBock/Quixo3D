using Quixo3D.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Engine.Sidetracked
{
	internal sealed class ValidMovesAsMovesCollection : IEnumerable<Move>
	{
		private IEnumerable<ValidMove> validMoves;
		
		internal ValidMovesAsMovesCollection(IEnumerable<ValidMove> validMoves)
			: base()
		{
			this.validMoves = validMoves;
		}
		
		public IEnumerator<Move> GetEnumerator()
		{
			foreach(var validMove in this.validMoves)
			{
				var source = validMove.Source;

				foreach(var destination in validMove.Destinations)
				{
					yield return new Move(source, destination);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
