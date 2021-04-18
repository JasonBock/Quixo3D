using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;
using Petzold.Media3D;
using Quixo3D.Framework;

namespace Quixo3D.UI {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public partial class MainWindow : System.Windows.Window {

        GameBoardController controller;
        bool showingOnlyPlacedPieces;
        Vector3D currentUpDirection;

        /// <summary>
        /// Creates a new instance of the window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.trackball.Enabled = false;
            this.mainViewport.MouseDown += new MouseButtonEventHandler(mainViewport_MouseDown);
            this.mainViewport.MouseMove += new MouseEventHandler(mainViewport_MouseMove);
            this.mainViewport.MouseLeave += new MouseEventHandler(mainViewport_MouseLeave);
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            this.KeyUp += new KeyEventHandler(MainWindow_KeyUp);

            this.controller = new GameBoardController(this.boardVisual);
            this.controller.MoveMade += new EventHandler<MoveMadeEventArgs>(controller_MoveMade);
            this.controller.GameReset += new EventHandler<EventArgs>(controller_GameReset);
            this.controller.Reset();
            this.moveHistory.Controller = this.controller;
            this.currentUpDirection = ((PerspectiveCamera)this.mainViewport.Camera).UpDirection;

        }

        void controller_GameReset(object sender, EventArgs e)
        {
            this.UpdateCurrentPlayerText();
        }

        void UpdateCurrentPlayerText()
        {
            if(this.controller.Board.Winner == Players.None)
            {
                this.turnTextBlock.Text = string.Format("Current Player: {0}", this.controller.Board.Current.ToString());
            }
            else
            {
                this.turnTextBlock.Text = string.Format("Player {0} is the winner!", this.controller.Board.Winner.ToString());
            }
        }

        void controller_MoveMade(object sender, MoveMadeEventArgs e)
        {
            string message = "Player {0} moved from {1} to {2}.";
            string source = e.Move.Source.ToString();
            string destination = e.Move.Destination.ToString();
            this.infoTextBlock.Text = string.Format(message, e.Player.ToString(), source, destination);

            this.UpdateCurrentPlayerText();

        }

        void mainViewport_MouseLeave(object sender, MouseEventArgs e)
        {
            this.controller.ClearHighlight();
        }

        void mainViewport_MouseMove(object sender, MouseEventArgs e)
        {
                if(!trackball.Enabled)
                {
                    Point location = e.GetPosition(mainViewport);
                    this.controller.HighlightPiece(location, this.mainViewport);
                }
        }
      
        void MainWindow_KeyDown(object sender, KeyEventArgs e) {

                if(e.Key == Key.LeftShift || e.Key == Key.RightShift)
                {
                    this.trackball.Enabled = true;
                }
                else if(e.Key == Key.Escape)
                {
                    this.controller.CancelSelection();
                }
                else if(e.Key == Key.S)
                {
                    if(showingOnlyPlacedPieces)
                    {
                        this.controller.ShowAllPieces();
                        this.showingOnlyPlacedPieces = false;
                    }
                    else
                    {
                        this.controller.ShowOnlyPlacedPieces();
                        this.showingOnlyPlacedPieces = true;
                    }
                }
                else if(e.Key == Key.X)
                {
                    this.controller.ToggleAxesVisibility();
                }
                else if(e.Key == Key.U)
                {
                    this.ChangeUpDirection();
                }
               
        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            this.trackball.Enabled = false;
        }

        void mainViewport_MouseDown(object sender, MouseButtonEventArgs e) {
           
                if(this.trackball.Enabled)
                {
                    return;
                }

                if(e.LeftButton == MouseButtonState.Pressed)
                {
                    Point location = e.GetPosition(this.mainViewport);
                    this.controller.SelectPiece(location, this.mainViewport);
                }
         
        }

        void ChangeUpDirection(){
            Vector3D newUpDirection = new Vector3D(0,1,0);

            if(this.currentUpDirection.X == 1)
            {
                newUpDirection = new Vector3D(0, 0, 1);
            }
            else if(this.currentUpDirection.Y == 1)
            {
                newUpDirection = new Vector3D(1, 0, 0);
            }
            else if(this.currentUpDirection.Z == 1)
            {
                newUpDirection = new Vector3D(0, 1, 0);
            }

            PerspectiveCamera camera = this.mainViewport.Camera as PerspectiveCamera;
            Vector3DAnimation animation = new Vector3DAnimation(currentUpDirection, newUpDirection, TimeSpan.FromMilliseconds(1000));
            camera.BeginAnimation(PerspectiveCamera.UpDirectionProperty, animation);
            this.currentUpDirection = newUpDirection;

        }

        void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        void NewMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.CheckForGameInProgress();

                PluginSelector selector = new PluginSelector();
                bool? result = selector.ShowDialog();

                if(result == true)
                {
                    this.controller.PlayerX = selector.PlayerX;
                    this.controller.PlayerO = selector.PlayerO;
                }

                this.controller.Reset();
                
            }
            finally
            {
                this.GiveBoardFocus();
            }
        }

        void CheckForGameInProgress()
        {
            if(this.controller.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save your current game before continuing?", "Save Current Game", MessageBoxButton.YesNoCancel);

                if(result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if(result == MessageBoxResult.Yes)
                {
                    this.SaveGame();
                }
            }

        }

        void OpenMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.CheckForGameInProgress();
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.Filter = "Quixo3D files (*.q3d)|*.q3d";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = string.Empty;
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.FileName;
                    this.controller.OpenGame(path);
                    this.moveHistory.Controller = controller;
                }
            }
            finally
            {
                this.GiveBoardFocus();
            }
        }

        void SaveGame()
        {
            string path = string.Empty;
            if(string.IsNullOrEmpty(controller.SavePath))
            {
                System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                dialog.Filter = "Quixo3D files (*.q3d)|*.q3d";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.FileName;
                }
                else
                {
                    return;
                }
            }
            else
            {
                path = controller.SavePath;
            }

            this.controller.SaveGame(path);

        }

        void GiveBoardFocus()
        {
            this.Focus();
        }

        static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        void SaveMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(controller.IsDirty)
                {
                    this.SaveGame();
                }
            }
            catch(InvalidOperationException invalidOpException)
            {
                MainWindow.ShowMessage(invalidOpException.ToString());
            }
            finally
            {
                this.GiveBoardFocus();

            }
        }

    }
}