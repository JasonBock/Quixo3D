using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Quixo3D.Framework;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Diagnostics;
using Quixo3D.Engine;

namespace Quixo3D.UI
{

    /// <summary>
    /// Manages the interaction between the UI window and the <see cref="Quixo3D.Framework.Board">Board</see>.
    /// </summary>
    public class GameBoardController
    {

        Board board;
        Hashtable coordinatePieceMap;
        ModelVisual3D parentVisual;
        TurnState turnState;
        GamePieceVisual pendingSourcePiece = null;
        IEngine playerX;
        IEngine playerO;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        public GameBoardController(ModelVisual3D parentVisual)
        {

            this.parentVisual = parentVisual;
            this.turnState = TurnState.Idle;

            board = new Board();
            coordinatePieceMap = new Hashtable();
            InitializeMap();

            playerO = new RandomEngine();

            BuildBoardFromMoveHistory();
        }

       
        public Board Board
        {
            get { return board; }
        }

        void InitializeMap()
        {
            coordinatePieceMap.Clear();
            int maxDimension = Constants.Dimension;
            for(int x = 0;x < maxDimension;x++)
            {
                for(int y = 0;y < maxDimension;y++)
                {
                    for(int z = 0;z < maxDimension;z++)
                    {
                        Coordinate coordinate = new Coordinate(
                            Convert.ToByte(x),
                            Convert.ToByte(y),
                            Convert.ToByte(z));
                        GamePieceVisual gamePiece = new GamePieceVisual();
                        gamePiece.PieceType = PieceType.Empty;
                        gamePiece.PieceState = PieceState.Nothing;

                        TranslateTransform3D transform = new TranslateTransform3D(
    Convert.ToDouble(x) * ConfigSettings.Instance.CellSpacing,
    Convert.ToDouble(y) * ConfigSettings.Instance.CellSpacing,
    Convert.ToDouble(z) * ConfigSettings.Instance.CellSpacing);
                        gamePiece.Transform = transform;


                        coordinatePieceMap.Add(coordinate, gamePiece);
                    }
                }

            }
        }

        ModelVisual3D GetPieceContainer()
        {
            ModelVisual3D container;
            if(parentVisual.Children.Count > 0 && parentVisual.Children[0] != null)
            {
                container = parentVisual.Children[0] as ModelVisual3D;
            }
            else
            {
                container = new ModelVisual3D();
                parentVisual.Children.Add(container);
            }

            return container;

        }      

        void BuildBoardFromMoveHistory()
        {
            bool isX = true;

            ModelVisual3D container = GetPieceContainer();
            container.Children.Clear();
            GamePieceVisual gamePiece;

            //reset everything
            foreach(Coordinate key in coordinatePieceMap.Keys)
            {
                gamePiece = (GamePieceVisual)coordinatePieceMap[key];

                Player player = board.GetPlayer(key);

                switch(player)
                {
                    case Player.None:
                        gamePiece.PieceType = PieceType.Empty;
                        gamePiece.PieceState = PieceState.Nothing;
                        break;
                    case Player.X:
                        gamePiece.PieceType = PieceType.X;
                        gamePiece.PieceState = PieceState.Placed;
                        break;
                    case Player.O:
                        gamePiece.PieceType = PieceType.O;
                        gamePiece.PieceState = PieceState.Placed;
                        break;
                }

                //gamePiece.PieceType = PieceType.Empty;
                container.Children.Add(gamePiece);
            }

            double centerMovement = ConfigSettings.Instance.CellSpacing * 2 * -1;
            TranslateTransform3D centering = new TranslateTransform3D(centerMovement, centerMovement, centerMovement);
            container.Transform = centering;

            //change pieces at coordinates that have been
            //moved to.
            foreach(Move move in board.MoveHistory)
            {
                Coordinate destination = move.Destination;
                gamePiece = (GamePieceVisual)coordinatePieceMap[destination];
                gamePiece.PieceState = PieceState.Placed;
                if(isX)
                {
                    gamePiece.PieceType = PieceType.X;
                }
                else
                {
                    gamePiece.PieceType = PieceType.O;
                }
                isX = isX != true;

            }

            ShowValidSources();
            RedrawAllPieces();
        }

        void RedrawAllPieces()
        {
            //redraw everything
            foreach(Coordinate key in coordinatePieceMap.Keys)
            {
                GamePieceVisual gamePiece = (GamePieceVisual)coordinatePieceMap[key];
                gamePiece.Redraw();
            }
        }

        public void ShowPlacedPieces()
        {
            GamePieceVisual gamePiece;
            foreach(Coordinate key in coordinatePieceMap.Keys)
            {
                gamePiece = (GamePieceVisual)coordinatePieceMap[key];

                if(gamePiece.PieceType == PieceType.Empty)
                {
                    gamePiece.PieceState = PieceState.Nothing;
                }
                else
                {
                    gamePiece.PieceState = PieceState.Placed;
                }
            }

            RedrawAllPieces();

        }

        void GreyOutAllPieces()
        {
            GamePieceVisual gamePiece;
            foreach(Coordinate key in coordinatePieceMap.Keys)
            {
                gamePiece = (GamePieceVisual)coordinatePieceMap[key];
                gamePiece.PieceType = PieceType.Empty;
                gamePiece.PieceState = PieceState.Nothing;
            }

        }

        public void ShowValidSources()
        {
            pendingSourcePiece = null;

            GamePieceVisual gamePiece;
            ReadOnlyCollection<ValidMove> validMoves = board.GetValidMoves();
            foreach(ValidMove move in validMoves)
            {
                Coordinate source = move.Source;
                gamePiece = (GamePieceVisual)coordinatePieceMap[source];
                gamePiece.PieceState = PieceState.ValidSource;
                gamePiece.Highlighted = false;
                gamePiece.Redraw();
            }

            RedrawAllPieces();
            turnState = TurnState.Idle;

        }

        public void ShowValidMovePieces(ValidMove validMove)
        {
            Coordinate sourceCoordinate = validMove.Source;
            GamePieceVisual sourcePiece = coordinatePieceMap[sourceCoordinate] as GamePieceVisual;
            sourcePiece.PieceState = PieceState.ValidSource;
            sourcePiece.Highlighted = true;
            sourcePiece.Redraw();

            foreach(Coordinate destinationCoordinate in validMove.Destinations)
            {
                GamePieceVisual destinationPiece = coordinatePieceMap[destinationCoordinate] as GamePieceVisual;
                destinationPiece.PieceState = PieceState.ValidDestination;
                destinationPiece.Redraw();
            }
        }

        public void CancelCurrentSelection()
        {
            ShowValidSources();
            RedrawAllPieces();


            turnState = TurnState.Idle;
        }

        void ShowDestinations(GamePieceVisual gamePiece)
        {
            pendingSourcePiece = gamePiece;
            ValidMove targetMove = null;
            ReadOnlyCollection<ValidMove> moves = board.GetValidMoves();
            foreach(ValidMove move in moves)
            {
                if(coordinatePieceMap[move.Source] == gamePiece)
                {
                    targetMove = move;
                    break;
                }
            }

            ShowValidMovePieces(targetMove);
            //RedrawAllPieces();
            turnState = TurnState.ShowingDestinations;

        }

        public GamePieceVisualInfo GetPieceInfo(Viewport3D viewport, Point location)
        {
            ModelVisual3D selectedItem = GetHitTestResult(viewport, location);

            if(selectedItem == null)
            {
                return null;
            }

            if(selectedItem is GamePieceVisual)
            {
                GamePieceVisual gamePiece = selectedItem as GamePieceVisual;
                GamePieceVisualInfo info = gamePiece.GetInfoObject(this);

                return info;
            }

            return null;

        }
        
        public void ExecuteHitTest(Viewport3D viewport, Point location)
        {
            ModelVisual3D selectedItem = GetHitTestResult(viewport, location);

            if(selectedItem == null)
            {
                return;
            }

            if(selectedItem is GamePieceVisual)
            {

                GamePieceVisual gamePiece = selectedItem as GamePieceVisual;

                if(gamePiece.PieceState == PieceState.ValidSource)
                {
                    if(turnState == TurnState.Idle)
                    {
                        ShowDestinations(gamePiece);
                    }
                    else if(turnState == TurnState.ShowingDestinations)
                    {
                        //turn off destinations, go back to idle
                        ShowValidSources();
                    }
                }
                else if(gamePiece.PieceState == PieceState.ValidDestination)
                {
                    if(turnState == TurnState.ShowingDestinations)
                    {
                        //execute  move
                        if(pendingSourcePiece == null)
                        {
                            throw new GameException("There was no currently selected source piece.");
                        }

                        ExecuteMove(pendingSourcePiece, gamePiece);
                    }

                }
            }
        }

        void OnMoveMade(Player player, Move move)
        {
            if(MoveMade != null)
            {
                MoveMadeEventArgs e = new MoveMadeEventArgs(player, move);
                MoveMade(this, e);
            }
        }

        void ExecuteMove(GamePieceVisual sourcePiece, GamePieceVisual destinationPiece)
        {
            Coordinate source = GetCoordinateFromGamePiece(sourcePiece);
            Coordinate destination = GetCoordinateFromGamePiece(destinationPiece);

            Move newMove = new Move(source, destination);
            Player currentPlayer = board.Current;
            board.MovePiece(newMove);
            OnMoveMade(currentPlayer, newMove);

            BuildBoardFromMoveHistory();

            CheckForMoveGeneration();

        }

        internal Coordinate GetCoordinateFromGamePiece(GamePieceVisual gamePiece)
        {
            foreach(Coordinate key in coordinatePieceMap.Keys)
            {
                if(coordinatePieceMap[key] == gamePiece)
                {
                    return key;
                }
            }

            throw new GameException("Coordinate could not be found from game piece.");
        }

        ModelVisual3D GetHitTestResult(Viewport3D viewport, Point location)
        {

            HitTestResult result = VisualTreeHelper.HitTest(viewport, location);
            if(result != null && result.VisualHit is ModelVisual3D)
            {
                ModelVisual3D modelVisual = (ModelVisual3D)result.VisualHit;
                return modelVisual;
            }

            return null;
        }

        private void CheckForMoveGeneration()
        {
            if(((this.board.Current == Player.O) && (this.playerO != null)) || ((this.board.Current == Player.X) && (this.playerX != null)))
            {
                IEngine engine = (this.playerX != null) ? this.playerX : ((this.playerO != null) ? this.playerO : null);
                if(engine != null)
                {
                    Move move = engine.GenerateMove(this.board.Clone() as Board);
                    Player currentPlayer = board.Current;
                    this.board.MovePiece(move.Source, move.Destination);
                    OnMoveMade(currentPlayer, move);

                    BuildBoardFromMoveHistory();
                }
            }
        }

        public event MoveMadeEventHandler MoveMade;

    }

    public delegate void MoveMadeEventHandler(object sender, MoveMadeEventArgs e);

    public class MoveMadeEventArgs : EventArgs
    {
        private Player player;
        private Move move;

        public MoveMadeEventArgs(Player player, Move move)
        {
            this.player = player;
            this.move = move;
        }

        public Player Player { get { return this.player; } }
        public Move Move { get { return this.move; } }
    }

}
