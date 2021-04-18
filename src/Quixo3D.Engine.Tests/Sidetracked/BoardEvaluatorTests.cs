using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Engine.Sidetracked;
using Quixo3D.Framework;
using Quixo3D.Framework.Tests;
using System;
using System.Globalization;

namespace Quixo3D.Engine.Tests.Sidetracked
{
	[TestClass]
	public sealed class BoardEvaluatorTests : EngineTests
	{
		[TestMethod]
		public void Evaluate()
		{
			var board = EngineTests.GenerateGameWithEvenPosition();

			var evaluation = BoardEvaluator.Evaluate(board);
			this.TestContext.WriteLine("Evaluation = " +
				evaluation.ToString(CultureInfo.CurrentCulture));
			Assert.IsTrue(evaluation > 0); // 1
		}

		[TestMethod]
		public void EvaluateWeakPosition()
		{
			var board = EngineTests.GenerateGameWithPlayerAboutToLose();

			var evaluation = BoardEvaluator.Evaluate(board);
			this.TestContext.WriteLine("Evaluation = " +
				evaluation.ToString(CultureInfo.CurrentCulture));
			Assert.IsTrue(evaluation < 0); // -116
		}
		
		[TestMethod]
		public void EvaluateComplexGame()
		{
			var board = EngineTests.GenerateComplexGame();

			var evaluation = BoardEvaluator.Evaluate(board);
			this.TestContext.WriteLine("Evaluation = " +
				evaluation.ToString(CultureInfo.CurrentCulture));
			Assert.IsTrue(evaluation < 0); // -3
		}
	}
}
