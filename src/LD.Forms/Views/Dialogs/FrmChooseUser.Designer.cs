namespace LD.Forms.Views.Dialogs
{
    partial class FrmChooseUser
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            dataGridView4 = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            Turno = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(dataGridView4);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(783, 387);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 330);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(781, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(597, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image = Properties.Resources._checked;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(401, 8);
            button1.Name = "button1";
            button1.Size = new Size(190, 35);
            button1.TabIndex = 6;
            button1.Text = "Seleccionar";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(781, 35);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            panel2.MouseDown += panel1_MouseDown;
            panel2.MouseMove += panel1_MouseMove;
            panel2.MouseUp += panel1_MouseUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(21, 5);
            label1.Name = "label1";
            label1.Size = new Size(72, 22);
            label1.TabIndex = 3;
            label1.Text = "Usuarios";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(744, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // dataGridView4
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView4.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[] { Id, Nombre, Turno });
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(0, 35);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(781, 295);
            dataGridView4.TabIndex = 9;
            // 
            // Id
            // 
            Id.HeaderText = "Id";
            Id.MinimumWidth = 6;
            Id.Name = "Id";
            Id.Width = 125;
            // 
            // Nombre
            // 
            Nombre.HeaderText = "Nombre";
            Nombre.MinimumWidth = 6;
            Nombre.Name = "Nombre";
            Nombre.Width = 400;
            // 
            // Turno
            // 
            Turno.HeaderText = "Turno";
            Turno.MinimumWidth = 6;
            Turno.Name = "Turno";
            Turno.Width = 125;
            // 
            // FrmChooseUser
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 387);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmChooseUser";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private Button button2;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private DataGridView dataGridView4;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Turno;
    }
}