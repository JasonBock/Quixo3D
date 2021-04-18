using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	/// <summary>
	/// An enumerator for valid moves.
	/// </summary>
	public sealed class BoardValidMoveCollection : IEnumerable<ValidMove>
	{
		private Board board;

		internal BoardValidMoveCollection(Board board)
			: base()
		{
			this.board = board;
		}

		/// <summary>
		/// An enumerator for valid moves.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<ValidMove> GetEnumerator()
		{
			// Get x 0 and 4, y 0 to 4, z 0 to 4
			foreach(ValidMove a in new BoardValidMovePhaseCollection(this.board,
				new byte[] { 0, 4 }, new byte[] { 0, 1, 2, 3, 4 }, new byte[] { 0, 1, 2, 3, 4 }))
			{
				yield return a;
			}

			// Get x 1 to 3, y 0 and 4, z 0 to 4
			foreach(ValidMove b in new BoardValidMovePhaseCollection(this.board,
				new byte[] { 1, 2, 3 }, new byte[] { 0, 4 }, new byte[] { 0, 1, 2, 3, 4 }))
			{
				yield return b;
			}

			// Get x 1 to 3, y 1 to 3, z 0 to 4
			foreach(ValidMove c in new BoardValidMovePhaseCollection(this.board,
				new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }, new byte[] { 0, 4 }))
			{
				yield return c;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
