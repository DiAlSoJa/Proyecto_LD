namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevoUsuario
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
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            txtConfirmPassword = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            label7 = new Label();
            txtUsername = new TextBox();
            cckIsActive = new CheckBox();
            txtName = new TextBox();
            label5 = new Label();
            label8 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            cmbRol = new ComboBox();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(cmbRol);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(txtUsername);
            panel1.Controls.Add(cckIsActive);
            panel1.Controls.Add(txtName);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(721, 469);
            panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtConfirmPassword);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtPassword);
            groupBox1.Controls.Add(label7);
            groupBox1.Location = new Point(45, 232);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(635, 174);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            groupBox1.Text = "Seguridad";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.Font = new Font("Segoe UI", 9.75F);
            txtConfirmPassword.Location = new Point(195, 103);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Size = new Size(407, 29);
            txtConfirmPassword.TabIndex = 41;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 110);
            label2.Name = "label2";
            label2.Size = new Size(143, 20);
            label2.TabIndex = 40;
            label2.Text = "Confirmar Password:";
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 9.75F);
            txtPassword.Location = new Point(195, 59);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(407, 29);
            txtPassword.TabIndex = 39;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(34, 66);
            label7.Name = "label7";
            label7.Size = new Size(73, 20);
            label7.TabIndex = 38;
            label7.Text = "Password:";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 9.75F);
            txtUsername.Location = new Point(243, 77);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(316, 29);
            txtUsername.TabIndex = 44;
            // 
            // cckIsActive
            // 
            cckIsActive.AutoSize = true;
            cckIsActive.Location = new Point(243, 202);
            cckIsActive.Name = "cckIsActive";
            cckIsActive.Size = new Size(73, 24);
            cckIsActive.TabIndex = 40;
            cckIsActive.Text = "Activo";
            cckIsActive.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 9.75F);
            txtName.Location = new Point(243, 113);
            txtName.Name = "txtName";
            txtName.Size = new Size(405, 29);
            txtName.TabIndex = 37;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(82, 122);
            label5.Name = "label5";
            label5.Size = new Size(67, 20);
            label5.TabIndex = 35;
            label5.Text = "Nombre:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(82, 86);
            label8.Name = "label8";
            label8.Size = new Size(62, 20);
            label8.TabIndex = 34;
            label8.Text = "Usuario:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 412);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(719, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(534, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(356, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(172, 35);
            btnSave.TabIndex = 6;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(719, 35);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(20, 5);
            label1.Name = "label1";
            label1.Size = new Size(111, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo usuario";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(683, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(82, 161);
            label3.Name = "label3";
            label3.Size = new Size(34, 20);
            label3.TabIndex = 46;
            label3.Text = "Rol:";
            // 
            // cmbRol
            // 
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(243, 153);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(405, 28);
            cmbRol.TabIndex = 47;
            // 
            // FrmNuevoUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(721, 469);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoUsuario";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo usuario";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private Button button2;
        private Button btnSave;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox cckIsActive;
        private TextBox txtPassword;
        private Label label7;
        private TextBox txtName;
        private Label label5;
        private Label label8;
        private TextBox txtUsername;
        private GroupBox groupBox1;
        private TextBox txtConfirmPassword;
        private Label label2;
        private Label label3;
        private ComboBox cmbRol;
    }
}