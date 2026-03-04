using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmUsuarios
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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnActualizar = new Button();
            btnAdd = new Button();
            btnEdit = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Activo = new DataGridViewCheckBoxColumn();
            Usuarios = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            Rol = new DataGridViewTextBoxColumn();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            dataGridView2 = new DataGridView();
            Rol2 = new DataGridViewTextBoxColumn();
            Permiso = new DataGridViewTextBoxColumn();
            NombreX = new DataGridViewTextBoxColumn();
            Activos = new DataGridViewCheckBoxColumn();
            dataGridView3 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            panel2 = new Panel();
            button7 = new Button();
            button4 = new Button();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1404, 32);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnActualizar);
            flowLayoutPanel1.Controls.Add(btnAdd);
            flowLayoutPanel1.Controls.Add(btnEdit);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(18, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(414, 32);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnActualizar
            // 
            btnActualizar.Image = Properties.Resources.update;
            btnActualizar.ImageAlign = ContentAlignment.MiddleLeft;
            btnActualizar.Location = new Point(21, 2);
            btnActualizar.Margin = new Padding(3, 2, 3, 2);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(108, 26);
            btnActualizar.TabIndex = 5;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // btnAdd
            // 
            btnAdd.Image = Properties.Resources.plusM;
            btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdd.Location = new Point(135, 2);
            btnAdd.Margin = new Padding(3, 2, 3, 2);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 26);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Nuevo";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += button1_Click;
            // 
            // btnEdit
            // 
            btnEdit.Image = Properties.Resources.editar;
            btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
            btnEdit.Location = new Point(235, 2);
            btnEdit.Margin = new Padding(3, 2, 3, 2);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 26);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Editar";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += button3_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(button2);
            flowLayoutPanel2.Dock = DockStyle.Left;
            flowLayoutPanel2.Location = new Point(21, 32);
            flowLayoutPanel2.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(349, 0);
            flowLayoutPanel2.TabIndex = 1;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.plusM;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(3, 2);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(94, 26);
            button2.TabIndex = 0;
            button2.Text = "Nuevo";
            button2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle4.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Activo, Usuarios, Nombre, Rol });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(813, 254);
            dataGridView1.TabIndex = 1;
            // 
            // Activo
            // 
            Activo.HeaderText = "Activo";
            Activo.MinimumWidth = 6;
            Activo.Name = "Activo";
            Activo.Width = 125;
            // 
            // Usuarios
            // 
            Usuarios.HeaderText = "ID";
            Usuarios.MinimumWidth = 6;
            Usuarios.Name = "Usuarios";
            Usuarios.Width = 125;
            // 
            // Nombre
            // 
            Nombre.HeaderText = "Nombre";
            Nombre.MinimumWidth = 6;
            Nombre.Name = "Nombre";
            Nombre.Width = 400;
            // 
            // Rol
            // 
            Rol.HeaderText = "Rol";
            Rol.MinimumWidth = 6;
            Rol.Name = "Rol";
            Rol.Width = 150;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 32);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView3);
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Size = new Size(1404, 504);
            splitContainer1.SplitterDistance = 813;
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
            splitContainer2.Size = new Size(813, 504);
            splitContainer2.SplitterDistance = 254;
            splitContainer2.SplitterWidth = 3;
            splitContainer2.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle5.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Rol2, Permiso, NombreX, Activos });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Margin = new Padding(3, 2, 3, 2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(813, 247);
            dataGridView2.TabIndex = 2;
            // 
            // Rol2
            // 
            Rol2.HeaderText = "Rol";
            Rol2.MinimumWidth = 6;
            Rol2.Name = "Rol2";
            Rol2.Width = 125;
            // 
            // Permiso
            // 
            Permiso.HeaderText = "Permiso";
            Permiso.MinimumWidth = 6;
            Permiso.Name = "Permiso";
            Permiso.Width = 125;
            // 
            // NombreX
            // 
            NombreX.HeaderText = "Nombre";
            NombreX.MinimumWidth = 6;
            NombreX.Name = "NombreX";
            NombreX.Width = 250;
            // 
            // Activos
            // 
            Activos.HeaderText = "Activo";
            Activos.MinimumWidth = 6;
            Activos.Name = "Activos";
            Activos.Width = 125;
            // 
            // dataGridView3
            // 
            dataGridViewCellStyle6.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewCheckBoxColumn1 });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(54, 0);
            dataGridView3.Margin = new Padding(3, 2, 3, 2);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(533, 504);
            dataGridView3.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Rol";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Nombre";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCheckBoxColumn1.HeaderText = "Activo";
            dataGridViewCheckBoxColumn1.MinimumWidth = 6;
            dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            dataGridViewCheckBoxColumn1.Width = 125;
            // 
            // panel2
            // 
            panel2.Controls.Add(button7);
            panel2.Controls.Add(button4);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(54, 504);
            panel2.TabIndex = 0;
            // 
            // button7
            // 
            button7.BackColor = Color.LightSalmon;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Bookman Old Style", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button7.ImageAlign = ContentAlignment.MiddleLeft;
            button7.Location = new Point(2, 45);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(49, 36);
            button7.TabIndex = 4;
            button7.Text = ">";
            button7.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.LightGreen;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Bookman Old Style", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(2, 4);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(49, 36);
            button4.TabIndex = 3;
            button4.Text = "<";
            button4.UseVisualStyleBackColor = false;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1404, 536);
            Controls.Add(splitContainer1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmUsuarios";
            Text = "Usuarios";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnAdd;
        private Button btnEdit;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView dataGridView1;
        private Button btnActualizar;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private DataGridView dataGridView2;
        private DataGridViewCheckBoxColumn Activo;
        private DataGridViewTextBoxColumn Usuarios;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Rol;
        private DataGridView dataGridView3;
        private Panel panel2;
        private Button button4;
        private Button button7;
        private DataGridViewTextBoxColumn Rol2;
        private DataGridViewTextBoxColumn Permiso;
        private DataGridViewTextBoxColumn NombreX;
        private DataGridViewCheckBoxColumn Activos;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    }
}