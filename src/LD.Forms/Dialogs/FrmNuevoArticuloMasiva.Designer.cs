namespace LD.Dialogs
{
    partial class FrmNuevoArticuloMasiva
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panel1 = new Panel();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            label5 = new Label();
            label8 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel3 = new Panel();
            button4 = new Button();
            dataGridView1 = new DataGridView();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            NumPArte = new DataGridViewTextBoxColumn();
            Descripcion = new DataGridViewTextBoxColumn();
            FIFO = new DataGridViewCheckBoxColumn();
            LIFO = new DataGridViewCheckBoxColumn();
            NumeroLote = new DataGridViewCheckBoxColumn();
            FechaCaducidad = new DataGridViewCheckBoxColumn();
            Unidadmin = new DataGridViewTextBoxColumn();
            Unidadmed = new DataGridViewTextBoxColumn();
            UnidadMax = new DataGridViewTextBoxColumn();
            Solicitarnumerolote = new DataGridViewCheckBoxColumn();
            Solicitafechacad = new DataGridViewCheckBoxColumn();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1414, 576);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 9.75F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(174, 60);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(465, 29);
            comboBox2.TabIndex = 43;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 9.75F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(174, 25);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(465, 29);
            comboBox1.TabIndex = 42;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 69);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 35;
            label5.Text = "Proyecto:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(13, 33);
            label8.Name = "label8";
            label8.Size = new Size(58, 20);
            label8.TabIndex = 34;
            label8.Text = "Cliente:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 519);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1412, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image =Forms.Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1227, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image =Forms.Properties.Resources.save;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(1049, 8);
            button1.Name = "button1";
            button1.Size = new Size(172, 35);
            button1.TabIndex = 6;
            button1.Text = "Guardar";
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
            panel2.Size = new Size(1412, 35);
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
            label1.Size = new Size(103, 22);
            label1.TabIndex = 3;
            label1.Text = "Carga masiva";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image =Forms.Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1376, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(button4);
            panel3.Controls.Add(comboBox1);
            panel3.Controls.Add(comboBox2);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 35);
            panel3.Name = "panel3";
            panel3.Size = new Size(1412, 164);
            panel3.TabIndex = 44;
            // 
            // button4
            // 
            button4.Image =Forms.Properties.Resources.pegar;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(20, 123);
            button4.Name = "button4";
            button4.Size = new Size(148, 35);
            button4.TabIndex = 44;
            button4.Text = "Pegar";
            button4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle3.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Cliente, Proyecto, NumPArte, Descripcion, FIFO, LIFO, NumeroLote, FechaCaducidad, Unidadmin, Unidadmed, UnidadMax, Solicitarnumerolote, Solicitafechacad });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 199);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1412, 320);
            dataGridView1.TabIndex = 45;
            // 
            // Cliente
            // 
            Cliente.HeaderText = "Cliente";
            Cliente.MinimumWidth = 6;
            Cliente.Name = "Cliente";
            Cliente.Width = 200;
            // 
            // Proyecto
            // 
            Proyecto.HeaderText = "Proyecto";
            Proyecto.MinimumWidth = 6;
            Proyecto.Name = "Proyecto";
            Proyecto.Width = 200;
            // 
            // NumPArte
            // 
            NumPArte.HeaderText = "No. Parte";
            NumPArte.MinimumWidth = 6;
            NumPArte.Name = "NumPArte";
            NumPArte.Width = 125;
            // 
            // Descripcion
            // 
            Descripcion.HeaderText = "Descripción";
            Descripcion.MinimumWidth = 6;
            Descripcion.Name = "Descripcion";
            Descripcion.Width = 125;
            // 
            // FIFO
            // 
            FIFO.HeaderText = "FIFO";
            FIFO.MinimumWidth = 6;
            FIFO.Name = "FIFO";
            FIFO.Resizable = DataGridViewTriState.True;
            FIFO.SortMode = DataGridViewColumnSortMode.Automatic;
            FIFO.Width = 60;
            // 
            // LIFO
            // 
            LIFO.HeaderText = "LIFO";
            LIFO.MinimumWidth = 6;
            LIFO.Name = "LIFO";
            LIFO.Resizable = DataGridViewTriState.True;
            LIFO.SortMode = DataGridViewColumnSortMode.Automatic;
            LIFO.Width = 60;
            // 
            // NumeroLote
            // 
            NumeroLote.HeaderText = "Número de Lote";
            NumeroLote.MinimumWidth = 6;
            NumeroLote.Name = "NumeroLote";
            NumeroLote.Resizable = DataGridViewTriState.True;
            NumeroLote.SortMode = DataGridViewColumnSortMode.Automatic;
            NumeroLote.Width = 80;
            // 
            // FechaCaducidad
            // 
            FechaCaducidad.HeaderText = "Fecha de caducidad";
            FechaCaducidad.MinimumWidth = 6;
            FechaCaducidad.Name = "FechaCaducidad";
            FechaCaducidad.Resizable = DataGridViewTriState.True;
            FechaCaducidad.SortMode = DataGridViewColumnSortMode.Automatic;
            FechaCaducidad.Width = 80;
            // 
            // Unidadmin
            // 
            Unidadmin.HeaderText = "Unidad Mínima";
            Unidadmin.MinimumWidth = 6;
            Unidadmin.Name = "Unidadmin";
            Unidadmin.Width = 80;
            // 
            // Unidadmed
            // 
            Unidadmed.HeaderText = "Unidad Media";
            Unidadmed.MinimumWidth = 6;
            Unidadmed.Name = "Unidadmed";
            Unidadmed.Width = 80;
            // 
            // UnidadMax
            // 
            UnidadMax.HeaderText = "Unidad Máxima";
            UnidadMax.MinimumWidth = 6;
            UnidadMax.Name = "UnidadMax";
            UnidadMax.Width = 80;
            // 
            // Solicitarnumerolote
            // 
            Solicitarnumerolote.HeaderText = "Solicitar Número de Lote";
            Solicitarnumerolote.MinimumWidth = 6;
            Solicitarnumerolote.Name = "Solicitarnumerolote";
            Solicitarnumerolote.Resizable = DataGridViewTriState.True;
            Solicitarnumerolote.SortMode = DataGridViewColumnSortMode.Automatic;
            Solicitarnumerolote.Width = 80;
            // 
            // Solicitafechacad
            // 
            Solicitafechacad.HeaderText = "Solicitar Fecha de Caducidad";
            Solicitafechacad.MinimumWidth = 6;
            Solicitafechacad.Name = "Solicitafechacad";
            Solicitafechacad.Resizable = DataGridViewTriState.True;
            Solicitafechacad.SortMode = DataGridViewColumnSortMode.Automatic;
            Solicitafechacad.Width = 80;
            // 
            // FrmNuevoArticuloMasiva
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1414, 576);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoArticuloMasiva";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
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
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Label label5;
        private Label label8;
        private Panel panel3;
        private Button button4;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn NumPArte;
        private DataGridViewTextBoxColumn Descripcion;
        private DataGridViewCheckBoxColumn FIFO;
        private DataGridViewCheckBoxColumn LIFO;
        private DataGridViewCheckBoxColumn NumeroLote;
        private DataGridViewCheckBoxColumn FechaCaducidad;
        private DataGridViewTextBoxColumn Unidadmin;
        private DataGridViewTextBoxColumn Unidadmed;
        private DataGridViewTextBoxColumn UnidadMax;
        private DataGridViewCheckBoxColumn Solicitarnumerolote;
        private DataGridViewCheckBoxColumn Solicitafechacad;
    }
}