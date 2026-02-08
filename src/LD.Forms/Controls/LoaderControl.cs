using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LD.Forms.Controls
{
    public partial class LoaderControl : UserControl
    {
        public LoaderControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }
        private void LoaderControl_Resize(object sender, EventArgs e)
        {
            CenterElements();
        }

        private void CenterElements()
        {
            loaderGif.Location = new Point(
                (Width - loaderGif.Width) / 2,
                (Height - loaderGif.Height) / 2
            );
            label1.Location = new Point(
                (Width - label1.Width) / 2,
                loaderGif.Top - 10 - label1.Height
            );
        }
        public void SetText(string text)
        {
            label1.Text = string.IsNullOrWhiteSpace(text) ? "Cargando..." : text;
            CenterElements();
        }
    }
}
