namespace LD.Forms.Controls
{
    partial class LoaderControl
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoaderControl));
            label1 = new Label();
            loaderGif = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)loaderGif).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(196, 138);
            label1.Name = "label1";
            label1.Size = new Size(97, 23);
            label1.TabIndex = 0;
            label1.Text = "Cargando...";
            // 
            // loaderGif
            // 
            loaderGif.Image = (Image)resources.GetObject("loaderGif.Image");
            loaderGif.Location = new Point(167, 164);
            loaderGif.Name = "loaderGif";
            loaderGif.Size = new Size(168, 159);
            loaderGif.SizeMode = PictureBoxSizeMode.Zoom;
            loaderGif.TabIndex = 1;
            loaderGif.TabStop = false;
            // 
            // LoaderControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(loaderGif);
            Controls.Add(label1);
            Name = "LoaderControl";
            Size = new Size(530, 349);
            Resize += LoaderControl_Resize;
            ((System.ComponentModel.ISupportInitialize)loaderGif).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox loaderGif;
    }
}
