using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spackle.Testing;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Quixo3D.Framework.Tests
{
	[TestClass]
	public sealed class LineExceptionTests
		: ExceptionTests<LineException, ArgumentException>
	{
		private const string Message = "What a bad line!";

		public LineExceptionTests()
			: base(LineExceptionTests.Message)
		{
		}

		[TestMethod]
		public void CreateException()
		{
			this.CreateExceptionTest();
		}

		[TestMethod]
		public void CreateExceptionWithMessage()
		{
			this.CreateExceptionWithMessageTest();
		}

		[TestMethod]
		public void CreateExceptionWithMessageAndInnerException()
		{
			this.CreateExceptionWithMessageAndInnerExceptionTest();
		}

		[TestMethod]
		public void RoundtripException()
		{
			this.RoundtripExceptionTest();
		}
	}
}
