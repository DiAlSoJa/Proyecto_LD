using System;
using System.Windows.Forms;

namespace LD.Forms
{
    partial class FrmClientes
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
            button6 = new Button();
            button1 = new Button();
            button3 = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Activo = new DataGridViewCheckBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            NombreComercial = new DataGridViewTextBoxColumn();
            RazonSocial = new DataGridViewTextBoxColumn();
            RFC = new DataGridViewTextBoxColumn();
            Domicilio = new DataGridViewTextBoxColumn();
            Telefono = new DataGridViewTextBoxColumn();
            Ciudad = new DataGridViewTextBoxColumn();
            CP = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            flowLayoutPanel1.Controls.Add(button6);
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
            // button6
            // 
            button6.Image =Forms.Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(23, 3);
            button6.Name = "button6";
            button6.Size = new Size(124, 35);
            button6.TabIndex = 6;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button1
            // 
            button1.Image =Forms.Properties.Resources.plusM;
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
            button3.Image =Forms.Properties.Resources.editar;
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
            button2.Image =Forms.Properties.Resources.plusM;
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Activo, Id, NombreComercial, RazonSocial, RFC, Domicilio, Telefono, Ciudad, CP });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 47);
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
            // Id
            // 
            Id.HeaderText = "Id";
            Id.MinimumWidth = 6;
            Id.Name = "Id";
            Id.Resizable = DataGridViewTriState.True;
            Id.SortMode = DataGridViewColumnSortMode.NotSortable;
            Id.Width = 50;
            // 
            // NombreComercial
            // 
            NombreComercial.HeaderText = "Nombre comercial";
            NombreComercial.MinimumWidth = 6;
            NombreComercial.Name = "NombreComercial";
            NombreComercial.Resizable = DataGridViewTriState.True;
            NombreComercial.SortMode = DataGridViewColumnSortMode.NotSortable;
            NombreComercial.Width = 250;
            // 
            // RazonSocial
            // 
            RazonSocial.HeaderText = "Razón social";
            RazonSocial.MinimumWidth = 6;
            RazonSocial.Name = "RazonSocial";
            RazonSocial.Resizable = DataGridViewTriState.True;
            RazonSocial.SortMode = DataGridViewColumnSortMode.NotSortable;
            RazonSocial.Width = 250;
            // 
            // RFC
            // 
            RFC.HeaderText = "RFC";
            RFC.MinimumWidth = 6;
            RFC.Name = "RFC";
            RFC.Resizable = DataGridViewTriState.True;
            RFC.SortMode = DataGridViewColumnSortMode.NotSortable;
            RFC.Width = 125;
            // 
            // Domicilio
            // 
            Domicilio.HeaderText = "Domicilio comercial";
            Domicilio.MinimumWidth = 6;
            Domicilio.Name = "Domicilio";
            Domicilio.Width = 125;
            // 
            // Telefono
            // 
            Telefono.HeaderText = "Teléfono";
            Telefono.MinimumWidth = 6;
            Telefono.Name = "Telefono";
            Telefono.Width = 125;
            // 
            // Ciudad
            // 
            Ciudad.HeaderText = "Ciudad";
            Ciudad.MinimumWidth = 6;
            Ciudad.Name = "Ciudad";
            Ciudad.Width = 125;
            // 
            // CP
            // 
            CP.HeaderText = "CP";
            CP.MinimumWidth = 6;
            CP.Name = "CP";
            CP.Width = 80;
            // 
            // FrmClientes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmClientes";
            Text = "FrmPlantillaForm";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
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
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn NombreComercial;
        private DataGridViewTextBoxColumn RazonSocial;
        private DataGridViewTextBoxColumn RFC;
        private DataGridViewTextBoxColumn Domicilio;
        private DataGridViewTextBoxColumn Telefono;
        private DataGridViewTextBoxColumn Ciudad;
        private DataGridViewTextBoxColumn CP;
        private Button button6;
    }
}