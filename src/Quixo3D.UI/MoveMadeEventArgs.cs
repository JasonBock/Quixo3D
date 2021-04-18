using System;
using System.Collections.Generic;
using System.Text;
using Quixo3D.Framework;

namespace Quixo3D.UI
{

    /// <summary>
    /// Provides information about events when moves are made in the game.
    /// </summary>
    public class MoveMadeEventArgs : EventArgs
    {
        private Players player;
        private Move move;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="move"></param>
        public MoveMadeEventArgs(Players player, Move move)
        {
            this.player = player;
            this.move = move;
        }

        /// <summary>
        /// Gets the player associated with the move.
        /// </summary>
        public Players Player
        {
            get { return player; }
        }

        /// <summary>
        /// Gets the actual <see cref="Quixo3D.Framework.Move"/> object.
        /// </summary>
        public Move Move
        {
            get { return move; }
        }
    }
}
