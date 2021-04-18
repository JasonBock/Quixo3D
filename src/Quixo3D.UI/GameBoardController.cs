using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
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
using Petzold.Text3D;

namespace Quixo3D.UI
{
    /// <summary>
    /// The <see cref="GameBoardController"/> class manages all interaction between the game logic and
    /// user interface elements.
    /// </summary>
    public class GameBoardController
    {

        bool showingOnlyPlacedPieces;
        bool isDirty;
        string savePath = string.Empty;
        Board board;
        BoardAxes axes;
        Collection<ExecuteMoveOperation> moveOperations;
        Collection<GamePieceVisual> pieces;
        GamePieceVisual highlightedPiece;
        Hashtable coordinatePieceMap;
        IEngine playerX;
        IEngine playerO;
        ModelVisual3D parentVisual;
        ShowValidMoveOperation currentShowValidMoveOperation;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        public GameBoardController(ModelVisual3D parentVisual)
        {
            this.parentVisual = parentVisual;
            this.InitializeMap();
        }

        /// <summary>
        /// Gets a collection of <see cref="ExecuteMoveOperation"/> objects that represent
        /// moves made throughout the game.
        /// </summary>
        public Collection<ExecuteMoveOperation> MoveOperations
        {
            get { return this.moveOperations; }
        }

        /// <summary>
        /// Gets or sets the AI plugin used for Player X.
        /// </summary>
        public IEngine PlayerX
        {
            get { return this.playerX; }
            set { this.playerX = value; }
        }

        /// <summary>
        /// Gets or sets the AI plugin used for Player O.
        /// </summary>
        public IEngine PlayerO
        {
            get { return this.playerO; }
            set { this.playerO = value; }
        }

        /// <summary>
        /// Gets or sets the file system path used for saving/loading a game.
        /// </summary>
        public string SavePath
        {
            get { return this.savePath; }
            set { this.savePath = value; }
        }

        /// <summary>
        /// Gets whether the game state has changed and has not yet been persisted to disk.
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
        }

        /// <summary>
        /// Gets whether the game is over.
        /// </summary>
        public bool GameOver
        {
            get
            {
                return this.board.Winner != Players.None;
            }
        }

        /// <summary>
        /// Gets a reference to the <see cref="Quixo3D.Framework.Board"/> object that holds information
        /// about the game.
        /// </summary>
        public Board Board
        {
            get { return this.board; }
        }

        /// <summary>
        /// Starts over with a new game.
        /// </summary>
        public void Reset()
        {
            this.Reset(null);
        }

        void Reset(Board newBoard)
        {
            if(newBoard == null)
            {
                this.board = new Board();
            }
            else
            {
                this.board = newBoard;
            }

            this.moveOperations = new Collection<ExecuteMoveOperation>();
            this.OnGameReset();
            this.BuildBoardFromMoveHistory();
            this.isDirty = false;
            this.savePath = string.Empty;
        }

        /// <summary>
        /// Fires whenever a move is made in the game.
        /// </summary>
        public event EventHandler<MoveMadeEventArgs> MoveMade;

        /// <summary>
        /// Fires whenever a new game is started.
        /// </summary>
        public event EventHandler<EventArgs> GameReset;

        void OnGameReset()
        {
            if(this.GameReset != null)
            {
                this.GameReset(this, EventArgs.Empty);
            }
        }

        void InitializeMap()
        {
            this.coordinatePieceMap = new Hashtable();
            ModelVisual3D container = GetPieceContainer();
            this.pieces = new Collection<GamePieceVisual>();
            container.Children.Clear();

            int maxDimension = Constants.Dimension;
            
            for(byte x = 0;x < maxDimension;x++)
            {
                for(byte y = 0;y < maxDimension;y++)
                {
                    for(byte z = 0;z < maxDimension;z++)
                    {
                        Coordinate coordinate = new Coordinate(x, y, z);
                        GamePieceVisual gamePiece = new GamePieceVisual();

                        gamePiece.BaseGridTransform.OffsetX = Convert.ToDouble(x) * ConfigSettings.Instance.CellSpacing;
                        gamePiece.BaseGridTransform.OffsetY = Convert.ToDouble(y) * ConfigSettings.Instance.CellSpacing;
                        gamePiece.BaseGridTransform.OffsetZ = Convert.ToDouble(z) * ConfigSettings.Instance.CellSpacing;

                        container.Children.Add(gamePiece);

                        this.pieces.Add(gamePiece);
                        this.coordinatePieceMap.Add(coordinate, gamePiece);
                    }
                }
            }

            this.axes = new BoardAxes();
            this.axes.Visible = true;
            container.Children.Add(axes);

            double centerMovement = ConfigSettings.Instance.CellSpacing * 2 * -1;
            TranslateTransform3D centering = new TranslateTransform3D(centerMovement, centerMovement, centerMovement);
            container.Transform = centering;
        }

        void BuildBoardFromMoveHistory()
        {

            GamePieceVisual gamePiece;

            //reset everything
            foreach(Coordinate key in this.coordinatePieceMap.Keys)
            {
                gamePiece = (GamePieceVisual)this.coordinatePieceMap[key];
                gamePiece.IsValidSource = false;
                gamePiece.HighlightedAsValidDestination = false;
                gamePiece.HighlightedAsValidSource = false;

                if(gamePiece.IsEnlarged)
                {
                    gamePiece.ShrinkToNormalSize();
                }

                Players player = this.board.GetPlayer(key);

                switch(player)
                {
                    case Players.None:
                        gamePiece.PieceType = PieceType.Empty;
                        break;
                    case Players.X:
                        gamePiece.PieceType = PieceType.X;
                        break;
                    case Players.O:
                        gamePiece.PieceType = PieceType.O;
                        break;
                }

            }

            if(this.board.Winner == Players.None)
            {
                ReadOnlyCollection<ValidMove> validMoves = this.board.GetValidMoves();
                foreach(ValidMove move in validMoves)
                {
                    Coordinate source = move.Source;
                    GamePieceVisual piece = this.coordinatePieceMap[source] as GamePieceVisual;
                    if(piece != null)
                    {
                        piece.IsValidSource = true;
                    }
                }
            }
            else
            {
                this.HighlightWinningPieces();
            }
        }

        void HighlightWinningPieces()
        {
            ReadOnlyCollection<Line> lines = board.WinningLines;
            GamePieceVisual piece;

            foreach (Line line in lines){

                foreach(Coordinate coordinate in coordinatePieceMap.Keys)
                {
                    piece = GetCoordinateGamePiece(coordinate);
                    if(line.Contains(coordinate))
                    {
                        piece.Enlarge(1.6);
                    }
                    else
                    {
                        piece.Visible = false;
                    }
                }
            }
        }

        ModelVisual3D GetPieceContainer()
        {
            ModelVisual3D container;
            if(this.parentVisual.Children.Count > 0 && this.parentVisual.Children[0] != null)
            {
                container = this.parentVisual.Children[0] as ModelVisual3D;
            }
            else
            {
                container = new ModelVisual3D();
                this.parentVisual.Children.Add(container);
            }

            return container;
        }      

        void OnMoveMade(ExecuteMoveOperation operation)
        {
            if(this.MoveMade != null)
            {
                MoveMadeEventArgs args = new MoveMadeEventArgs(operation.Player, operation.Move);
                this.MoveMade(this, args);
            }
        }

        static ModelVisual3D GetHitTestResult(Viewport3D viewport, Point location)
        {

            HitTestResult result = VisualTreeHelper.HitTest(viewport, location);
            if(result != null && result.VisualHit is ModelVisual3D)
            {
                ModelVisual3D modelVisual = (ModelVisual3D)result.VisualHit;
                return modelVisual;
            }
            return null;
        }

        /// <summary>
        /// Shows or hides the X,Y,Z axes model in the game.
        /// </summary>
        public void ToggleAxesVisibility()
        {
            this.axes.Visible = !this.axes.Visible;
        }

        /// <summary>
        /// If a piece on the board is currently highlighted, change it to an un-highlighted state.
        /// </summary>
        public void ClearHighlight()
        {
            if(this.highlightedPiece != null)
            {
                this.highlightedPiece.Highlighted = false;
            }
            this.highlightedPiece = null;
        }

        /// <summary>
        /// Gets the <see cref="GamePieceVisual"/> game piece that exists at the specified coordinate.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public GamePieceVisual GetCoordinateGamePiece(Coordinate coordinate)
        {
            return this.coordinatePieceMap[coordinate] as GamePieceVisual;
        }

        /// <summary>
        /// Gets the <see cref="Quixo3D.Framework.Coordinate"/> that corresponds to the specified
        /// <see cref="GamePieceVisual"/>.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        public Coordinate GetGamePieceCoordinate(GamePieceVisual piece)
        {
            foreach(Coordinate key in this.coordinatePieceMap.Keys)
            {
                GamePieceVisual foundPiece = this.coordinatePieceMap[key] as GamePieceVisual;
                if(foundPiece == piece)
                {
                    return key;
                }
            }

            throw new GameException("A coordinate for the specified game piece was not found.");
        }

        /// <summary>
        /// Automates an AI engine move if a plugin is used for the current player.
        /// </summary>
        void CheckForEngineMove()
        {
            Move move = null;
            if(this.board.Current == Players.X && this.PlayerX != null)
            {
                move = this.PlayerX.GenerateMove(board);
            }
            else if(this.board.Current == Players.O && this.PlayerO != null)
            {
                move = this.PlayerO.GenerateMove(this.board);
            }

            if(move != null)
            {
                ExecuteMoveOperation operation = new ExecuteMoveOperation(this, move);
                operation.Execute();
                moveOperations.Add(operation);
                this.BuildBoardFromMoveHistory();
                this.OnMoveMade(operation);
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Takes user interface input in the form of a mouse pointer location and the containing
        /// 3D viewport and attempts to "select" the piece that exists at that location.  If a piece is 
        /// found, the appropriate game operation is performed, such as highlighting a valid source, or
        /// executing a move.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="viewport"></param>
        public void SelectPiece(Point location, Viewport3D viewport)
        {
            if(this.GameOver || this.showingOnlyPlacedPieces)
            {
                return;
            }

            ModelVisual3D hitTestResult = GameBoardController.GetHitTestResult(viewport, location);
            GamePieceVisual targetPiece = GameBoardController.ConvertHitTestResultToGamePieceVisual(hitTestResult);

            if(this.currentShowValidMoveOperation != null)
                {
                    if(targetPiece != null && this.currentShowValidMoveOperation.ValidDestinationPieces.Contains(targetPiece))
                    {
                        //execute move
                        ExecuteMoveOperation operation = new ExecuteMoveOperation(this, this.currentShowValidMoveOperation.SourcePiece, targetPiece);
                        this.currentShowValidMoveOperation.Hide();
                        this.currentShowValidMoveOperation = null;
                        operation.Execute();
                        this.moveOperations.Add(operation);
                        this.BuildBoardFromMoveHistory();
                        this.OnMoveMade(operation);
                        this.CheckForEngineMove();
                        this.isDirty = true;
                    }
                }
                else
                {
                    if(targetPiece != null)
                    {
                        if(targetPiece.IsValidSource)
                        {
                            this.currentShowValidMoveOperation = new ShowValidMoveOperation(this, targetPiece);
                            this.currentShowValidMoveOperation.Show();
                        }
                    }
                }
        }

        /// <summary>
        /// If any pieces are being highlighted for a valid move operation, this method
        /// returns those pieces to a normal, un-highlighted state.
        /// </summary>
        public void CancelSelection()
        {
            if(this.currentShowValidMoveOperation != null)
            {
                this.currentShowValidMoveOperation.Hide();
                this.currentShowValidMoveOperation = null;
            }
        }

        /// <summary>
        /// Gets a collection of all of the game pieces on the board.
        /// </summary>
        public Collection<GamePieceVisual> Pieces
        {
            get { return this.pieces; }
        }

        /// <summary>
        /// Hides all pieces on the board unless they are a placed X or O piece.
        /// </summary>
        public void ShowOnlyPlacedPieces()
        {
            foreach(GamePieceVisual piece in this.pieces)
            {
                if(piece.PieceType == PieceType.Empty)
                {
                    piece.Visible = false;
                }
                else
                {
                    piece.HideWireFrame();
                }
            }
            this.showingOnlyPlacedPieces = true;
        }

        /// <summary>
        /// Shows all pieces on the board if they are a valid source or a placed piece.
        /// </summary>
        public void ShowAllPieces()
        {
            foreach(GamePieceVisual piece in this.pieces)
            {
                piece.Visible = true;
            }
            this.showingOnlyPlacedPieces = false;
        }

        /// <summary>
        /// Attempts to highlight a game piece at the specified mouse pointer location.
        /// </summary>
        /// <remarks>
        /// This method is primarily used for mouse "rollover" actions.
        /// </remarks>
        /// <param name="location"></param>
        /// <param name="viewport"></param>
        public void HighlightPiece(Point location, Viewport3D viewport)
        {
            if(this.GameOver || this.currentShowValidMoveOperation != null || this.showingOnlyPlacedPieces)
            {
                return;
            }

            ModelVisual3D selectedItem = GameBoardController.GetHitTestResult(viewport, location);

            if(selectedItem == null)
            {
                if(this.highlightedPiece != null)
                {
                    this.highlightedPiece.Highlighted = false;
                }
                return;
            }

            GamePieceVisual targetPiece = GameBoardController.ConvertHitTestResultToGamePieceVisual(selectedItem);

            if(targetPiece != null)
            {
                if(targetPiece != this.highlightedPiece)
                {
                    if(this.highlightedPiece != null)
                    {
                        this.highlightedPiece.Highlighted = false;
                    }
                }

                this.highlightedPiece = targetPiece;
                if(targetPiece.IsValidSource)
                {
                    this.highlightedPiece.Highlighted = true;
                }
            }
        }

        /// <summary>
        /// A helper method used to find a <see cref="GamePieceVisual"/> object by either direct
        /// casting or walking the visual tree.
        /// </summary>
        /// <param name="hitTestResult"></param>
        /// <returns></returns>
        static GamePieceVisual ConvertHitTestResultToGamePieceVisual(ModelVisual3D hitTestResult)
        {
            if(hitTestResult == null)
            {
                return null;
            }

            PlanarText textResult = hitTestResult as PlanarText;
            if(textResult != null)
            {
                return GameBoardController.GetGamePieceVisualFromText(textResult);
            }

            GamePieceVisual pieceResult = hitTestResult as GamePieceVisual;
            if(pieceResult != null)
            {
                return pieceResult;
            }

            return null;
        }

        /// <summary>
        /// Walks the visual tree to find a <see cref="GamePieceVisual"/> from a
        /// <see cref="PlanarText"/> object.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static GamePieceVisual GetGamePieceVisualFromText(PlanarText text)
        {
            bool flag = false;
            DependencyObject currentChild = text;
            GamePieceVisual foundPiece = null;
            int max = 100;
            int count = 0;
            while(flag == false)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(currentChild);
                GamePieceVisual parentPiece = parent as GamePieceVisual;
                if(parentPiece != null)
                {
                    foundPiece = parentPiece;
                    flag = true;
                }
                else
                {
                    if(parent is BoardAxes)
                    {
                        return null;
                    }
                    currentChild = parent;
                }

                count = count + 1;
                if(count > max)
                {
                    throw new GameException("A parent game piece on a text model could not be found.");
                }
            }

            return foundPiece;
        }

        /// <summary>
        /// Persists the game to disk.
        /// </summary>
        public void SaveGame()
        {
            this.SaveGame(savePath);
        }

        /// <summary>
        /// Persists the game to disk at the specified path.
        /// </summary>
        /// <param name="path"></param>
        public void SaveGame(string path)
        {
            FileStream stream = null;
            try
            {
                this.savePath = path;
                BoardFormatter formatter = new BoardFormatter();
                stream = new FileStream(this.savePath, FileMode.Create);
                formatter.Serialize(stream, this.board);
                this.isDirty = false;
            }
            finally
            {
                if(stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Opens a game from disk using the specified path.
        /// </summary>
        /// <param name="path"></param>
        public void OpenGame(string path)
        {
            FileStream stream = null;
            try
            {
                BoardFormatter formatter = new BoardFormatter();
                stream = new FileStream(path, FileMode.Open);
                Board savedBoard = formatter.Deserialize(stream) as Board;

                if(savedBoard == null)
                {
                    throw new GameException(string.Format("Board could not be loaded from path {0}.", path));
                }

                this.Reset(savedBoard);
                this.savePath = path;
            }
            finally
            {
                if(stream != null)
                {
                    stream.Dispose();
                }
            }
        }
    }
}
