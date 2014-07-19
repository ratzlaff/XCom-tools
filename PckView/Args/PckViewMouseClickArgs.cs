using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PckView.Args
{
    public class PckViewMouseClickArgs : MouseEventArgs
    {
        public PckViewMouseClickArgs(MouseEventArgs eventArgs, int clickedPck)
            : base(eventArgs.Button, eventArgs.Clicks, eventArgs.X, eventArgs.Y, eventArgs.Delta)
        {
            ClickedPck = clickedPck;
        }

        public readonly int ClickedPck;
    }
}
