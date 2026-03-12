using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmCatalogos
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
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            tabPage1 = new TabPage();
            tabControl1 = new TabControl();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            textBox5 = new TextBox();
            button1 = new Button();
            label16 = new Label();
            dataGridView2 = new DataGridView();
            Estatus = new DataGridViewTextBoxColumn();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            textBox1 = new TextBox();
            button4 = new Button();
            label1 = new Label();
            button5 = new Button();
            button6 = new Button();
            panel3 = new Panel();
            groupBox3 = new GroupBox();
            textBox2 = new TextBox();
            button7 = new Button();
            label2 = new Label();
            panel4 = new Panel();
            groupBox4 = new GroupBox();
            textBox3 = new TextBox();
            button10 = new Button();
            label3 = new Label();
            button11 = new Button();
            button12 = new Button();
            dataGridView1 = new DataGridView();
            dataGridView3 = new DataGridView();
            dataGridView4 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            button13 = new Button();
            button2 = new Button();
            button3 = new Button();
            button14 = new Button();
            button8 = new Button();
            button9 = new Button();
            button15 = new Button();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel3.SuspendLayout();
            groupBox3.SuspendLayout();
            panel4.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            SuspendLayout();
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dataGridView2);
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1616, 845);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Status";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1624, 878);
            tabControl1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button14);
            panel2.Controls.Add(groupBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1610, 222);
            panel2.TabIndex = 66;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox5);
            groupBox2.Controls.Add(button1);
            groupBox2.Controls.Add(label16);
            groupBox2.Location = new Point(11, 16);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(742, 150);
            groupBox2.TabIndex = 65;
            groupBox2.TabStop = false;
            groupBox2.Text = "Status";
            // 
            // textBox5
            // 
            textBox5.BorderStyle = BorderStyle.FixedSingle;
            textBox5.Font = new Font("Segoe UI", 9.75F);
            textBox5.Location = new Point(134, 26);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(278, 29);
            textBox5.TabIndex = 59;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.save;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(14, 86);
            button1.Name = "button1";
            button1.Size = new Size(226, 35);
            button1.TabIndex = 57;
            button1.Text = "Guardar Status";
            button1.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(14, 35);
            label16.Name = "label16";
            label16.Size = new Size(52, 20);
            label16.TabIndex = 60;
            label16.Text = "Status:";
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle14.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle14;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Estatus });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 225);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(1610, 617);
            dataGridView2.TabIndex = 67;
            // 
            // Estatus
            // 
            Estatus.HeaderText = "Estatus";
            Estatus.MinimumWidth = 6;
            Estatus.Name = "Estatus";
            Estatus.Width = 350;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dataGridView1);
            tabPage2.Controls.Add(panel1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1616, 845);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Categorías";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dataGridView3);
            tabPage3.Controls.Add(panel3);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1616, 845);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Unidades de medida";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dataGridView4);
            tabPage4.Controls.Add(panel4);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1616, 845);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Monedas";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(button5);
            panel1.Controls.Add(button6);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1610, 222);
            panel1.TabIndex = 67;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(11, 16);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(742, 150);
            groupBox1.TabIndex = 65;
            groupBox1.TabStop = false;
            groupBox1.Text = "Categorías";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9.75F);
            textBox1.Location = new Point(134, 26);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(278, 29);
            textBox1.TabIndex = 59;
            // 
            // button4
            // 
            button4.Image = Properties.Resources.save;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(14, 86);
            button4.Name = "button4";
            button4.Size = new Size(226, 35);
            button4.TabIndex = 57;
            button4.Text = "Guardar Categoría";
            button4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 35);
            label1.Name = "label1";
            label1.Size = new Size(77, 20);
            label1.TabIndex = 60;
            label1.Text = "Categoría:";
            // 
            // button5
            // 
            button5.Image = Properties.Resources.editar;
            button5.ImageAlign = ContentAlignment.MiddleLeft;
            button5.Location = new Point(13, 172);
            button5.Name = "button5";
            button5.Size = new Size(108, 35);
            button5.TabIndex = 56;
            button5.Text = "Editar";
            button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.cancel;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(127, 172);
            button6.Name = "button6";
            button6.Size = new Size(113, 35);
            button6.TabIndex = 58;
            button6.Text = "Eliminar";
            button6.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(button8);
            panel3.Controls.Add(button9);
            panel3.Controls.Add(button15);
            panel3.Controls.Add(groupBox3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1610, 222);
            panel3.TabIndex = 67;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox2);
            groupBox3.Controls.Add(button7);
            groupBox3.Controls.Add(label2);
            groupBox3.Location = new Point(11, 16);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(742, 150);
            groupBox3.TabIndex = 65;
            groupBox3.TabStop = false;
            groupBox3.Text = "Unidades de medida";
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Font = new Font("Segoe UI", 9.75F);
            textBox2.Location = new Point(170, 26);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(278, 29);
            textBox2.TabIndex = 59;
            // 
            // button7
            // 
            button7.Image = Properties.Resources.save;
            button7.ImageAlign = ContentAlignment.MiddleLeft;
            button7.Location = new Point(14, 86);
            button7.Name = "button7";
            button7.Size = new Size(226, 35);
            button7.TabIndex = 57;
            button7.Text = "Guardar Unidad";
            button7.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 35);
            label2.Name = "label2";
            label2.Size = new Size(150, 20);
            label2.TabIndex = 60;
            label2.Text = "Unidades de medida:";
            // 
            // panel4
            // 
            panel4.Controls.Add(button13);
            panel4.Controls.Add(groupBox4);
            panel4.Controls.Add(button11);
            panel4.Controls.Add(button12);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(1610, 222);
            panel4.TabIndex = 67;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox3);
            groupBox4.Controls.Add(button10);
            groupBox4.Controls.Add(label3);
            groupBox4.Location = new Point(11, 16);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(742, 150);
            groupBox4.TabIndex = 65;
            groupBox4.TabStop = false;
            groupBox4.Text = "Monedas";
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Segoe UI", 9.75F);
            textBox3.Location = new Point(134, 26);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(278, 29);
            textBox3.TabIndex = 59;
            // 
            // button10
            // 
            button10.Image = Properties.Resources.save;
            button10.ImageAlign = ContentAlignment.MiddleLeft;
            button10.Location = new Point(14, 86);
            button10.Name = "button10";
            button10.Size = new Size(226, 35);
            button10.TabIndex = 57;
            button10.Text = "Guardar Moneda";
            button10.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 35);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 60;
            label3.Text = "Moneda:";
            // 
            // button11
            // 
            button11.Image = Properties.Resources.editar;
            button11.ImageAlign = ContentAlignment.MiddleLeft;
            button11.Location = new Point(131, 172);
            button11.Name = "button11";
            button11.Size = new Size(108, 35);
            button11.TabIndex = 56;
            button11.Text = "Editar";
            button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Image = Properties.Resources.cancel;
            button12.ImageAlign = ContentAlignment.MiddleLeft;
            button12.Location = new Point(245, 172);
            button12.Name = "button12";
            button12.Size = new Size(113, 35);
            button12.TabIndex = 58;
            button12.Text = "Eliminar";
            button12.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle15.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1 });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 225);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1610, 617);
            dataGridView1.TabIndex = 68;
            // 
            // dataGridView3
            // 
            dataGridViewCellStyle13.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn2 });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(3, 225);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(1610, 617);
            dataGridView3.TabIndex = 68;
            // 
            // dataGridView4
            // 
            dataGridViewCellStyle16.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            dataGridView4.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn3 });
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(3, 225);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(1610, 617);
            dataGridView4.TabIndex = 68;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Categoría";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 350;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Unidad de medida";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 350;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Moneda";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 350;
            // 
            // button13
            // 
            button13.Image = Properties.Resources.update;
            button13.ImageAlign = ContentAlignment.MiddleLeft;
            button13.Location = new Point(17, 172);
            button13.Name = "button13";
            button13.Size = new Size(108, 35);
            button13.TabIndex = 66;
            button13.Text = "Actualizar";
            button13.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.update;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(11, 172);
            button2.Name = "button2";
            button2.Size = new Size(108, 35);
            button2.TabIndex = 69;
            button2.Text = "Actualizar";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.editar;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(125, 172);
            button3.Name = "button3";
            button3.Size = new Size(108, 35);
            button3.TabIndex = 67;
            button3.Text = "Editar";
            button3.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            button14.Image = Properties.Resources.cancel;
            button14.ImageAlign = ContentAlignment.MiddleLeft;
            button14.Location = new Point(239, 172);
            button14.Name = "button14";
            button14.Size = new Size(113, 35);
            button14.TabIndex = 68;
            button14.Text = "Eliminar";
            button14.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Image = Properties.Resources.update;
            button8.ImageAlign = ContentAlignment.MiddleLeft;
            button8.Location = new Point(13, 172);
            button8.Name = "button8";
            button8.Size = new Size(108, 35);
            button8.TabIndex = 69;
            button8.Text = "Actualizar";
            button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Image = Properties.Resources.editar;
            button9.ImageAlign = ContentAlignment.MiddleLeft;
            button9.Location = new Point(127, 172);
            button9.Name = "button9";
            button9.Size = new Size(108, 35);
            button9.TabIndex = 67;
            button9.Text = "Editar";
            button9.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            button15.Image = Properties.Resources.cancel;
            button15.ImageAlign = ContentAlignment.MiddleLeft;
            button15.Location = new Point(241, 172);
            button15.Name = "button15";
            button15.Size = new Size(113, 35);
            button15.TabIndex = 68;
            button15.Text = "Eliminar";
            button15.UseVisualStyleBackColor = true;
            // 
            // FrmCatalogos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1624, 878);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmCatalogos";
            Text = "Catálogos";
            tabPage1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel4.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private TabPage tabPage1;
        private TabControl tabControl1;
        private DataGridView dataGridView2;
        private Panel panel2;
        private GroupBox groupBox2;
        private TextBox textBox5;
        private Button button1;
        private Label label16;
        private DataGridViewTextBoxColumn Estatus;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private DataGridView dataGridView1;
        private Panel panel1;
        private GroupBox groupBox1;
        private TextBox textBox1;
        private Button button4;
        private Label label1;
        private Button button5;
        private Button button6;
        private DataGridView dataGridView3;
        private Panel panel3;
        private GroupBox groupBox3;
        private TextBox textBox2;
        private Button button7;
        private Label label2;
        private DataGridView dataGridView4;
        private Panel panel4;
        private GroupBox groupBox4;
        private TextBox textBox3;
        private Button button10;
        private Label label3;
        private Button button11;
        private Button button12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private Button button13;
        private Button button2;
        private Button button3;
        private Button button14;
        private Button button8;
        private Button button9;
        private Button button15;
    }
}