namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevoAleatorio
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            panel1 = new Panel();
            dataGridView2 = new DataGridView();
            panel3 = new Panel();
            button3 = new Button();
            groupBox1 = new GroupBox();
            label6 = new Label();
            textBox6 = new TextBox();
            comboBox1 = new ComboBox();
            label8 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton3 = new RadioButton();
            label2 = new Label();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            label3 = new Label();
            comboBox4 = new ComboBox();
            label4 = new Label();
            splitContainer1 = new SplitContainer();
            Ubicacion = new DataGridViewTextBoxColumn();
            Ultimo = new DataGridViewTextBoxColumn();
            panel4 = new Panel();
            dataGridView1 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            button7 = new Button();
            button4 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel3.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(splitContainer1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1676, 915);
            panel1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle3.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Ubicacion, Ultimo });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(523, 477);
            dataGridView2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(groupBox1);
            panel3.Controls.Add(comboBox1);
            panel3.Controls.Add(label8);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 35);
            panel3.Name = "panel3";
            panel3.Size = new Size(1674, 346);
            panel3.TabIndex = 44;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.search2;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(45, 175);
            button3.Name = "button3";
            button3.Size = new Size(229, 35);
            button3.TabIndex = 47;
            button3.Text = "Buscar ubicaciones";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(comboBox4);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(comboBox3);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(textBox6);
            groupBox1.Location = new Point(23, 82);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1289, 232);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtro";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(43, 67);
            label6.Name = "label6";
            label6.Size = new Size(78, 20);
            label6.TabIndex = 38;
            label6.Text = "Ubicación:";
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.FixedSingle;
            textBox6.Font = new Font("Segoe UI", 9.75F);
            textBox6.Location = new Point(140, 63);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(161, 29);
            textBox6.TabIndex = 37;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 9.75F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(184, 36);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(465, 29);
            comboBox1.TabIndex = 42;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(23, 44);
            label8.Name = "label8";
            label8.Size = new Size(62, 20);
            label8.TabIndex = 34;
            label8.Text = "Auditor:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 858);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1674, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1490, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.save;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(1247, 8);
            button1.Name = "button1";
            button1.Size = new Size(237, 35);
            button1.TabIndex = 6;
            button1.Text = "Crear Nuevo Inventario";
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
            panel2.Size = new Size(1674, 35);
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
            label1.Size = new Size(128, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo Inventario";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1637, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(45, 31);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(70, 24);
            radioButton1.TabIndex = 49;
            radioButton1.TabStop = true;
            radioButton1.Text = "Todos";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(552, 26);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(61, 24);
            radioButton2.TabIndex = 50;
            radioButton2.TabStop = true;
            radioButton2.Text = "Rack";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(396, 26);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(85, 24);
            radioButton3.TabIndex = 51;
            radioButton3.TabStop = true;
            radioButton3.Text = "No Rack";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(393, 70);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 52;
            label2.Text = "Rack:";
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 9.75F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(470, 58);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(161, 29);
            comboBox2.TabIndex = 53;
            // 
            // comboBox3
            // 
            comboBox3.Font = new Font("Segoe UI", 9.75F);
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(470, 93);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(161, 29);
            comboBox3.TabIndex = 55;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(393, 102);
            label3.Name = "label3";
            label3.Size = new Size(66, 20);
            label3.TabIndex = 54;
            label3.Text = "Posición:";
            // 
            // comboBox4
            // 
            comboBox4.Font = new Font("Segoe UI", 9.75F);
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(470, 128);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(161, 29);
            comboBox4.TabIndex = 57;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(393, 132);
            label4.Name = "label4";
            label4.Size = new Size(52, 20);
            label4.TabIndex = 56;
            label4.Text = "Altura:";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 381);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridView2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(1674, 477);
            splitContainer1.SplitterDistance = 523;
            splitContainer1.TabIndex = 46;
            // 
            // Ubicacion
            // 
            Ubicacion.HeaderText = "Ubicación";
            Ubicacion.MinimumWidth = 6;
            Ubicacion.Name = "Ubicacion";
            Ubicacion.Width = 125;
            // 
            // Ultimo
            // 
            Ultimo.HeaderText = "Ultimo Inventario";
            Ultimo.MinimumWidth = 6;
            Ultimo.Name = "Ultimo";
            Ultimo.Width = 125;
            // 
            // panel4
            // 
            panel4.Controls.Add(button7);
            panel4.Controls.Add(button4);
            panel4.Dock = DockStyle.Left;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(80, 477);
            panel4.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle4.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(80, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1067, 477);
            dataGridView1.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Ubicación";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Ultimo Inventario";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // button7
            // 
            button7.BackColor = Color.LightSalmon;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Bookman Old Style", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ImageAlign = ContentAlignment.MiddleLeft;
            button7.Location = new Point(13, 61);
            button7.Name = "button7";
            button7.Size = new Size(56, 48);
            button7.TabIndex = 6;
            button7.Text = "<";
            button7.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.LightGreen;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Bookman Old Style", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(13, 6);
            button4.Name = "button4";
            button4.Size = new Size(56, 48);
            button4.TabIndex = 5;
            button4.Text = ">";
            button4.UseVisualStyleBackColor = false;
            // 
            // FrmNuevoAleatorio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1676, 915);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoAleatorio";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel4.ResumeLayout(false);
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
        private ComboBox comboBox1;
        private Label label8;
        private Panel panel3;
        private GroupBox groupBox1;
        private Label label6;
        private TextBox textBox6;
        private Button button3;
        private DataGridView dataGridView2;
        private RadioButton radioButton1;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private Label label2;
        private ComboBox comboBox4;
        private Label label4;
        private ComboBox comboBox3;
        private Label label3;
        private ComboBox comboBox2;
        private SplitContainer splitContainer1;
        private DataGridViewTextBoxColumn Ubicacion;
        private DataGridViewTextBoxColumn Ultimo;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private Panel panel4;
        private Button button7;
        private Button button4;
    }
}