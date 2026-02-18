using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmProyectos
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
            button1 = new Button();
            button3 = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Activo = new DataGridViewCheckBoxColumn();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            Almacen = new DataGridViewTextBoxColumn();
            EscaneoDUB = new DataGridViewCheckBoxColumn();
            EscaneoNoParte = new DataGridViewCheckBoxColumn();
            EscaneoCantidad = new DataGridViewCheckBoxColumn();
            RequiereLote = new DataGridViewCheckBoxColumn();
            RequiereCaducidad = new DataGridViewCheckBoxColumn();
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
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1496, 47);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(399, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.plusM;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(23, 3);
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
            button3.Location = new Point(136, 3);
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
            flowLayoutPanel2.Location = new Point(23, 44);
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Activo, Cliente, Proyecto, Almacen, EscaneoDUB, EscaneoNoParte, EscaneoCantidad, RequiereLote, RequiereCaducidad });
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
            // Cliente
            // 
            Cliente.HeaderText = "Cliente";
            Cliente.MinimumWidth = 6;
            Cliente.Name = "Cliente";
            Cliente.Width = 125;
            // 
            // Proyecto
            // 
            Proyecto.HeaderText = "Proyecto";
            Proyecto.MinimumWidth = 6;
            Proyecto.Name = "Proyecto";
            Proyecto.Width = 125;
            // 
            // Almacen
            // 
            Almacen.HeaderText = "Almacen";
            Almacen.MinimumWidth = 6;
            Almacen.Name = "Almacen";
            Almacen.Width = 125;
            // 
            // EscaneoDUB
            // 
            EscaneoDUB.HeaderText = "EscaneoDUB";
            EscaneoDUB.MinimumWidth = 6;
            EscaneoDUB.Name = "EscaneoDUB";
            EscaneoDUB.Resizable = DataGridViewTriState.True;
            EscaneoDUB.SortMode = DataGridViewColumnSortMode.Automatic;
            EscaneoDUB.Width = 125;
            // 
            // EscaneoNoParte
            // 
            EscaneoNoParte.HeaderText = "Escaneo No. Parte";
            EscaneoNoParte.MinimumWidth = 6;
            EscaneoNoParte.Name = "EscaneoNoParte";
            EscaneoNoParte.Width = 125;
            // 
            // EscaneoCantidad
            // 
            EscaneoCantidad.HeaderText = "Escaneo cantidad";
            EscaneoCantidad.MinimumWidth = 6;
            EscaneoCantidad.Name = "EscaneoCantidad";
            EscaneoCantidad.Width = 125;
            // 
            // RequiereLote
            // 
            RequiereLote.HeaderText = "Requiere Lote";
            RequiereLote.MinimumWidth = 6;
            RequiereLote.Name = "RequiereLote";
            RequiereLote.Width = 125;
            // 
            // RequiereCaducidad
            // 
            RequiereCaducidad.HeaderText = "RequiereFechaCaducidad";
            RequiereCaducidad.MinimumWidth = 6;
            RequiereCaducidad.Name = "RequiereCaducidad";
            RequiereCaducidad.Width = 125;
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
            // FrmProyectos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmProyectos";
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
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn Almacen;
        private DataGridViewCheckBoxColumn EscaneoDUB;
        private DataGridViewCheckBoxColumn EscaneoNoParte;
        private DataGridViewCheckBoxColumn EscaneoCantidad;
        private DataGridViewCheckBoxColumn RequiereLote;
        private DataGridViewCheckBoxColumn RequiereCaducidad;
        private Panel gridContainer;
    }
}