using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmControlPatio
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
            dataGridView1 = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Fecha = new DataGridViewTextBoxColumn();
            Motivo = new DataGridViewTextBoxColumn();
            Tipo = new DataGridViewTextBoxColumn();
            Origen = new DataGridViewTextBoxColumn();
            TipoC = new DataGridViewTextBoxColumn();
            NCaja = new DataGridViewTextBoxColumn();
            Instrucci = new DataGridViewTextBoxColumn();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            dataGridView2 = new DataGridView();
            IDT = new DataGridViewTextBoxColumn();
            FechaT = new DataGridViewTextBoxColumn();
            ChoferT = new DataGridViewTextBoxColumn();
            NumUndT = new DataGridViewTextBoxColumn();
            InstruccionT = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            reloadBtn = new Button();
            label1 = new Label();
            splitContainer3 = new SplitContainer();
            dataGridView3 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            CortinaA = new DataGridViewTextBoxColumn();
            Inicio = new DataGridViewTextBoxColumn();
            FinPOp = new DataGridViewTextBoxColumn();
            Ed = new DataGridViewLinkColumn();
            ca = new DataGridViewLinkColumn();
            dataGridView4 = new DataGridView();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
            Editar = new DataGridViewLinkColumn();
            Cac = new DataGridViewLinkColumn();
            panel2 = new Panel();
            label2 = new Label();
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
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ID, Fecha, Motivo, Tipo, Origen, TipoC, NCaja, Instrucci });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(840, 317);
            dataGridView1.TabIndex = 1;
            // 
            // ID
            // 
            ID.HeaderText = "ID";
            ID.MinimumWidth = 6;
            ID.Name = "ID";
            ID.Width = 50;
            // 
            // Fecha
            // 
            Fecha.HeaderText = "FechaRegistro";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 125;
            // 
            // Motivo
            // 
            Motivo.HeaderText = "Motivo";
            Motivo.MinimumWidth = 6;
            Motivo.Name = "Motivo";
            Motivo.Width = 125;
            // 
            // Tipo
            // 
            Tipo.HeaderText = "Tipo";
            Tipo.MinimumWidth = 6;
            Tipo.Name = "Tipo";
            Tipo.Width = 125;
            // 
            // Origen
            // 
            Origen.HeaderText = "Origen";
            Origen.MinimumWidth = 6;
            Origen.Name = "Origen";
            Origen.Width = 125;
            // 
            // TipoC
            // 
            TipoC.HeaderText = "Tipo caja";
            TipoC.MinimumWidth = 6;
            TipoC.Name = "TipoC";
            TipoC.Width = 125;
            // 
            // NCaja
            // 
            NCaja.HeaderText = "No. de Caja";
            NCaja.MinimumWidth = 6;
            NCaja.Name = "NCaja";
            NCaja.Width = 125;
            // 
            // Instrucci
            // 
            Instrucci.HeaderText = "Instrucción";
            Instrucci.MinimumWidth = 6;
            Instrucci.Name = "Instrucci";
            Instrucci.Width = 250;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer3);
            splitContainer1.Panel2.Controls.Add(panel2);
            splitContainer1.Size = new Size(1624, 878);
            splitContainer1.SplitterDistance = 363;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 46);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dataGridView2);
            splitContainer2.Size = new Size(1624, 317);
            splitContainer2.SplitterDistance = 840;
            splitContainer2.TabIndex = 0;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { IDT, FechaT, ChoferT, NumUndT, InstruccionT });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(780, 317);
            dataGridView2.TabIndex = 2;
            // 
            // IDT
            // 
            IDT.HeaderText = "ID";
            IDT.MinimumWidth = 6;
            IDT.Name = "IDT";
            IDT.Width = 50;
            // 
            // FechaT
            // 
            FechaT.HeaderText = "Fecha de registro";
            FechaT.MinimumWidth = 6;
            FechaT.Name = "FechaT";
            FechaT.Width = 125;
            // 
            // ChoferT
            // 
            ChoferT.HeaderText = "Chofer";
            ChoferT.MinimumWidth = 6;
            ChoferT.Name = "ChoferT";
            ChoferT.Width = 125;
            // 
            // NumUndT
            // 
            NumUndT.HeaderText = "No. de unidad";
            NumUndT.MinimumWidth = 6;
            NumUndT.Name = "NumUndT";
            NumUndT.Width = 125;
            // 
            // InstruccionT
            // 
            InstruccionT.HeaderText = "Instrucción";
            InstruccionT.MinimumWidth = 6;
            InstruccionT.Name = "InstruccionT";
            InstruccionT.Width = 250;
            // 
            // panel1
            // 
            panel1.Controls.Add(reloadBtn);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1624, 46);
            panel1.TabIndex = 10;
            // 
            // reloadBtn
            // 
            reloadBtn.Image = Properties.Resources.update;
            reloadBtn.ImageAlign = ContentAlignment.MiddleLeft;
            reloadBtn.Location = new Point(247, 5);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(123, 35);
            reloadBtn.TabIndex = 6;
            reloadBtn.Text = "Actualizar";
            reloadBtn.UseVisualStyleBackColor = true;
            reloadBtn.Click += reloadBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(220, 28);
            label1.TabIndex = 0;
            label1.Text = "Operaciones Pendientes";
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 46);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(dataGridView3);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(dataGridView4);
            splitContainer3.Size = new Size(1624, 465);
            splitContainer3.SplitterDistance = 839;
            splitContainer3.TabIndex = 0;
            // 
            // dataGridView3
            // 
            dataGridViewCellStyle3.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, CortinaA, Inicio, FinPOp, Ed, ca });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(0, 0);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(839, 465);
            dataGridView3.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "ID";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "FechaRegistro";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Motivo";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Tipo";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Origen";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Tipo caja";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "No. de Caja";
            dataGridViewTextBoxColumn10.MinimumWidth = 6;
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.Width = 125;
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "Status";
            dataGridViewTextBoxColumn11.MinimumWidth = 6;
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.Width = 125;
            // 
            // CortinaA
            // 
            CortinaA.HeaderText = "Cortina";
            CortinaA.MinimumWidth = 6;
            CortinaA.Name = "CortinaA";
            CortinaA.Width = 125;
            // 
            // Inicio
            // 
            Inicio.HeaderText = "Inicio operación";
            Inicio.MinimumWidth = 6;
            Inicio.Name = "Inicio";
            Inicio.Width = 125;
            // 
            // FinPOp
            // 
            FinPOp.HeaderText = "Fin operación";
            FinPOp.MinimumWidth = 6;
            FinPOp.Name = "FinPOp";
            FinPOp.Width = 125;
            // 
            // Ed
            // 
            Ed.HeaderText = "Editar";
            Ed.MinimumWidth = 6;
            Ed.Name = "Ed";
            Ed.Resizable = DataGridViewTriState.True;
            Ed.SortMode = DataGridViewColumnSortMode.Automatic;
            Ed.Width = 80;
            // 
            // ca
            // 
            ca.HeaderText = "Cancelar";
            ca.MinimumWidth = 6;
            ca.Name = "ca";
            ca.Width = 80;
            // 
            // dataGridView4
            // 
            dataGridViewCellStyle4.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView4.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView4.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn12, dataGridViewTextBoxColumn13, Editar, Cac });
            dataGridView4.Dock = DockStyle.Fill;
            dataGridView4.Location = new Point(0, 0);
            dataGridView4.Name = "dataGridView4";
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.Size = new Size(781, 465);
            dataGridView4.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "ID";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Width = 50;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Fecha de registro";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Chofer";
            dataGridViewTextBoxColumn9.MinimumWidth = 6;
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.Width = 125;
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewTextBoxColumn12.HeaderText = "No. de unidad";
            dataGridViewTextBoxColumn12.MinimumWidth = 6;
            dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            dataGridViewTextBoxColumn12.Width = 125;
            // 
            // dataGridViewTextBoxColumn13
            // 
            dataGridViewTextBoxColumn13.HeaderText = "Status";
            dataGridViewTextBoxColumn13.MinimumWidth = 6;
            dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            dataGridViewTextBoxColumn13.Width = 125;
            // 
            // Editar
            // 
            Editar.HeaderText = "Editar";
            Editar.MinimumWidth = 6;
            Editar.Name = "Editar";
            Editar.Width = 80;
            // 
            // Cac
            // 
            Cac.HeaderText = "Cancelar";
            Cac.MinimumWidth = 6;
            Cac.Name = "Cac";
            Cac.Width = 80;
            // 
            // panel2
            // 
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1624, 46);
            panel2.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(126, 28);
            label2.TabIndex = 0;
            label2.Text = "Operaciones ";
            // 
            // FrmControlPatio
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1624, 878);
            Controls.Add(splitContainer1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmControlPatio";
            Text = "Control de Patio";
            Load += FrmControlPatio_Load;
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
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer3;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Label label2;
        private DataGridView dataGridView2;
        private DataGridView dataGridView3;
        private DataGridView dataGridView4;
        private Button reloadBtn;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Motivo;
        private DataGridViewTextBoxColumn Tipo;
        private DataGridViewTextBoxColumn Origen;
        private DataGridViewTextBoxColumn TipoC;
        private DataGridViewTextBoxColumn NCaja;
        private DataGridViewTextBoxColumn Instrucci;
        private DataGridViewTextBoxColumn IDT;
        private DataGridViewTextBoxColumn FechaT;
        private DataGridViewTextBoxColumn ChoferT;
        private DataGridViewTextBoxColumn NumUndT;
        private DataGridViewTextBoxColumn InstruccionT;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn CortinaA;
        private DataGridViewTextBoxColumn Inicio;
        private DataGridViewTextBoxColumn FinPOp;
        private DataGridViewLinkColumn Ed;
        private DataGridViewLinkColumn ca;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewLinkColumn Editar;
        private DataGridViewLinkColumn Cac;
    }
}