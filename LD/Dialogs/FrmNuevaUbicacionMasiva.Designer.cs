namespace LD.Dialogs
{
    partial class FrmNuevaUbicacionMasiva
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
            panel4 = new Panel();
            dataGridView1 = new DataGridView();
            panel3 = new Panel();
            button4 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            Almacen = new DataGridViewTextBoxColumn();
            Rack = new DataGridViewTextBoxColumn();
            Pasillo = new DataGridViewTextBoxColumn();
            Nivel = new DataGridViewTextBoxColumn();
            Ubicacion = new DataGridViewTextBoxColumn();
            Dimensio = new DataGridViewTextBoxColumn();
            Usado = new DataGridViewCheckBoxColumn();
            General = new DataGridViewCheckBoxColumn();
            Recib = new DataGridViewCheckBoxColumn();
            Cuarentena = new DataGridViewCheckBoxColumn();
            Embarque = new DataGridViewCheckBoxColumn();
            RackC = new DataGridViewCheckBoxColumn();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1420, 549);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // panel4
            // 
            panel4.Controls.Add(dataGridView1);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 87);
            panel4.Name = "panel4";
            panel4.Size = new Size(1418, 405);
            panel4.TabIndex = 10;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Almacen, Rack, Pasillo, Nivel, Ubicacion, Dimensio, Usado, General, Recib, Cuarentena, Embarque, RackC });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1418, 405);
            dataGridView1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(button4);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 35);
            panel3.Name = "panel3";
            panel3.Size = new Size(1418, 52);
            panel3.TabIndex = 9;
            // 
            // button4
            // 
            button4.Image = Properties.Resources.pegar;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(20, 11);
            button4.Name = "button4";
            button4.Size = new Size(148, 35);
            button4.TabIndex = 4;
            button4.Text = "Pegar";
            button4.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 492);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1418, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1249, 8);
            button2.Name = "button2";
            button2.Size = new Size(156, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.save;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(1071, 8);
            button1.Name = "button1";
            button1.Size = new Size(172, 35);
            button1.TabIndex = 6;
            button1.Text = "Guardar";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.ForestGreen;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1418, 35);
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
            label1.Location = new Point(20, 5);
            label1.Name = "label1";
            label1.Size = new Size(177, 22);
            label1.TabIndex = 3;
            label1.Text = "Nueva ubicación másiva";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1382, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // Almacen
            // 
            Almacen.HeaderText = "Almacén";
            Almacen.MinimumWidth = 6;
            Almacen.Name = "Almacen";
            Almacen.Width = 125;
            // 
            // Rack
            // 
            Rack.HeaderText = "Rack";
            Rack.MinimumWidth = 6;
            Rack.Name = "Rack";
            Rack.Width = 125;
            // 
            // Pasillo
            // 
            Pasillo.HeaderText = "Pasillo";
            Pasillo.MinimumWidth = 6;
            Pasillo.Name = "Pasillo";
            Pasillo.Width = 125;
            // 
            // Nivel
            // 
            Nivel.HeaderText = "Nivel";
            Nivel.MinimumWidth = 6;
            Nivel.Name = "Nivel";
            Nivel.Width = 125;
            // 
            // Ubicacion
            // 
            Ubicacion.HeaderText = "Ubicación";
            Ubicacion.MinimumWidth = 6;
            Ubicacion.Name = "Ubicacion";
            Ubicacion.Width = 125;
            // 
            // Dimensio
            // 
            Dimensio.HeaderText = "Dimensión";
            Dimensio.MinimumWidth = 6;
            Dimensio.Name = "Dimensio";
            Dimensio.Width = 125;
            // 
            // Usado
            // 
            Usado.HeaderText = "Usado";
            Usado.MinimumWidth = 6;
            Usado.Name = "Usado";
            Usado.Width = 70;
            // 
            // General
            // 
            General.HeaderText = "General";
            General.MinimumWidth = 6;
            General.Name = "General";
            General.Width = 80;
            // 
            // Recib
            // 
            Recib.HeaderText = "Recibo";
            Recib.MinimumWidth = 6;
            Recib.Name = "Recib";
            Recib.Width = 60;
            // 
            // Cuarentena
            // 
            Cuarentena.HeaderText = "Cuarentena";
            Cuarentena.MinimumWidth = 6;
            Cuarentena.Name = "Cuarentena";
            Cuarentena.Width = 125;
            // 
            // Embarque
            // 
            Embarque.HeaderText = "Embarque";
            Embarque.MinimumWidth = 6;
            Embarque.Name = "Embarque";
            Embarque.Width = 80;
            // 
            // RackC
            // 
            RackC.HeaderText = "Rack";
            RackC.MinimumWidth = 6;
            RackC.Name = "RackC";
            RackC.Width = 80;
            // 
            // FrmNuevaUbicacionMasiva
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1420, 549);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevaUbicacionMasiva";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel3.ResumeLayout(false);
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
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Panel panel4;
        private DataGridView dataGridView1;
        private Panel panel3;
        private Button button4;
        private DataGridViewTextBoxColumn Almacen;
        private DataGridViewTextBoxColumn Rack;
        private DataGridViewTextBoxColumn Pasillo;
        private DataGridViewTextBoxColumn Nivel;
        private DataGridViewTextBoxColumn Ubicacion;
        private DataGridViewTextBoxColumn Dimensio;
        private DataGridViewCheckBoxColumn Usado;
        private DataGridViewCheckBoxColumn General;
        private DataGridViewCheckBoxColumn Recib;
        private DataGridViewCheckBoxColumn Cuarentena;
        private DataGridViewCheckBoxColumn Embarque;
        private DataGridViewCheckBoxColumn RackC;
    }
}