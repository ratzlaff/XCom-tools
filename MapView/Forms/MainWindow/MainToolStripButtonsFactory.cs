using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MapView.Forms.MainWindow
{
    public class MainToolStripButtonsFactory
    {
        private readonly List<ToolStripButton> _pasteButtons = new List<ToolStripButton>();
 
        /// <summary>
        /// Adds buttons for Up,Down,Cut,Copy and Paste to a toolstrip as well as sets some properties for the toolstrip
        /// </summary>
        /// <param name="toolStrip"></param>
        public void MakeToolstrip(ToolStrip toolStrip)
        {
            var btnUp = new ToolStripButton();
            var btnDown = new ToolStripButton();
            var btnCut = new ToolStripButton();
            var btnCopy = new ToolStripButton();
            var btnPaste = new ToolStripButton();
            var btnFill = new ToolStripButton();

            // 
            // toolStrip1
            // 
            //toolStrip.Dock = DockStyle.None;
            //toolStrip.GripMargin = new Padding(0);
            //toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Items.AddRange(new ToolStripItem[] {
            btnUp,
            btnDown,
            btnCut,
            btnCopy,
            btnPaste,
            btnFill});
            //toolStrip1.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            toolStrip.Padding = new Padding(0);
            toolStrip.RenderMode = ToolStripRenderMode.System;
            toolStrip.TabIndex = 1;

            // 
            // btnFill
            // 
            btnFill.AutoSize = false;
            btnFill.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnFill.Name = "btnFill";
            btnFill.Size = new Size(25, 25);
            btnFill.Text = "Fill";
            btnFill.ToolTipText = "Fill";
            btnFill.Click += MainWindowsManager.TopView.TopViewControl.Fill_Click;

            // 
            // btnUp
            // 
            btnUp.AutoSize = false;
            btnUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnUp.ImageScaling = ToolStripItemImageScaling.None;
            btnUp.ImageTransparentColor = Color.Magenta;
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(25, 25);
            btnUp.Text = "toolStripButton1";
            btnUp.ToolTipText = "Level Up";
            btnUp.Click += btnUp_Click;
            // 
            // btnDown
            // 
            btnDown.AutoSize = false;
            btnDown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDown.ImageScaling = ToolStripItemImageScaling.None;
            btnDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnDown.Name = "btnDown";
            btnDown.Size = new System.Drawing.Size(25, 25);
            btnDown.Text = "toolStripButton2";
            btnDown.ToolTipText = "Level Down";
            btnDown.Click += btnDown_Click;
            // 
            // btnCut
            // 
            btnCut.AutoSize = false;
            btnCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCut.ImageScaling = ToolStripItemImageScaling.None;
            btnCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCut.Name = "btnCut";
            btnCut.Size = new System.Drawing.Size(25, 25);
            btnCut.Text = "toolStripButton3";
            btnCut.ToolTipText = "Cut";
            btnCut.Click += (o, args) =>
            {
                ActivatePasteButtons();
                MapViewPanel.Instance.Cut_click(o, args);
            };

            // 
            // btnCopy
            // 
            btnCopy.AutoSize = false;
            btnCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnCopy.ImageScaling = ToolStripItemImageScaling.None;
            btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(25, 25);
            btnCopy.Text = "toolStripButton4";
            btnCopy.ToolTipText = "Copy";
            btnCopy.Click += (o, args) =>
            {
                ActivatePasteButtons();
                MapViewPanel.Instance.Copy_click(o, args);
            };

            // 
            // btnPaste
            // 
            btnPaste.AutoSize = false;
            btnPaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnPaste.ImageScaling = ToolStripItemImageScaling.None;
            btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new System.Drawing.Size(25, 25);
            btnPaste.Text = "toolStripButton5";
            btnPaste.ToolTipText = "Paste";
            btnPaste.Click += MapViewPanel.Instance.Paste_click;
            btnPaste.Enabled = false;
            _pasteButtons.Add(btnPaste);

            Assembly a = Assembly.GetExecutingAssembly();
            btnCut.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.cut.gif"));
            btnPaste.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.paste.gif"));
            btnCopy.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.copy.gif"));
            btnUp.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.up.gif"));
            btnDown.Image = Bitmap.FromStream(a.GetManifestResourceStream("MapView._Embedded.down.gif"));
        }

        private void ActivatePasteButtons()
        {
            foreach (var pasteButton in _pasteButtons)
            {
                pasteButton.Enabled = true;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (MapViewPanel.Instance.View.Map != null)
            {
                MapViewPanel.Instance.View.Map.Down();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (MapViewPanel.Instance.View.Map != null)
            {
                MapViewPanel.Instance.View.Map.Up();
            }
        }
    }
}
