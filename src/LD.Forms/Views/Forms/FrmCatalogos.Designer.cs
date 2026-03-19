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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            tabPage1 = new TabPage();
            dtStatus = new DataGridView();
            panel2 = new Panel();
            btnNuevoS = new Button();
            btnActualizarS = new Button();
            btnEditarS = new Button();
            tabControl1 = new TabControl();
            tabPage2 = new TabPage();
            dtCategoria = new DataGridView();
            panel1 = new Panel();
            panel5 = new Panel();
            btnNuevoCa = new Button();
            btnActualizarC = new Button();
            btnEditarC = new Button();
            tabPage3 = new TabPage();
            dtUnidad = new DataGridView();
            panel3 = new Panel();
            btnNuevoU = new Button();
            button8 = new Button();
            button9 = new Button();
            tabPage4 = new TabPage();
            dtMoneda = new DataGridView();
            panel4 = new Panel();
            btnNuevoM = new Button();
            button13 = new Button();
            button11 = new Button();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtStatus).BeginInit();
            panel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtCategoria).BeginInit();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtUnidad).BeginInit();
            panel3.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtMoneda).BeginInit();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dtStatus);
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1616, 845);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Status";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dtStatus
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dtStatus.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dtStatus.BackgroundColor = SystemColors.ButtonHighlight;
            dtStatus.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtStatus.Dock = DockStyle.Fill;
            dtStatus.Location = new Point(3, 56);
            dtStatus.Name = "dtStatus";
            dtStatus.RowHeadersWidth = 51;
            dtStatus.Size = new Size(1610, 786);
            dtStatus.TabIndex = 8;
            dtStatus.SelectionChanged += dtStatus_SelectionChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnNuevoS);
            panel2.Controls.Add(btnActualizarS);
            panel2.Controls.Add(btnEditarS);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1610, 53);
            panel2.TabIndex = 66;
            // 
            // btnNuevoS
            // 
            btnNuevoS.Image = Properties.Resources.plusM;
            btnNuevoS.ImageAlign = ContentAlignment.MiddleLeft;
            btnNuevoS.Location = new Point(128, 12);
            btnNuevoS.Name = "btnNuevoS";
            btnNuevoS.Size = new Size(113, 35);
            btnNuevoS.TabIndex = 8;
            btnNuevoS.Text = "Nuevo";
            btnNuevoS.UseVisualStyleBackColor = true;
            btnNuevoS.Click += btnNuevoS_Click;
            // 
            // btnActualizarS
            // 
            btnActualizarS.Image = Properties.Resources.update;
            btnActualizarS.ImageAlign = ContentAlignment.MiddleLeft;
            btnActualizarS.Location = new Point(14, 12);
            btnActualizarS.Name = "btnActualizarS";
            btnActualizarS.Size = new Size(108, 35);
            btnActualizarS.TabIndex = 5;
            btnActualizarS.Text = "Actualizar";
            btnActualizarS.UseVisualStyleBackColor = true;
            btnActualizarS.Click += btnActualizarS_Click;
            // 
            // btnEditarS
            // 
            btnEditarS.Image = Properties.Resources.editar;
            btnEditarS.ImageAlign = ContentAlignment.MiddleLeft;
            btnEditarS.Location = new Point(247, 12);
            btnEditarS.Name = "btnEditarS";
            btnEditarS.Size = new Size(108, 35);
            btnEditarS.TabIndex = 6;
            btnEditarS.Text = "Editar";
            btnEditarS.UseVisualStyleBackColor = true;
            btnEditarS.Click += btnEditarS_Click;
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
            // tabPage2
            // 
            tabPage2.Controls.Add(dtCategoria);
            tabPage2.Controls.Add(panel1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1616, 845);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Categorías";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dtCategoria
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dtCategoria.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dtCategoria.BackgroundColor = SystemColors.ButtonHighlight;
            dtCategoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtCategoria.Dock = DockStyle.Fill;
            dtCategoria.Location = new Point(3, 61);
            dtCategoria.Name = "dtCategoria";
            dtCategoria.RowHeadersWidth = 51;
            dtCategoria.Size = new Size(1610, 781);
            dtCategoria.TabIndex = 15;
            dtCategoria.SelectionChanged += dtCategoria_SelectionChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel5);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1610, 58);
            panel1.TabIndex = 67;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnNuevoCa);
            panel5.Controls.Add(btnActualizarC);
            panel5.Controls.Add(btnEditarC);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1610, 53);
            panel5.TabIndex = 67;
            // 
            // btnNuevoCa
            // 
            btnNuevoCa.Image = Properties.Resources.plusM;
            btnNuevoCa.ImageAlign = ContentAlignment.MiddleLeft;
            btnNuevoCa.Location = new Point(125, 13);
            btnNuevoCa.Name = "btnNuevoCa";
            btnNuevoCa.Size = new Size(113, 35);
            btnNuevoCa.TabIndex = 15;
            btnNuevoCa.Text = "Nuevo";
            btnNuevoCa.UseVisualStyleBackColor = true;
            btnNuevoCa.Click += btnNuevoCa_Click;
            // 
            // btnActualizarC
            // 
            btnActualizarC.Image = Properties.Resources.update;
            btnActualizarC.ImageAlign = ContentAlignment.MiddleLeft;
            btnActualizarC.Location = new Point(11, 13);
            btnActualizarC.Name = "btnActualizarC";
            btnActualizarC.Size = new Size(108, 35);
            btnActualizarC.TabIndex = 12;
            btnActualizarC.Text = "Actualizar";
            btnActualizarC.UseVisualStyleBackColor = true;
            btnActualizarC.Click += btnActualizarC_Click;
            // 
            // btnEditarC
            // 
            btnEditarC.Image = Properties.Resources.editar;
            btnEditarC.ImageAlign = ContentAlignment.MiddleLeft;
            btnEditarC.Location = new Point(242, 13);
            btnEditarC.Name = "btnEditarC";
            btnEditarC.Size = new Size(108, 35);
            btnEditarC.TabIndex = 13;
            btnEditarC.Text = "Editar";
            btnEditarC.UseVisualStyleBackColor = true;
            btnEditarC.Click += btnEditarC_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dtUnidad);
            tabPage3.Controls.Add(panel3);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1616, 845);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Unidades de medida";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dtUnidad
            // 
            dataGridViewCellStyle3.BackColor = Color.FromArgb(253, 252, 213);
            dtUnidad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dtUnidad.BackgroundColor = SystemColors.ButtonHighlight;
            dtUnidad.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtUnidad.Dock = DockStyle.Fill;
            dtUnidad.Location = new Point(3, 56);
            dtUnidad.Name = "dtUnidad";
            dtUnidad.RowHeadersWidth = 51;
            dtUnidad.Size = new Size(1610, 786);
            dtUnidad.TabIndex = 22;
            dtUnidad.SelectionChanged += dtUnidad_SelectionChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnNuevoU);
            panel3.Controls.Add(button8);
            panel3.Controls.Add(button9);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1610, 53);
            panel3.TabIndex = 67;
            // 
            // btnNuevoU
            // 
            btnNuevoU.Image = Properties.Resources.plusM;
            btnNuevoU.ImageAlign = ContentAlignment.MiddleLeft;
            btnNuevoU.Location = new Point(129, 13);
            btnNuevoU.Name = "btnNuevoU";
            btnNuevoU.Size = new Size(113, 35);
            btnNuevoU.TabIndex = 22;
            btnNuevoU.Text = "Nuevo";
            btnNuevoU.UseVisualStyleBackColor = true;
            btnNuevoU.Click += btnNuevoU_Click;
            // 
            // button8
            // 
            button8.Image = Properties.Resources.update;
            button8.ImageAlign = ContentAlignment.MiddleLeft;
            button8.Location = new Point(15, 13);
            button8.Name = "button8";
            button8.Size = new Size(108, 35);
            button8.TabIndex = 19;
            button8.Text = "Actualizar";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Image = Properties.Resources.editar;
            button9.ImageAlign = ContentAlignment.MiddleLeft;
            button9.Location = new Point(248, 13);
            button9.Name = "button9";
            button9.Size = new Size(108, 35);
            button9.TabIndex = 20;
            button9.Text = "Editar";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dtMoneda);
            tabPage4.Controls.Add(panel4);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1616, 845);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Monedas";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dtMoneda
            // 
            dataGridViewCellStyle4.BackColor = Color.FromArgb(253, 252, 213);
            dtMoneda.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dtMoneda.BackgroundColor = SystemColors.ButtonHighlight;
            dtMoneda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtMoneda.Dock = DockStyle.Fill;
            dtMoneda.Location = new Point(3, 56);
            dtMoneda.Name = "dtMoneda";
            dtMoneda.RowHeadersWidth = 51;
            dtMoneda.Size = new Size(1610, 786);
            dtMoneda.TabIndex = 29;
            dtMoneda.SelectionChanged += dtMoneda_SelectionChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnNuevoM);
            panel4.Controls.Add(button13);
            panel4.Controls.Add(button11);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(1610, 53);
            panel4.TabIndex = 67;
            // 
            // btnNuevoM
            // 
            btnNuevoM.Image = Properties.Resources.plusM;
            btnNuevoM.ImageAlign = ContentAlignment.MiddleLeft;
            btnNuevoM.Location = new Point(119, 12);
            btnNuevoM.Name = "btnNuevoM";
            btnNuevoM.Size = new Size(113, 35);
            btnNuevoM.TabIndex = 29;
            btnNuevoM.Text = "Nuevo";
            btnNuevoM.UseVisualStyleBackColor = true;
            btnNuevoM.Click += btnNuevoM_Click;
            // 
            // button13
            // 
            button13.Image = Properties.Resources.update;
            button13.ImageAlign = ContentAlignment.MiddleLeft;
            button13.Location = new Point(5, 12);
            button13.Name = "button13";
            button13.Size = new Size(108, 35);
            button13.TabIndex = 26;
            button13.Text = "Actualizar";
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // button11
            // 
            button11.Image = Properties.Resources.editar;
            button11.ImageAlign = ContentAlignment.MiddleLeft;
            button11.Location = new Point(238, 12);
            button11.Name = "button11";
            button11.Size = new Size(108, 35);
            button11.TabIndex = 27;
            button11.Text = "Editar";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
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
            Load += FrmCatalogos_Load;
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtStatus).EndInit();
            panel2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtCategoria).EndInit();
            panel1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtUnidad).EndInit();
            panel3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtMoneda).EndInit();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TabPage tabPage1;
        private TabControl tabControl1;
        private DataGridView dtStatus;
        private Panel panel2;
        private TextBox textBox5;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private DataGridView dtCategoria;
        private Panel panel1;
        private DataGridView dtUnidad;
        private Panel panel3;
        private TextBox textBox2;
        private DataGridView dtMoneda;
        private Panel panel4;
        private TextBox textBox3;
        private Button button11;
        private Button button13;
        private Button button2;
        private Button button3;
        private Button button8;
        private Button button9;
        private Panel panel5;
        private Button btnActualizarC;
        private Button btnEditarC;
        private Button btnActualizarS;
        private Button btnEditarS;
        private Button btnNuevoS;
        private Button btnNuevoCa;
        private Button btnNuevoU;
        private Button btnNuevoM;
    }
}