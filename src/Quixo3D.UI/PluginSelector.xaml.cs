using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Quixo3D.Engine;

namespace Quixo3D.UI
{
    /// <summary>
    /// Interaction logic for PluginSelector.xaml
    /// </summary>
    public partial class PluginSelector : Window
    {

        IEngine playerX;
        IEngine playerO;

        /// <summary>
        /// Creates a new instance of the control.
        /// </summary>
        public PluginSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the AI engine for Player X.
        /// </summary>
        public IEngine PlayerX
        {
            get { return playerX; }
        }

        /// <summary>
        /// Gets the AI engine for Player O.
        /// </summary>
        public IEngine PlayerO
        {
            get { return playerO; }
        }

        void OkButtonClick(object sender, EventArgs e)
        {

            if(!this.xPluginList.IsHuman)
            {
                Type xType = this.xPluginList.SelectedPluginType;
                playerX = (IEngine)Activator.CreateInstance(xType);

            }
            else
            {
                playerX = null;
            }

            if(!this.oPluginList.IsHuman)
            {
                Type oType = this.oPluginList.SelectedPluginType;
                playerO = (IEngine)Activator.CreateInstance(oType);
            }
            else
            {
                playerO = null;
            }

            this.DialogResult = true;
            
            Close();
        }


       

    }
}