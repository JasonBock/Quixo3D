using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Quixo3D.UI {

    /// <summary>
    /// Represents an error that occurred in the game.
    /// </summary>
    [Serializable]
    public class GameException : Exception {

        /// <summary>
        /// Creates a new, empty exception.
        /// </summary>
        public GameException() : base() { }

        /// <summary>
        /// Creates a new exception with a message.
        /// </summary>
        /// <param name="message"></param>
        public GameException(string message) : base(message) { }

        /// <summary>
        /// Creates a new exception with a message and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public GameException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates a new exception with the specified SerializationInfo and Streaming Context.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected GameException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
