namespace LD.Forms.Views.Dialogs
{
    partial class FrmRoles
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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panelContainer = new Panel();
            gridPermisos = new DataGridView();
            gridRoles = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            editBtn = new Button();
            addBtn = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridPermisos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridRoles).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panelContainer);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1087, 569);
            panel1.TabIndex = 0;
            // 
            // panelContainer
            // 
            panelContainer.Controls.Add(gridPermisos);
            panelContainer.Controls.Add(gridRoles);
            panelContainer.Location = new Point(19, 60);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1042, 435);
            panelContainer.TabIndex = 11;
            // 
            // gridPermisos
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            gridPermisos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            gridPermisos.BackgroundColor = SystemColors.ButtonHighlight;
            gridPermisos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridPermisos.Dock = DockStyle.Right;
            gridPermisos.Location = new Point(550, 0);
            gridPermisos.Name = "gridPermisos";
            gridPermisos.RowHeadersWidth = 51;
            gridPermisos.Size = new Size(492, 435);
            gridPermisos.TabIndex = 11;
            // 
            // gridRoles
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            gridRoles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            gridRoles.BackgroundColor = SystemColors.ButtonHighlight;
            gridRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridRoles.Dock = DockStyle.Left;
            gridRoles.Location = new Point(0, 0);
            gridRoles.Name = "gridRoles";
            gridRoles.RowHeadersWidth = 51;
            gridRoles.Size = new Size(464, 435);
            gridRoles.TabIndex = 10;
            gridRoles.SelectionChanged += gridRoles_SelectionChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(editBtn);
            flowLayoutPanel1.Controls.Add(addBtn);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 512);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1085, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(920, 8);
            button2.Name = "button2";
            button2.Size = new Size(152, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // editBtn
            // 
            editBtn.Image = Properties.Resources.editar;
            editBtn.ImageAlign = ContentAlignment.MiddleLeft;
            editBtn.Location = new Point(807, 8);
            editBtn.Name = "editBtn";
            editBtn.Size = new Size(107, 35);
            editBtn.TabIndex = 10;
            editBtn.Text = "Editar rol";
            editBtn.UseVisualStyleBackColor = true;
            editBtn.Click += editBtn_Click;
            // 
            // addBtn
            // 
            addBtn.Image = Properties.Resources.plusM;
            addBtn.ImageAlign = ContentAlignment.MiddleLeft;
            addBtn.Location = new Point(664, 8);
            addBtn.Name = "addBtn";
            addBtn.Size = new Size(137, 35);
            addBtn.TabIndex = 9;
            addBtn.Text = "Nuevo rol";
            addBtn.UseVisualStyleBackColor = true;
            addBtn.Click += addBtn_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1085, 35);
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
            label1.Size = new Size(49, 22);
            label1.TabIndex = 3;
            label1.Text = "Roles";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1049, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmRoles
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1087, 569);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmRoles";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo usuario";
            panel1.ResumeLayout(false);
            panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridPermisos).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridRoles).EndInit();
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
        private FlowLayoutPanel flowLayoutPanel1;
        private DataGridView gridRoles;
        private Panel panelContainer;
        private DataGridView gridPermisos;
        private Button editBtn;
        private Button addBtn;
    }
}