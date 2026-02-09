using System;
using System.Windows.Forms;

namespace LD.Forms
{
    partial class FrmUbicaciones
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
            button4 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button1 = new Button();
            button3 = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Activo = new DataGridViewCheckBoxColumn();
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
            gridContainer = new Panel();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            gridContainer.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button4);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1496, 47);
            panel1.TabIndex = 0;
            // 
            // button4
            // 
            button4.Image = Properties.Resources.pegar;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(415, 3);
            button4.Name = "button4";
            button4.Size = new Size(148, 35);
            button4.TabIndex = 3;
            button4.Text = "Carga masiva";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(399, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.plusM;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(107, 35);
            button1.TabIndex = 0;
            button1.Text = "Nuevo";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.editar;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(116, 3);
            button3.Name = "button3";
            button3.Size = new Size(107, 35);
            button3.TabIndex = 2;
            button3.Text = "Editar";
            button3.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(button2);
            flowLayoutPanel2.Dock = DockStyle.Left;
            flowLayoutPanel2.Location = new Point(3, 44);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(399, 0);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.plusM;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(107, 35);
            button2.TabIndex = 0;
            button2.Text = "Nuevo";
            button2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Activo, Almacen, Rack, Pasillo, Nivel, Ubicacion, Dimensio, Usado, General, Recib, Cuarentena, Embarque, RackC });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1496, 674);
            dataGridView1.TabIndex = 1;
            // 
            // Activo
            // 
            Activo.HeaderText = "Activo";
            Activo.MinimumWidth = 6;
            Activo.Name = "Activo";
            Activo.Width = 50;
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
            // gridContainer
            // 
            gridContainer.Controls.Add(dataGridView1);
            gridContainer.Dock = DockStyle.Fill;
            gridContainer.Location = new Point(0, 47);
            gridContainer.Name = "gridContainer";
            gridContainer.Size = new Size(1496, 674);
            gridContainer.TabIndex = 4;
            // 
            // FrmUbicaciones
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUbicaciones";
            Text = "FrmPlantillaForm";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            gridContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private Button button3;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn Activo;
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
        private Button button4;
        private Panel gridContainer;
    }
}