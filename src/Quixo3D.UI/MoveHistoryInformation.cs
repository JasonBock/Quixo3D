using System;
using System.Collections.Generic;
using System.Text;
using Quixo3D.Framework;

namespace Quixo3D.UI
{
    /// <summary>
    /// Encapsulates simple data about a move in the game.
    /// </summary>
    public class MoveHistoryInformation
    {
        string source;
        string destination;
        string player;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="move"></param>
        public MoveHistoryInformation(Players player, Move move)
        {
            if(move == null)
            {
                throw new ArgumentNullException("move", "Move cannot be a null value.");
            }

            string format = "{0}, {1}, {2}";
            source = string.Format(format, move.Source.X.ToString(), move.Source.Y.ToString(), move.Source.Z.ToString());
            destination = string.Format(format, move.Destination.X.ToString(), move.Destination.Y.ToString(), move.Destination.Z.ToString());

            if(player == Players.X)
            {
                this.player = "X";
            }
            else if(player == Players.O)
            {
                this.player = "O";
            }
            else
            {
                this.player = "?";
            }
        }

        /// <summary>
        /// Gets a string description of the move's source.
        /// </summary>
        public string Source
        {
            get { return source; }
        }

        /// <summary>
        /// Gets a string description of the move's destination.
        /// </summary>
        public string Destination
        {
            get { return destination; }
        }

        /// <summary>
        /// Gets a string description of the player that performed the move.
        /// </summary>
        public string PlayerText
        {
            get
            {
                return player;
            }
        }
            
    }
}
