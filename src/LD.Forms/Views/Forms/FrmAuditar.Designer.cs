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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
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
            flowLayoutPanel2 = new FlowLayoutPanel();
            button21 = new Button();
            button22 = new Button();
            button23 = new Button();
            flowLayoutPanel3 = new FlowLayoutPanel();
            button24 = new Button();
            button25 = new Button();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
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
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1386, 49);
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
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(18, 8, 0, 0);
            flowLayoutPanel1.Size = new Size(727, 49);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(21, 10);
            button6.Margin = new Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new Size(108, 26);
            button6.TabIndex = 5;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.plusM;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(135, 10);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(162, 26);
            button1.TabIndex = 0;
            button1.Text = "Nueva auditoria";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.correo_electronico;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(303, 10);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(135, 26);
            button2.TabIndex = 6;
            button2.Text = "Enviar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.editar;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(444, 10);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(135, 26);
            button3.TabIndex = 2;
            button3.Text = "Editar";
            button3.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 49);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = SystemColors.ButtonHighlight;
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel2);
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel3);
            splitContainer1.Panel2.Controls.Add(pictureBox1);
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Size = new Size(1386, 669);
            splitContainer1.SplitterDistance = 802;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(3, 2, 3, 2);
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
            splitContainer2.Size = new Size(802, 669);
            splitContainer2.SplitterDistance = 337;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Requisicion, Picking, Cliente, Prou, FEcha, Hora });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(802, 337);
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
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { No, DEscr, Cantida, Status, Lote, Ubi });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Margin = new Padding(3, 2, 3, 2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(802, 329);
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
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel2.BackColor = Color.FromArgb(226, 131, 47);
            flowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel2.Controls.Add(button21);
            flowLayoutPanel2.Controls.Add(button22);
            flowLayoutPanel2.Controls.Add(button23);
            flowLayoutPanel2.Location = new Point(101, 128);
            flowLayoutPanel2.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Padding = new Padding(4, 0, 0, 0);
            flowLayoutPanel2.Size = new Size(194, 505);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // button21
            // 
            button21.FlatStyle = FlatStyle.Flat;
            button21.Location = new Point(7, 2);
            button21.Margin = new Padding(3, 2, 3, 2);
            button21.Name = "button21";
            button21.Size = new Size(159, 28);
            button21.TabIndex = 16;
            button21.Text = "202601012-002025";
            button21.UseVisualStyleBackColor = true;
            // 
            // button22
            // 
            button22.Location = new Point(7, 34);
            button22.Margin = new Padding(3, 2, 3, 2);
            button22.Name = "button22";
            button22.Size = new Size(159, 28);
            button22.TabIndex = 17;
            button22.Text = "202601012-002025";
            button22.UseVisualStyleBackColor = true;
            // 
            // button23
            // 
            button23.Location = new Point(7, 66);
            button23.Margin = new Padding(3, 2, 3, 2);
            button23.Name = "button23";
            button23.Size = new Size(159, 28);
            button23.TabIndex = 18;
            button23.Text = "202601012-002025";
            button23.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoScroll = true;
            flowLayoutPanel3.BackColor = Color.FromArgb(226, 131, 47);
            flowLayoutPanel3.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel3.Controls.Add(button24);
            flowLayoutPanel3.Controls.Add(button25);
            flowLayoutPanel3.Location = new Point(299, 128);
            flowLayoutPanel3.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Padding = new Padding(4, 0, 0, 0);
            flowLayoutPanel3.Size = new Size(194, 505);
            flowLayoutPanel3.TabIndex = 2;
            // 
            // button24
            // 
            button24.Location = new Point(7, 2);
            button24.Margin = new Padding(3, 2, 3, 2);
            button24.Name = "button24";
            button24.Size = new Size(159, 28);
            button24.TabIndex = 5;
            button24.Text = "202601012-002025";
            button24.UseVisualStyleBackColor = true;
            // 
            // button25
            // 
            button25.Location = new Point(7, 34);
            button25.Margin = new Padding(3, 2, 3, 2);
            button25.Name = "button25";
            button25.Size = new Size(159, 28);
            button25.TabIndex = 6;
            button25.Text = "202601012-002025";
            button25.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Truck;
            pictureBox1.Location = new Point(23, 7);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(544, 661);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(14, 669);
            panel2.TabIndex = 0;
            // 
            // FrmAuditar
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1386, 718);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
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
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel3.ResumeLayout(false);
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
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button button24;
        private Button button25;
        private Button button21;
        private Button button22;
        private Button button23;
    }
}