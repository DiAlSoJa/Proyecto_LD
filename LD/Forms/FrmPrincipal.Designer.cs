namespace LD.Forms
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelTop = new Panel();
            lblTitle = new Label();
            panelUser = new Panel();
            label1 = new Label();
            lblUser = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnFrmMinimizar = new PictureBox();
            btnFrmMaximizar = new PictureBox();
            btnFrmClose = new PictureBox();
            pictureBox1 = new PictureBox();
            flowLayoutPest = new FlowLayoutPanel();
            menuUser = new ContextMenuStrip(components);
            configuraciónToolStripMenuItem = new ToolStripMenuItem();
            cerrarSesiónToolStripMenuItem = new ToolStripMenuItem();
            pCenter = new Panel();
            panelTop.SuspendLayout();
            panelUser.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnFrmMinimizar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnFrmMaximizar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnFrmClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            menuUser.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(20, 41, 84);
            panelTop.Controls.Add(lblTitle);
            panelTop.Controls.Add(panelUser);
            panelTop.Controls.Add(flowLayoutPanel1);
            panelTop.Controls.Add(pictureBox1);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1115, 45);
            panelTop.TabIndex = 0;
            panelTop.DoubleClick += panelTop_DoubleClick;
            panelTop.MouseDown += panelTop_MouseDown;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 10.2F);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(91, 11);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(91, 23);
            lblTitle.TabIndex = 6;
            lblTitle.Text = "LD - Menú";
            // 
            // panelUser
            // 
            panelUser.Controls.Add(label1);
            panelUser.Controls.Add(lblUser);
            panelUser.Dock = DockStyle.Right;
            panelUser.Location = new Point(728, 0);
            panelUser.Name = "panelUser";
            panelUser.Size = new Size(246, 45);
            panelUser.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Cursor = Cursors.Hand;
            label1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(214, 9);
            label1.Name = "label1";
            label1.Size = new Size(25, 23);
            label1.TabIndex = 1;
            label1.Text = "▼";
            label1.Click += label1_Click;
            label1.MouseLeave += lblUser_MouseLeave;
            label1.MouseHover += lblUser_MouseEnter;
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Cursor = Cursors.Hand;
            lblUser.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUser.ForeColor = Color.White;
            lblUser.Location = new Point(7, 9);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(201, 23);
            lblUser.TabIndex = 0;
            lblUser.Text = "Juan Manuel Parvol Vega";
            lblUser.Click += lblUser_Click;
            lblUser.MouseEnter += lblUser_MouseEnter;
            lblUser.MouseLeave += lblUser_MouseLeave;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnFrmMinimizar);
            flowLayoutPanel1.Controls.Add(btnFrmMaximizar);
            flowLayoutPanel1.Controls.Add(btnFrmClose);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(974, 0);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(141, 45);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // btnFrmMinimizar
            // 
            btnFrmMinimizar.Image = Properties.Resources.min;
            btnFrmMinimizar.Location = new Point(3, 3);
            btnFrmMinimizar.Name = "btnFrmMinimizar";
            btnFrmMinimizar.Size = new Size(39, 39);
            btnFrmMinimizar.SizeMode = PictureBoxSizeMode.CenterImage;
            btnFrmMinimizar.TabIndex = 2;
            btnFrmMinimizar.TabStop = false;
            btnFrmMinimizar.Click += btnFrmMinimizar_Click;
            btnFrmMinimizar.MouseLeave += btnFrmMinimizar_MouseLeave;
            btnFrmMinimizar.MouseHover += btnFrmMinimizar_MouseHover;
            // 
            // btnFrmMaximizar
            // 
            btnFrmMaximizar.Image = Properties.Resources.max2;
            btnFrmMaximizar.Location = new Point(48, 3);
            btnFrmMaximizar.Name = "btnFrmMaximizar";
            btnFrmMaximizar.Size = new Size(39, 39);
            btnFrmMaximizar.SizeMode = PictureBoxSizeMode.CenterImage;
            btnFrmMaximizar.TabIndex = 3;
            btnFrmMaximizar.TabStop = false;
            btnFrmMaximizar.Click += btnFrmMaximizar_Click;
            btnFrmMaximizar.MouseLeave += btnFrmMaximizar_MouseLeave;
            btnFrmMaximizar.MouseHover += btnFrmMaximizar_MouseHover;
            // 
            // btnFrmClose
            // 
            btnFrmClose.Image = Properties.Resources.cerr;
            btnFrmClose.Location = new Point(93, 3);
            btnFrmClose.Name = "btnFrmClose";
            btnFrmClose.Size = new Size(45, 42);
            btnFrmClose.SizeMode = PictureBoxSizeMode.CenterImage;
            btnFrmClose.TabIndex = 4;
            btnFrmClose.TabStop = false;
            btnFrmClose.Click += btnFrmClose_Click;
            btnFrmClose.MouseEnter += btnFrmClose_MouseEnter;
            btnFrmClose.MouseLeave += btnFrmClose_MouseLeave;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.barra_de_menus;
            pictureBox1.Location = new Point(11, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 33);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // flowLayoutPest
            // 
            flowLayoutPest.Dock = DockStyle.Top;
            flowLayoutPest.Location = new Point(0, 45);
            flowLayoutPest.Name = "flowLayoutPest";
            flowLayoutPest.Size = new Size(1115, 45);
            flowLayoutPest.TabIndex = 1;
            // 
            // menuUser
            // 
            menuUser.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            menuUser.ImageScalingSize = new Size(20, 20);
            menuUser.Items.AddRange(new ToolStripItem[] { configuraciónToolStripMenuItem, cerrarSesiónToolStripMenuItem });
            menuUser.Name = "menuUser";
            menuUser.Size = new Size(234, 88);
            // 
            // configuraciónToolStripMenuItem
            // 
            configuraciónToolStripMenuItem.AutoSize = false;
            configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            configuraciónToolStripMenuItem.Size = new Size(265, 28);
            configuraciónToolStripMenuItem.Text = "Cambiar contraseña";
            // 
            // cerrarSesiónToolStripMenuItem
            // 
            cerrarSesiónToolStripMenuItem.AutoSize = false;
            cerrarSesiónToolStripMenuItem.Name = "cerrarSesiónToolStripMenuItem";
            cerrarSesiónToolStripMenuItem.Size = new Size(265, 28);
            cerrarSesiónToolStripMenuItem.Text = "Cerrar sesión";
            // 
            // pCenter
            // 
            pCenter.Dock = DockStyle.Fill;
            pCenter.Location = new Point(0, 90);
            pCenter.Name = "pCenter";
            pCenter.Size = new Size(1115, 528);
            pCenter.TabIndex = 2;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1115, 618);
            Controls.Add(pCenter);
            Controls.Add(flowLayoutPest);
            Controls.Add(panelTop);
            Name = "FrmPrincipal";
            Text = "FrmPrincipal";
            WindowState = FormWindowState.Maximized;
            Resize += FrmPrincipal_Resize;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelUser.ResumeLayout(false);
            panelUser.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnFrmMinimizar).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnFrmMaximizar).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnFrmClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            menuUser.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTop;
        private FlowLayoutPanel flowLayoutPest;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox btnFrmMinimizar;
        private PictureBox btnFrmMaximizar;
        private PictureBox btnFrmClose;
        private Label lblUser;
        private Panel panelUser;
        private Label label1;
        private ContextMenuStrip menuUser;
        private ToolStripMenuItem configuraciónToolStripMenuItem;
        private ToolStripMenuItem cerrarSesiónToolStripMenuItem;
        private Label lblTitle;
        private Panel pCenter;
    }
}