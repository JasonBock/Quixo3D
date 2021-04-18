using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Quixo3D.Framework;

namespace Quixo3D.UI
{
    /// <summary>
    /// Interaction logic for MoveHistory.xaml
    /// </summary>
    public partial class MoveHistory : System.Windows.Controls.UserControl
    {

        private GameBoardController controller;

        /// <summary>
        /// Creates a new instance of the control.
        /// </summary>
        public MoveHistory()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the <see cref="GameBoardController"/> that holds historic move information.
        /// </summary>
        public GameBoardController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                this.itemList.ItemsSource = new ObservableMoveCollection(controller);
            }
        }
    }
}