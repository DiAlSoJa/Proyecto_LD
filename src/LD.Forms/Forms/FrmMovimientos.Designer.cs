using System;
using System.Windows.Forms;

namespace LD.Forms
{
    partial class FrmMovimientos
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
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            button6 = new Button();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            NumPArte = new DataGridViewTextBoxColumn();
            Descripcion = new DataGridViewTextBoxColumn();
            Fecha = new DataGridViewTextBoxColumn();
            Hora = new DataGridViewTextBoxColumn();
            Usuario = new DataGridViewTextBoxColumn();
            Documento = new DataGridViewTextBoxColumn();
            TipoDoc = new DataGridViewTextBoxColumn();
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
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(399, 47);
            flowLayoutPanel1.TabIndex = 0;
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Cliente, Proyecto, NumPArte, Descripcion, Fecha, Hora, Usuario, Documento, TipoDoc });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 47);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1496, 674);
            dataGridView1.TabIndex = 1;
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
            // Fecha
            // 
            Fecha.HeaderText = "Fecha";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 125;
            // 
            // Hora
            // 
            Hora.HeaderText = "Hora";
            Hora.MinimumWidth = 6;
            Hora.Name = "Hora";
            Hora.Width = 125;
            // 
            // Usuario
            // 
            Usuario.HeaderText = "Usuario";
            Usuario.MinimumWidth = 6;
            Usuario.Name = "Usuario";
            Usuario.Width = 125;
            // 
            // Documento
            // 
            Documento.HeaderText = "Documento";
            Documento.MinimumWidth = 6;
            Documento.Name = "Documento";
            Documento.Width = 125;
            // 
            // TipoDoc
            // 
            TipoDoc.HeaderText = "Tipo de documento";
            TipoDoc.MinimumWidth = 6;
            TipoDoc.Name = "TipoDoc";
            TipoDoc.Width = 125;
            // 
            // FrmMovimientos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmMovimientos";
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
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView dataGridView1;
        private Button button6;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn NumPArte;
        private DataGridViewTextBoxColumn Descripcion;
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Hora;
        private DataGridViewTextBoxColumn Usuario;
        private DataGridViewTextBoxColumn Documento;
        private DataGridViewTextBoxColumn TipoDoc;
    }
}