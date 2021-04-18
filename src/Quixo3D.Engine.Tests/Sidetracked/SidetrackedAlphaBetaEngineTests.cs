using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quixo3D.Engine;
using Quixo3D.Engine.Sidetracked;
using Quixo3D.Framework;
using Quixo3D.Framework.Tests;
using System;
using System.Globalization;

namespace Quixo3D.Engine.Tests.Sidetracked
{
	[TestClass]
	public sealed class SidetrackedAlphaBetaEngineTests : EngineTests
	{
		[TestMethod]
		public void GenerateMoveForSimpleGame()
		{
			var board = EngineTests.GenerateSimpleGame();

			var engine = new SidetrackedAlphaBetaEngine();
			var generatedMove = engine.GenerateMove(board.Clone());
			this.TestContext.WriteLine(generatedMove.ToString());
			this.TestContext.WriteLine("Evaluations: " +
				engine.Evaluations.ToString(CultureInfo.CurrentCulture));
			this.TestContext.WriteLine("Visitations: " +
				engine.Visitations.ToString(CultureInfo.CurrentCulture));
			board.MovePiece(generatedMove);
		}

		[TestMethod]
		public void GenerateMoveForComplexGame()
		{
			var board = EngineTests.GenerateComplexGame();

			var engine = new SidetrackedAlphaBetaEngine();
			var generatedMove = engine.GenerateMove(board.Clone());
			this.TestContext.WriteLine(generatedMove.ToString());
			this.TestContext.WriteLine("Evaluations: " + 
				engine.Evaluations.ToString(CultureInfo.CurrentCulture));
			this.TestContext.WriteLine("Visitations: " +
				engine.Visitations.ToString(CultureInfo.CurrentCulture));
			board.MovePiece(generatedMove);
		}
	}
}
