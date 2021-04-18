using System;
using System.Collections;
using System.Collections.Generic;

namespace Quixo3D.Framework
{
	internal sealed class NonDiagonalLineCollection : IEnumerable<Line>
	{
		private Coordinate target;

		internal NonDiagonalLineCollection(Coordinate target)
			: base()
		{
			this.target = target;
		}

		public IEnumerator<Line> GetEnumerator()
		{
			yield return new Line(new Coordinate(0, this.target.Y, this.target.Z), 
				new Coordinate(Constants.Dimension - 1, this.target.Y, this.target.Z));
			yield return new Line(new Coordinate(this.target.X, 0, this.target.Z),
				new Coordinate(this.target.X, Constants.Dimension - 1, this.target.Z));
			yield return new Line(new Coordinate(this.target.X, this.target.Y, 0),
				new Coordinate(this.target.X, this.target.Y, Constants.Dimension - 1));
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}
	}
}
