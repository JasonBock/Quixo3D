using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Quixo3D.Framework;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace Quixo3D.UI
{
    /// <summary>
    /// Encapsulates the logic and behavior of a Quixo3D game move in the user interface.
    /// </summary>
    public class ExecuteMoveOperation
    {

        GameBoardController controller;
        GamePieceVisual sourcePiece;
        GamePieceVisual destinationPiece;
        Players player = Players.None;
        Move move;
        ReadOnlyCollection<Coordinate> affectedCoordinates;
        bool executed;

        /// <summary>
        /// Creates a new instance of a <see cref="ExecuteMoveOperation"/>.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="sourcePiece"></param>
        /// <param name="destinationPiece"></param>
        public ExecuteMoveOperation(GameBoardController controller, GamePieceVisual sourcePiece, GamePieceVisual destinationPiece)
        {
            this.controller = controller;
            this.sourcePiece = sourcePiece;
            this.destinationPiece = destinationPiece;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="ExecuteMoveOperation"/>.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="move"></param>
        public ExecuteMoveOperation(GameBoardController controller, Move move)
        {
            this.controller = controller;
            this.move = move;
        }

        /// <summary>
        /// Gets the <see cref="Quixo3D.Framework.Player"/> that made the move.
        /// </summary>
        public Players Player
        {
            get { return this.player; }
        }

        /// <summary>
        /// Gets the <see cref="Quixo3D.Framework.Move"/> that this operation corresponds to.
        /// </summary>
        public Move Move
        {
            get { return move; }
        }

        /// <summary>
        /// Gets the collection of <see cref="Quixo3D.Framework.Coordinate"/> objects that 
        /// were affected by the move.
        /// </summary>
        public ReadOnlyCollection<Coordinate> AffectedCoordinates
        {
            get { return affectedCoordinates; }
        }

        /// <summary>
        /// Executes a move in the Quixo3D game.
        /// </summary>
        public void Execute()
        {
            // do not execute more than once.
            if(executed)
            {
                return;
            }

            player = controller.Board.Current;

            if(sourcePiece != null && destinationPiece != null && move == null)
            {

                Coordinate source = controller.GetGamePieceCoordinate(sourcePiece);
                Coordinate destination = controller.GetGamePieceCoordinate(destinationPiece);
                move = new Move(source, destination);
            }

            if(move != null)
            {
                affectedCoordinates = controller.Board.MovePiece(move);
            }
            executed = true;
        }

    }
}
