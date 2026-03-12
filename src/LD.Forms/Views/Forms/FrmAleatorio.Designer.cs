using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmAleatorio
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
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label2 = new Label();
            dateTimePicker2 = new DateTimePicker();
            button6 = new Button();
            dataGridView1 = new DataGridView();
            Fecha = new DataGridViewTextBoxColumn();
            Almacen = new DataGridViewTextBoxColumn();
            Turno = new DataGridViewTextBoxColumn();
            splitContainer1 = new SplitContainer();
            groupBox1 = new GroupBox();
            panel2 = new Panel();
            button1 = new Button();
            splitContainer2 = new SplitContainer();
            groupBox2 = new GroupBox();
            dataGridView2 = new DataGridView();
            Ubicacion = new DataGridViewTextBoxColumn();
            Tomada = new DataGridViewCheckBoxColumn();
            Teoric = new DataGridViewTextBoxColumn();
            F = new DataGridViewTextBoxColumn();
            Retul = new DataGridViewTextBoxColumn();
            REs2 = new DataGridViewTextBoxColumn();
            Resultado = new DataGridViewTextBoxColumn();
            groupBox3 = new GroupBox();
            dataGridView3 = new DataGridView();
            NoParte = new DataGridViewTextBoxColumn();
            esc = new DataGridViewCheckBoxColumn();
            groupBox4 = new GroupBox();
            dataGridView4 = new DataGridView();
            splitContainer3 = new SplitContainer();
            Auditor = new DataGridViewTextBoxColumn();
            button3 = new Button();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1624, 66);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(dateTimePicker1);
            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(dateTimePicker2);
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(30, 20, 0, 0);
            flowLayoutPanel1.Size = new Size(1039, 66);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 20);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 5, 0, 0);
            label1.Size = new Size(54, 25);
            label1.TabIndex = 0;
            label1.Text = "Desde:";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(93, 23);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(157, 27);
            dateTimePicker1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(256, 20);
            label2.Name = "label2";
            label2.Padding = new Padding(0, 5, 0, 0);
            label2.Size = new Size(50, 25);
            label2.TabIndex = 7;
            label2.Text = "Hasta:";
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(312, 23);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(157, 27);
            dateTimePicker2.TabIndex = 8;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(475, 23);
            button6.Name = "button6";
            button6.Size = new Size(124, 35);
            button6.TabIndex = 6;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle5.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Fecha, Almacen, Turno });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 23);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1618, 226);
            dataGridView1.TabIndex = 1;
            // 
            // Fecha
            // 
            Fecha.HeaderText = "Fecha";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 125;
            // 
            // Almacen
            // 
            Almacen.HeaderText = "Almacen";
            Almacen.MinimumWidth = 6;
            Almacen.Name = "Almacen";
            Almacen.Width = 125;
            // 
            // Turno
            // 
            Turno.HeaderText = "Turno";
            Turno.MinimumWidth = 6;
            Turno.Name = "Turno";
            Turno.Width = 125;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 66);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            splitContainer1.Panel1.Controls.Add(panel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1624, 812);
            splitContainer1.SplitterDistance = 294;
            splitContainer1.TabIndex = 2;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1624, 252);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Fechas";
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(button3);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 252);
            panel2.Name = "panel2";
            panel2.Size = new Size(1624, 42);
            panel2.TabIndex = 1;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.editar;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(212, 3);
            button1.Name = "button1";
            button1.Size = new Size(129, 35);
            button1.TabIndex = 7;
            button1.Text = "Editar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox3);
            splitContainer2.Size = new Size(1624, 514);
            splitContainer2.SplitterDistance = 1016;
            splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1016, 324);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Ubicaciones";
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle6.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Ubicacion, Tomada, Teoric, F, Retul, REs2, Resultado });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 23);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(1010, 298);
            dataGridView2.TabIndex = 2;
            // 
            // Ubicacion
            // 
            Ubicacion.HeaderText = "Ubicación";
            Ubicacion.MinimumWidth = 6;
            Ubicacion.Name = "Ubicacion";
            Ubicacion.Width = 125;
            // 
            // Tomada
            // 
            Tomada.HeaderText = "Tomada";
            Tomada.MinimumWidth = 6;
            Tomada.Name = "Tomada";
            Tomada.Width = 125;
            // 
            // Teoric
            // 
            Teoric.HeaderText = "Teorico";
            Teoric.MinimumWidth = 6;
            Teoric.Name = "Teoric";
            Teoric.Width = 80;
            // 
            // F
            // 
            F.HeaderText = "Fisico";
            F.MinimumWidth = 6;
            F.Name = "F";
            F.Width = 80;
            // 
            // Retul
            // 
            Retul.HeaderText = "Resultado primera toma";
            Retul.MinimumWidth = 6;
            Retul.Name = "Retul";
            Retul.Width = 150;
            // 
            // REs2
            // 
            REs2.HeaderText = "Resultado segunda toma";
            REs2.MinimumWidth = 6;
            REs2.Name = "REs2";
            REs2.Width = 150;
            // 
            // Resultado
            // 
            Resultado.HeaderText = "Resultado final";
            Resultado.MinimumWidth = 6;
            Resultado.Name = "Resultado";
            Resultado.Width = 150;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dataGridView3);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(0, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(604, 514);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Resultados";
            // 
            // dataGridView3
            // 
            dataGridViewCellStyle7.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { NoParte, esc });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(3, 23);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(598, 488);
            dataGridView3.TabIndex = 3;
            // 
            // NoParte
            // 
            NoParte.HeaderText = "No. Parte";
            NoParte.MinimumWidth = 6;
            NoParte.Name = "NoParte";
            NoParte.Width = 125;
            // 
            // esc
            // 
            esc.HeaderText = "Escaneado";
            esc.MinimumWidth = 6;
            esc.Name = "esc";
            esc.Width = 125;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dataGridView4);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(0, 0);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1016, 186);
            groupBox4.TabIndex = 1;
            groupBox4.TabStop = false;
            groupBox4.Text = "Ubicaciones";
            // 
            // dataGridView4
            // 
            dataGridViewCellStyle8.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            dataGridView4.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[] { Auditor });
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(3, 23);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(1010, 160);
            dataGridView4.TabIndex = 2;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(groupBox4);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(groupBox2);
            splitContainer3.Size = new Size(1016, 514);
            splitContainer3.SplitterDistance = 186;
            splitContainer3.TabIndex = 2;
            // 
            // Auditor
            // 
            Auditor.HeaderText = "Auditor";
            Auditor.MinimumWidth = 6;
            Auditor.Name = "Auditor";
            Auditor.Width = 300;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.plusM;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.Size = new Size(203, 35);
            button3.TabIndex = 9;
            button3.Text = "Nuevo Inventario";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // FrmAleatorio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1624, 878);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmAleatorio";
            Text = "FrmPlantillaForm";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private DataGridView dataGridView1;
        private Button button6;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private DateTimePicker dateTimePicker1;
        private Label label1;
        private Label label2;
        private DateTimePicker dateTimePicker2;
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Almacen;
        private DataGridViewTextBoxColumn Turno;
        private SplitContainer splitContainer2;
        private GroupBox groupBox2;
        private DataGridView dataGridView2;
        private GroupBox groupBox3;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn NoParte;
        private DataGridViewCheckBoxColumn esc;
        private DataGridViewTextBoxColumn Ubicacion;
        private DataGridViewCheckBoxColumn Tomada;
        private DataGridViewTextBoxColumn Teoric;
        private DataGridViewTextBoxColumn F;
        private DataGridViewTextBoxColumn Retul;
        private DataGridViewTextBoxColumn REs2;
        private DataGridViewTextBoxColumn Resultado;
        private Button button1;
        private Panel panel2;
        private SplitContainer splitContainer3;
        private GroupBox groupBox4;
        private DataGridView dataGridView4;
        private DataGridViewTextBoxColumn Auditor;
        private Button button3;
    }
}