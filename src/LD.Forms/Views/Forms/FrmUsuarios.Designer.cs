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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel1 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnActualizar = new Button();
            btnAdd = new Button();
            btnEdit = new Button();
            btnRoles = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            usersGrid = new DataGridView();
            permissionGrid = new DataGridView();
            panelContainer = new Panel();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)usersGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)permissionGrid).BeginInit();
            panelContainer.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1605, 43);
            panel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(btnActualizar);
            flowLayoutPanel1.Controls.Add(btnAdd);
            flowLayoutPanel1.Controls.Add(btnEdit);
            flowLayoutPanel1.Controls.Add(btnRoles);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(21, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(598, 43);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnActualizar
            // 
            btnActualizar.Image = Properties.Resources.update;
            btnActualizar.ImageAlign = ContentAlignment.MiddleLeft;
            btnActualizar.Location = new Point(24, 3);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(123, 35);
            btnActualizar.TabIndex = 5;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // btnAdd
            // 
            btnAdd.Image = Properties.Resources.plusM;
            btnAdd.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdd.Location = new Point(153, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(107, 35);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Nuevo";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += button1_Click;
            // 
            // btnEdit
            // 
            btnEdit.Image = Properties.Resources.editar;
            btnEdit.ImageAlign = ContentAlignment.MiddleLeft;
            btnEdit.Location = new Point(266, 3);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(107, 35);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Editar";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += button3_Click;
            // 
            // btnRoles
            // 
            btnRoles.Image = Properties.Resources.plusM;
            btnRoles.ImageAlign = ContentAlignment.MiddleLeft;
            btnRoles.Location = new Point(379, 3);
            btnRoles.Name = "btnRoles";
            btnRoles.Size = new Size(137, 35);
            btnRoles.TabIndex = 4;
            btnRoles.Text = "Roles";
            btnRoles.UseVisualStyleBackColor = true;
            btnRoles.Click += btnRoles_Click;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(button2);
            flowLayoutPanel2.Dock = DockStyle.Left;
            flowLayoutPanel2.Location = new Point(24, 44);
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
            // usersGrid
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            usersGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            usersGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            usersGrid.BackgroundColor = SystemColors.ButtonHighlight;
            usersGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usersGrid.Location = new Point(12, 7);
            usersGrid.Name = "usersGrid";
            usersGrid.RowHeadersWidth = 51;
            usersGrid.Size = new Size(1185, 628);
            usersGrid.TabIndex = 1;
            usersGrid.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // permissionGrid
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            permissionGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            permissionGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            permissionGrid.BackgroundColor = SystemColors.ButtonHighlight;
            permissionGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            permissionGrid.Location = new Point(1203, 7);
            permissionGrid.Name = "permissionGrid";
            permissionGrid.RowHeadersWidth = 51;
            permissionGrid.Size = new Size(379, 628);
            permissionGrid.TabIndex = 2;
            // 
            // panelContainer
            // 
            panelContainer.Controls.Add(permissionGrid);
            panelContainer.Controls.Add(usersGrid);
            panelContainer.Dock = DockStyle.Fill;
            panelContainer.Location = new Point(0, 43);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1605, 672);
            panelContainer.TabIndex = 3;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1605, 715);
            Controls.Add(panelContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUsuarios";
            Text = "Usuarios";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)usersGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)permissionGrid).EndInit();
            panelContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnAdd;
        private Button btnEdit;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView usersGrid;
        private Button btnActualizar;
        private DataGridView permissionGrid;
        private Panel panelContainer;
        private Button btnEditRol;
        private Button btnNewRol;
        private Button btnRoles;
    }
}