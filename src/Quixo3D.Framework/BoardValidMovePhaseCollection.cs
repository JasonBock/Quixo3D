using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	internal sealed class BoardValidMovePhaseCollection : IEnumerable<ValidMove>
	{
		private Board board;
		private byte[] xValues;
		private byte[] yValues;
		private byte[] zValues;

		internal BoardValidMovePhaseCollection(Board board, byte[] xValues, byte[] yValues, byte[] zValues)
			: base()
		{
			this.board = board;
			this.xValues = xValues;
			this.yValues = yValues;
			this.zValues = zValues;
		}

		public IEnumerator<ValidMove> GetEnumerator()
		{
			for(var x = 0; x < xValues.Length; x++)
			{
				for(var y = 0; y < yValues.Length; y++)
				{
					for(var z = 0; z < zValues.Length; z++)
					{
						var source = new Coordinate(
							xValues[x], yValues[y], zValues[z]);
						var player = this.board.GetPlayer(source);

						if(player == Players.None || player == this.board.Current)
						{
							yield return new ValidMove(source, Board.GetValidDestinations(source));
						}
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}
	}
}
