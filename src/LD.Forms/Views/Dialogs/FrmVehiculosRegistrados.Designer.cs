namespace LD.Forms.Views.Dialogs
{
    partial class FrmVehiculosRegistrados
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
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            groupBox2 = new GroupBox();
            label3 = new Label();
            comboBox1 = new ComboBox();
            textBox1 = new TextBox();
            panel1 = new Panel();
            dataGridView1 = new DataGridView();
            FechaIn = new DataGridViewTextBoxColumn();
            Hora = new DataGridViewTextBoxColumn();
            PLACAS = new DataGridViewTextBoxColumn();
            Chofer = new DataGridViewTextBoxColumn();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(797, 35);
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
            label1.Size = new Size(157, 22);
            label1.TabIndex = 3;
            label1.Text = "Vehiculos y Choferes";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(760, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 579);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5, 5, 5, 5);
            flowLayoutPanel1.Size = new Size(797, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(613, 8);
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
            button1.Location = new Point(436, 8);
            button1.Name = "button1";
            button1.Size = new Size(171, 35);
            button1.TabIndex = 8;
            button1.Text = "Seleccionar";
            button1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(comboBox1);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 35);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(797, 140);
            groupBox2.TabIndex = 46;
            groupBox2.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(31, 39);
            label3.Name = "label3";
            label3.Size = new Size(110, 20);
            label3.TabIndex = 54;
            label3.Text = "Filtro de Horas:";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Últimas 24 hrs", "Últimos 2 días", "Últimos 3 días", "Últimos 5 días", "Últimos 10 días" });
            comboBox1.Location = new Point(157, 31);
            comboBox1.Margin = new Padding(3, 4, 3, 4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(138, 28);
            comboBox1.TabIndex = 53;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9.75F);
            textBox1.Location = new Point(31, 81);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Escriba para buscar";
            textBox1.Size = new Size(539, 29);
            textBox1.TabIndex = 51;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(dataGridView1);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(799, 636);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { FechaIn, Hora, PLACAS, Chofer });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 175);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(797, 404);
            dataGridView1.TabIndex = 52;
            // 
            // FechaIn
            // 
            FechaIn.HeaderText = "Fecha de Registro";
            FechaIn.MinimumWidth = 6;
            FechaIn.Name = "FechaIn";
            FechaIn.Width = 125;
            // 
            // Hora
            // 
            Hora.HeaderText = "Hora de registro";
            Hora.MinimumWidth = 6;
            Hora.Name = "Hora";
            Hora.Width = 125;
            // 
            // PLACAS
            // 
            PLACAS.HeaderText = "Placas";
            PLACAS.MinimumWidth = 6;
            PLACAS.Name = "PLACAS";
            PLACAS.Width = 125;
            // 
            // Chofer
            // 
            Chofer.HeaderText = "Chofer";
            Chofer.MinimumWidth = 6;
            Chofer.Name = "Chofer";
            Chofer.Width = 300;
            // 
            // FrmVehiculosRegistrados
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 636);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmVehiculosRegistrados";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            Load += FrmVehiculosRegistrados_Load;
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button2;
        private GroupBox groupBox2;
        private TextBox textBox1;
        private Panel panel1;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn FechaIn;
        private DataGridViewTextBoxColumn Hora;
        private DataGridViewTextBoxColumn PLACAS;
        private DataGridViewTextBoxColumn Chofer;
        private Label label3;
        private ComboBox comboBox1;
        private Button button1;
    }
}