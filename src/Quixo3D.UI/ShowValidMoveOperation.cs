using System;
using System.Collections.Generic;
using System.Text;
using Quixo3D.Framework;
using System.Collections.ObjectModel;

namespace Quixo3D.UI
{
    /// <summary>
    /// Encapsulates the behavior of a the user interface action where valid move
    /// pieces are shown.
    /// </summary>
    public class ShowValidMoveOperation
    {
        GamePieceVisual sourcePiece;
        GameBoardController controller;
        ValidMove validMove;
        Collection<GamePieceVisual> validDestinationPieces;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="sourcePiece"></param>
        public ShowValidMoveOperation(GameBoardController controller, GamePieceVisual sourcePiece)
        {
            this.sourcePiece = sourcePiece;
            this.controller = controller;
        }

        /// <summary>
        /// Gets the <see cref="GamePieceVisual"/> that is the source of the valid move.
        /// </summary>
        public GamePieceVisual SourcePiece
        {
            get { return sourcePiece; }
        }

        ValidMove ValidMove
        {
            get
            {
                if(validMove == null)
                {
                    Coordinate source = controller.GetGamePieceCoordinate(sourcePiece);
                    ReadOnlyCollection<ValidMove> validMoves = controller.Board.GetValidMoves();
                    foreach(ValidMove move in validMoves)
                    {
                        if(move.Source == source)
                        {
                            validMove = move;
                        }
                    }
                }
               return validMove;
            }
        }

        /// <summary>
        /// Gets a collection of the destination <see cref="GamePieceVisual"/> objects that are valid destinations.
        /// </summary>
        public Collection<GamePieceVisual> ValidDestinationPieces
        {
            get
            {
                if(validDestinationPieces == null)
                {
                    validDestinationPieces = new Collection<GamePieceVisual>();
                    foreach (Coordinate destination in ValidMove.Destinations){
                        GamePieceVisual piece = controller.GetCoordinateGamePiece(destination);
                        validDestinationPieces.Add(piece);
                    }
                }
                return validDestinationPieces;
            }
        }

        /// <summary>
        /// Displays the valid source and its destinations as highlighted game pieces.
        /// </summary>
        public void Show()
        {
            sourcePiece.HighlightedAsValidSource = true;
            foreach(Coordinate destination in ValidMove.Destinations)
            {
                GamePieceVisual piece = controller.GetCoordinateGamePiece(destination);
                piece.HighlightedAsValidDestination = true;
            }
        }

        /// <summary>
        /// Returns the valid source and its destinations to a normal, unhighlighted state.
        /// </summary>
        public void Hide()
        {
            sourcePiece.Reset();
            foreach(Coordinate destination in ValidMove.Destinations)
            {
                GamePieceVisual piece = controller.GetCoordinateGamePiece(destination);
                piece.Reset();
            }

        }
    }
}
