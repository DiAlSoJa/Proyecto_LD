using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmAuditar
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
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button6 = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            dataGridView1 = new DataGridView();
            Requisicion = new DataGridViewTextBoxColumn();
            Picking = new DataGridViewTextBoxColumn();
            Cliente = new DataGridViewTextBoxColumn();
            Prou = new DataGridViewTextBoxColumn();
            FEcha = new DataGridViewTextBoxColumn();
            Hora = new DataGridViewTextBoxColumn();
            dataGridView2 = new DataGridView();
            No = new DataGridViewTextBoxColumn();
            DEscr = new DataGridViewTextBoxColumn();
            Cantida = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Lote = new DataGridViewTextBoxColumn();
            Ubi = new DataGridViewTextBoxColumn();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            roundedButton1 = new LD.Forms.Controls.RoundedButton();
            roundedButton2 = new LD.Forms.Controls.RoundedButton();
            roundedButton3 = new LD.Forms.Controls.RoundedButton();
            roundedButton4 = new LD.Forms.Controls.RoundedButton();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1584, 65);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(21, 11, 0, 0);
            flowLayoutPanel1.Size = new Size(831, 65);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(24, 14);
            button6.Name = "button6";
            button6.Size = new Size(123, 35);
            button6.TabIndex = 5;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.plusM;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(153, 14);
            button1.Name = "button1";
            button1.Size = new Size(185, 35);
            button1.TabIndex = 0;
            button1.Text = "Nueva auditoria";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.correo_electronico;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(344, 14);
            button2.Name = "button2";
            button2.Size = new Size(154, 35);
            button2.TabIndex = 6;
            button2.Text = "Enviar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.editar;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(504, 14);
            button3.Name = "button3";
            button3.Size = new Size(154, 35);
            button3.TabIndex = 2;
            button3.Text = "Editar";
            button3.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 65);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = SystemColors.ButtonHighlight;
            splitContainer1.Panel2.Controls.Add(roundedButton4);
            splitContainer1.Panel2.Controls.Add(roundedButton3);
            splitContainer1.Panel2.Controls.Add(roundedButton2);
            splitContainer1.Panel2.Controls.Add(roundedButton1);
            splitContainer1.Panel2.Controls.Add(pictureBox1);
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Size = new Size(1584, 892);
            splitContainer1.SplitterDistance = 916;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dataGridView2);
            splitContainer2.Size = new Size(916, 892);
            splitContainer2.SplitterDistance = 449;
            splitContainer2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle9.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Requisicion, Picking, Cliente, Prou, FEcha, Hora });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(916, 449);
            dataGridView1.TabIndex = 1;
            // 
            // Requisicion
            // 
            Requisicion.HeaderText = "Requisición";
            Requisicion.MinimumWidth = 6;
            Requisicion.Name = "Requisicion";
            Requisicion.Width = 125;
            // 
            // Picking
            // 
            Picking.HeaderText = "Picking";
            Picking.MinimumWidth = 6;
            Picking.Name = "Picking";
            Picking.Width = 125;
            // 
            // Cliente
            // 
            Cliente.HeaderText = "Cliente";
            Cliente.MinimumWidth = 6;
            Cliente.Name = "Cliente";
            Cliente.Width = 200;
            // 
            // Prou
            // 
            Prou.HeaderText = "Proyecto";
            Prou.MinimumWidth = 6;
            Prou.Name = "Prou";
            Prou.Width = 200;
            // 
            // FEcha
            // 
            FEcha.HeaderText = "Fecha";
            FEcha.MinimumWidth = 6;
            FEcha.Name = "FEcha";
            FEcha.Width = 80;
            // 
            // Hora
            // 
            Hora.HeaderText = "Hora";
            Hora.MinimumWidth = 6;
            Hora.Name = "Hora";
            Hora.Width = 80;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle10.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { No, DEscr, Cantida, Status, Lote, Ubi });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(916, 439);
            dataGridView2.TabIndex = 2;
            // 
            // No
            // 
            No.HeaderText = "No Parte";
            No.MinimumWidth = 6;
            No.Name = "No";
            No.Width = 125;
            // 
            // DEscr
            // 
            DEscr.HeaderText = "Descripción";
            DEscr.MinimumWidth = 6;
            DEscr.Name = "DEscr";
            DEscr.Width = 125;
            // 
            // Cantida
            // 
            Cantida.HeaderText = "Cantidad";
            Cantida.MinimumWidth = 6;
            Cantida.Name = "Cantida";
            Cantida.Width = 125;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.Width = 125;
            // 
            // Lote
            // 
            Lote.HeaderText = "Lote";
            Lote.MinimumWidth = 6;
            Lote.Name = "Lote";
            Lote.Width = 125;
            // 
            // Ubi
            // 
            Ubi.HeaderText = "Ubicación";
            Ubi.MinimumWidth = 6;
            Ubi.Name = "Ubi";
            Ubi.Width = 125;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Truck;
            pictureBox1.Location = new Point(26, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(622, 881);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(16, 892);
            panel2.TabIndex = 0;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.Location = new Point(128, 171);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(201, 59);
            roundedButton1.TabIndex = 5;
            roundedButton1.Text = "20260101-2000\r\n# 5226200222520\r\n\r\n";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.White;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.ForeColor = Color.Black;
            roundedButton2.Location = new Point(348, 171);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Size = new Size(201, 59);
            roundedButton2.TabIndex = 6;
            roundedButton2.Text = "20260101-2000\r\n# 5226200222520\r\n\r\n";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // roundedButton3
            // 
            roundedButton3.BackColor = Color.White;
            roundedButton3.FlatAppearance.BorderSize = 0;
            roundedButton3.FlatStyle = FlatStyle.Flat;
            roundedButton3.ForeColor = Color.Black;
            roundedButton3.Location = new Point(128, 236);
            roundedButton3.Name = "roundedButton3";
            roundedButton3.Size = new Size(201, 59);
            roundedButton3.TabIndex = 7;
            roundedButton3.Text = "20260101-2000\r\n# 5226200222520\r\n\r\n";
            roundedButton3.UseVisualStyleBackColor = false;
            // 
            // roundedButton4
            // 
            roundedButton4.BackColor = Color.White;
            roundedButton4.FlatAppearance.BorderSize = 0;
            roundedButton4.FlatStyle = FlatStyle.Flat;
            roundedButton4.ForeColor = Color.Black;
            roundedButton4.Location = new Point(348, 236);
            roundedButton4.Name = "roundedButton4";
            roundedButton4.Size = new Size(201, 59);
            roundedButton4.TabIndex = 8;
            roundedButton4.Text = "20260101-2000\r\n# 5226200222520\r\n\r\n";
            roundedButton4.UseVisualStyleBackColor = false;
            // 
            // FrmAuditar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 957);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmAuditar";
            Text = "Usuarios";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private Button button3;
        private Button button6;
        private SplitContainer splitContainer1;
        private Panel panel2;
        private Button button2;
        private SplitContainer splitContainer2;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private DataGridViewTextBoxColumn Requisicion;
        private DataGridViewTextBoxColumn Picking;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Prou;
        private DataGridViewTextBoxColumn FEcha;
        private DataGridViewTextBoxColumn Hora;
        private DataGridViewTextBoxColumn No;
        private DataGridViewTextBoxColumn DEscr;
        private DataGridViewTextBoxColumn Cantida;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Lote;
        private DataGridViewTextBoxColumn Ubi;
        private PictureBox pictureBox1;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton roundedButton4;
        private Controls.RoundedButton roundedButton3;
        private Controls.RoundedButton roundedButton2;
    }
}