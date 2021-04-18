using System;
using System.Runtime.Serialization;

namespace Quixo3D.Framework
{
	/// <summary>
	/// The exception that is thrown if a user tries to make an invalid move.
	/// </summary>
	[Serializable]
	public sealed class InvalidMoveException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="InvalidMoveException" /> instance.
		/// </summary>
		public InvalidMoveException() : base()
		{
		}

		private InvalidMoveException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Creates a new <see cref="InvalidMoveException" /> instance
		/// with a specified error message.
		/// </summary>
		/// <param name="message">
		/// The message that describes the error.
		/// </param>
		public InvalidMoveException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Creates a new <see cref="InvalidMoveException" /> instance
		/// with a specified error message and a reference to the inner exception that is the cause of this exception. 
		/// </summary>
		/// <param name="message">
		/// The message that describes the error.
		/// </param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, 
		/// or a null reference if no inner exception is specified. 
		/// </param>
		public InvalidMoveException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
