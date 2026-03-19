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
            reloadBtn = new Button();
            AddBtn = new Button();
            EditBtn = new Button();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            button5 = new Button();
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
            flowLayoutPanel1.Controls.Add(reloadBtn);
            flowLayoutPanel1.Controls.Add(AddBtn);
            flowLayoutPanel1.Controls.Add(EditBtn);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Controls.Add(button5);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(20, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(473, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // reloadBtn
            // 
            reloadBtn.Image = Properties.Resources.update;
            reloadBtn.ImageAlign = ContentAlignment.MiddleLeft;
            reloadBtn.Location = new Point(23, 3);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(124, 35);
            reloadBtn.TabIndex = 5;
            reloadBtn.Text = "Actualizar";
            reloadBtn.UseVisualStyleBackColor = true;
            reloadBtn.Click += reloadBtn_Click;
            // 
            // AddBtn
            // 
            AddBtn.Image = Properties.Resources.plusM;
            AddBtn.ImageAlign = ContentAlignment.MiddleLeft;
            AddBtn.Location = new Point(153, 3);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(107, 35);
            AddBtn.TabIndex = 0;
            AddBtn.Text = "Nuevo";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += button1_Click;
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
        private Button AddBtn;
        private Button EditBtn;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button button2;
        private DataGridView dataGridView1;
        private Button button4;
        private Button reloadBtn;
        private Button button5;
        private Panel gridContainer;
    }
}