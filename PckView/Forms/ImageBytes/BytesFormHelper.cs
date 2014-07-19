using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PckView.Panels;

namespace PckView.Forms.ImageBytes
{
    public class BytesFormHelper
    {
        private static readonly BytesFormHelper _instance = new BytesFormHelper();

        private Form bytesFrame;
        private RichTextBox bytesText;

        public static void ShowBytes(ViewPckItemImage selected, MethodInvoker closedCallBack, Point location)
        {
            _instance.ShowBytesCore(selected, closedCallBack, location);
        }

        public static void ReloadBytes(ViewPckItemImage selected)
        {
            _instance.ReloadBytesCore(selected);
        }

        private void ShowBytesCore(ViewPckItemImage selected, MethodInvoker closedCallBack, Point location)
        {
            if (bytesFrame != null)
            {
                bytesFrame.BringToFront();
            }
            else if (selected != null)
            {
                bytesFrame = new Form();
                bytesText = new RichTextBox();
                bytesText.Dock = DockStyle.Fill;
                bytesFrame.Controls.Add(bytesText);

                foreach (byte b in selected.Image.Bytes)
                    bytesText.Text += b + " ";

                bytesFrame.Closing += bClosing;
                bytesFrame.Closing += (s, e) => closedCallBack();
                bytesFrame.Location = location;
                bytesFrame.Text = "Length: " + selected.Image.Bytes.Length;
                bytesFrame.Show();
            }
        }

        private void bClosing(object sender, CancelEventArgs e)
        {
            bytesFrame = null;
        }

        public void ReloadBytesCore(ViewPckItemImage selected)
        {
            if (bytesFrame == null) return;
            if (selected != null && selected.Image != null)
            {
                bytesFrame.Text = "Length: " + selected.Image.Bytes.Length;
                bytesText.Clear();
                var text = string.Empty;
                foreach (byte b in selected.Image.Bytes)
                    text += b + " ";
                bytesText.Text = text;
            }
            else
            {
                bytesFrame.Text = "";
                bytesText.Text = "";
            }
        }
    }
}
