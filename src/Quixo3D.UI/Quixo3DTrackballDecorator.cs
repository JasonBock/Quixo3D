using System;
using System.Collections.Generic;
using System.Text;
using _3DTools;

namespace Quixo3D.UI
{
    public class Quixo3DTrackballDecorator : TrackballDecorator
    {
        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!enabled)
            {
                return;
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if(!enabled)
            {
                return;
            }
            base.OnMouseMove(e);
        }
    }
}
