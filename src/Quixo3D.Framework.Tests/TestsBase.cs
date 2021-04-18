using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spackle.Testing;
using System;
using System.IO;

namespace Quixo3D.Framework.Tests
{
	/// <summary>
	/// Defines a base test class that provides common functionality.
	/// </summary>
	[TestClass]
	public abstract class TestsBase : CoreTests
	{		
		protected static void SaveBoard(Board board, string file)
		{
			BoardFormatter formatter = new BoardFormatter();

			using(Stream stream = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
			{
				formatter.Serialize(stream, board);
			}		
		}
	}
}
