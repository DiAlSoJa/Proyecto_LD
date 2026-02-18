using LD.Forms.Controls;

namespace LD.Forms.Views.Forms
{
    partial class FrmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            btnLogin = new RoundedButton();
            txtPassword = new TextBox();
            txtUsuario = new TextBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel4 = new Panel();
            panel3 = new Panel();
            panel1 = new Panel();
            roundedPanel1 = new RoundedPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(-1, -92);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(895, 591);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseDown += panel1_MouseDown;
            pictureBox1.MouseMove += panel1_MouseMove;
            pictureBox1.MouseUp += panel1_MouseUp;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.FromArgb(20, 41, 84);
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(267, 438);
            label2.Name = "label2";
            label2.Size = new Size(118, 23);
            label2.TabIndex = 10;
            label2.Text = "Version: 1.0.0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(20, 41, 84);
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(267, 255);
            label1.Name = "label1";
            label1.Size = new Size(79, 41);
            label1.TabIndex = 4;
            label1.Text = "LMS";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ButtonHighlight;
            label3.Cursor = Cursors.Hand;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(20, 41, 84);
            label3.Location = new Point(353, 20);
            label3.Name = "label3";
            label3.Size = new Size(38, 41);
            label3.TabIndex = 11;
            label3.Text = "X";
            label3.Click += pictureBox4_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(20, 41, 84);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(61, 337);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(320, 50);
            btnLogin.TabIndex = 9;
            btnLogin.Text = "Iniciar sesión";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = SystemColors.ButtonHighlight;
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(108, 225);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "Contraseña";
            txtPassword.Size = new Size(262, 27);
            txtPassword.TabIndex = 7;
            // 
            // txtUsuario
            // 
            txtUsuario.BackColor = SystemColors.ButtonHighlight;
            txtUsuario.BorderStyle = BorderStyle.None;
            txtUsuario.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsuario.Location = new Point(108, 127);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.PlaceholderText = "Usuario";
            txtUsuario.Size = new Size(262, 27);
            txtUsuario.TabIndex = 6;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.user;
            pictureBox3.Location = new Point(61, 125);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(41, 32);
            pictureBox3.TabIndex = 5;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.key;
            pictureBox2.Location = new Point(61, 221);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(41, 32);
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(20, 41, 84);
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Location = new Point(61, 261);
            panel4.Name = "panel4";
            panel4.Size = new Size(330, 3);
            panel4.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(20, 41, 84);
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Location = new Point(61, 163);
            panel3.Name = "panel3";
            panel3.Size = new Size(330, 3);
            panel3.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(roundedPanel1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(895, 500);
            panel1.TabIndex = 11;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = SystemColors.ButtonHighlight;
            roundedPanel1.BorderColor = Color.LightGray;
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.BorderSize = 1;
            roundedPanel1.Controls.Add(btnLogin);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(txtPassword);
            roundedPanel1.Controls.Add(pictureBox3);
            roundedPanel1.Controls.Add(txtUsuario);
            roundedPanel1.Controls.Add(panel3);
            roundedPanel1.Controls.Add(panel4);
            roundedPanel1.Controls.Add(pictureBox2);
            roundedPanel1.Location = new Point(448, 21);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(428, 452);
            roundedPanel1.TabIndex = 11;
            roundedPanel1.Paint += roundedPanel1_Paint;
            roundedPanel1.MouseDown += panel1_MouseDown;
            roundedPanel1.MouseMove += panel1_MouseMove;
            roundedPanel1.MouseUp += panel1_MouseUp;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(895, 500);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Panel panel4;
        private Panel panel3;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private RoundedButton btnLogin;
        private Label label3;
        private Panel panel1;
        private RoundedPanel roundedPanel1;
    }
}
