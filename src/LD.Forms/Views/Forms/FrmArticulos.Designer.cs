using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmArticulos
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
            button6 = new Button();
            button1 = new Button();
            button3 = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            button5 = new Button();
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
            button4.Location = new Point(422, 3);
            button4.Name = "button4";
            button4.Size = new Size(148, 35);
            button4.TabIndex = 4;
            button4.Text = "Carga masiva";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button6);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button3);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Controls.Add(button5);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(473, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(23, 3);
            button6.Name = "button6";
            button6.Size = new Size(124, 35);
            button6.TabIndex = 5;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.plusM;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(153, 3);
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
            button3.Location = new Point(266, 3);
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
            // button5
            // 
            button5.Image = Properties.Resources.editar;
            button5.ImageAlign = ContentAlignment.MiddleLeft;
            button5.Location = new Point(23, 50);
            button5.Name = "button5";
            button5.Size = new Size(107, 35);
            button5.TabIndex = 3;
            button5.Text = "Editar";
            button5.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Cliente, Proyecto, NumPArte, Descripcion, FIFO, LIFO, NumeroLote, FechaCaducidad, Unidadmin, Unidadmed, UnidadMax, Solicitarnumerolote, Solicitafechacad });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1496, 674);
            dataGridView1.TabIndex = 1;
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
            // gridContainer
            // 
            gridContainer.Controls.Add(dataGridView1);
            gridContainer.Dock = DockStyle.Fill;
            gridContainer.Location = new Point(0, 47);
            gridContainer.Name = "gridContainer";
            gridContainer.Size = new Size(1496, 674);
            gridContainer.TabIndex = 5;
            // 
            // FrmArticulos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmArticulos";
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
        private Button button4;
        private Button button6;
        private Button button5;
        private Panel gridContainer;
    }
}