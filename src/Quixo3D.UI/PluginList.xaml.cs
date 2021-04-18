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

namespace Quixo3D.UI
{
    /// <summary>
    /// Interaction logic for PluginList.xaml
    /// </summary>

    public partial class PluginList : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Creates a new instance of the control.
        /// </summary>
        public PluginList()
        {
            InitializeComponent();
            this.pluginListBox.ItemsSource = new PluginCollection();
            this.pluginListBox.SelectedIndex = 0;

            humanCheckBox.Checked += new RoutedEventHandler(humanCheckBox_Checked);
            humanCheckBox.Unchecked += new RoutedEventHandler(humanCheckBox_Unchecked);
        }

        void humanCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pluginListBox.IsEnabled = true;
        }

        void humanCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            pluginListBox.IsEnabled = false;
        }

        /// <summary>
        /// Gets or sets the text of the title area.
        /// </summary>
        public string Title
        {
            get { return titleTextBlock.Text; }
            set { titleTextBlock.Text = value; }
        }

        /// <summary>
        /// Gets the AI engine plugin that has been selected.
        /// </summary>
        public Type SelectedPluginType
        {
            get { return (Type)pluginListBox.SelectedValue; }
        }

        /// <summary>
        /// Gets whether a plugin is selected or if a human opponent is selected.
        /// </summary>
        public bool IsHuman
        {
            get { return humanCheckBox.IsChecked.Equals(true); }
        }

      
    }
}