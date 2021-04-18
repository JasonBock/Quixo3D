using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	/// <summary>
	/// Finds all of the lines for a given coordinate.
	/// </summary>
	public sealed class ValidLineCollection : IEnumerable<Line>
	{
		/// <summary>
		/// Creates a new <see cref="ValidLineCollection" /> instance.
		/// </summary>
		/// <param name="target">The coordinate to find the lines for.</param>
		public ValidLineCollection(Coordinate target)
			: base()
		{
			this.Target = target;
		}

		/// <summary>
		/// Gets an enumeration of <see cref="Line" /> instances for the given target.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Line> GetEnumerator()
		{
			var location = this.Target.Location;

			foreach(var line in new NonDiagonalLineCollection(this.Target))
			{
				yield return line;
			}

			foreach(var diagonalLine in new DiagonalLineCollection(this.Target))
			{
				yield return diagonalLine;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		
		/// <summary>
		/// Gets the target that lines are produced for.
		/// </summary>
		public Coordinate Target
		{
			get;
			private set;
		}
	}
}
