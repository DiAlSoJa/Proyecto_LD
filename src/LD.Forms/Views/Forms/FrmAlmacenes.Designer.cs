using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmAlmacenes
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
            btn_actualizar = new Button();
            NewBtn = new Button();
            EditBtn = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
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
            flowLayoutPanel1.Controls.Add(btn_actualizar);
            flowLayoutPanel1.Controls.Add(NewBtn);
            flowLayoutPanel1.Controls.Add(EditBtn);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(399, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btn_actualizar
            // 
            btn_actualizar.Image = Properties.Resources.update;
            btn_actualizar.ImageAlign = ContentAlignment.MiddleLeft;
            btn_actualizar.Location = new Point(23, 3);
            btn_actualizar.Name = "btn_actualizar";
            btn_actualizar.Size = new Size(124, 35);
            btn_actualizar.TabIndex = 6;
            btn_actualizar.Text = "Actualizar";
            btn_actualizar.UseVisualStyleBackColor = true;
            btn_actualizar.Click += btn_actualizar_Click;
            // 
            // NewBtn
            // 
            NewBtn.Image = Properties.Resources.plusM;
            NewBtn.ImageAlign = ContentAlignment.MiddleLeft;
            NewBtn.Location = new Point(153, 3);
            NewBtn.Name = "NewBtn";
            NewBtn.Size = new Size(107, 35);
            NewBtn.TabIndex = 0;
            NewBtn.Text = "Nuevo";
            NewBtn.UseVisualStyleBackColor = true;
            NewBtn.Click += button1_Click;
            // 
            // EditBtn
            // 
            EditBtn.Image = Properties.Resources.editar;
            EditBtn.ImageAlign = ContentAlignment.MiddleLeft;
            EditBtn.Location = new Point(266, 3);
            EditBtn.Name = "EditBtn";
            EditBtn.Size = new Size(107, 35);
            EditBtn.TabIndex = 2;
            EditBtn.Text = "Editar";
            EditBtn.UseVisualStyleBackColor = true;
            EditBtn.Click += EditBtn_Click;
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
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1496, 674);
            dataGridView1.TabIndex = 1;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
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
            // FrmAlmacenes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1496, 721);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmAlmacenes";
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
        private Button NewBtn;
        private Button EditBtn;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn Activo;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn NombreComercial;
        private DataGridViewTextBoxColumn Domicilio;
        private DataGridViewTextBoxColumn Telefono;
        private DataGridViewTextBoxColumn Ciudad;
        private DataGridViewTextBoxColumn CP;
        private Button btn_actualizar;
        private Panel gridContainer;
    }
}