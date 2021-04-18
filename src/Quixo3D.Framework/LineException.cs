using System;
using System.Runtime.Serialization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// The exception that is thrown if a user tries to make an invalid <see cref="Line" />.
	/// </summary>
	[Serializable]
	public sealed class LineException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="LineException" /> instance.
		/// </summary>
		public LineException() : base()
		{
		}

		private LineException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Creates a new <see cref="LineException" /> instance
		/// with a specified error message.
		/// </summary>
		/// <param name="message">
		/// The message that describes the error.
		/// </param>
		public LineException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Creates a new <see cref="LineException" /> instance
		/// with a specified error message and a reference to the inner exception that is the cause of this exception. 
		/// </summary>
		/// <param name="message">
		/// The message that describes the error.
		/// </param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, 
		/// or a null reference if no inner exception is specified. 
		/// </param>
		public LineException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
