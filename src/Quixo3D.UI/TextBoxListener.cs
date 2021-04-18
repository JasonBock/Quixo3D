using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Controls;


namespace Quixo3D.UI
{
    /// <summary>
    /// A text box class used for receiving debug messages.
    /// </summary>
    public class TextBoxListener : TraceListener
    {
        private TextBlock textBox = null;

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="box"></param>
        public TextBoxListener(TextBlock box)
        {
            textBox = box;
        }

        /// <summary>
        /// Writes a message to the text box.
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            textBox.Text = message + "\n" + textBox.Text;    
        }

        /// <summary>
        /// Writes a message to the text box.
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            textBox.Text = message + "\n" + textBox.Text;
        }
    }
}
