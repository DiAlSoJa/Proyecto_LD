using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmASN
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
            panel1 = new Panel();
            button4 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            reloadBtn = new Button();
            AddBtn = new Button();
            dataGridView1 = new DataGridView();
            gridContainer = new Panel();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            dataGridView2 = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            panel2 = new Panel();
            button1 = new Button();
            dataGridView3 = new DataGridView();
            Esta = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
            Estandar = new DataGridViewTextBoxColumn();
            Maxima = new DataGridViewTextBoxColumn();
            Envia = new DataGridViewTextBoxColumn();
            SD = new DataGridViewTextBoxColumn();
            Recibida = new DataGridViewTextBoxColumn();
            Statuss = new DataGridViewTextBoxColumn();
            Ubic = new DataGridViewTextBoxColumn();
            Lote = new DataGridViewTextBoxColumn();
            Cad = new DataGridViewTextBoxColumn();
            Rf = new DataGridViewTextBoxColumn();
            ASN = new DataGridViewTextBoxColumn();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            Factura = new DataGridViewTextBoxColumn();
            Guia = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Bultos = new DataGridViewTextBoxColumn();
            Origen = new DataGridViewTextBoxColumn();
            Fecha = new DataGridViewTextBoxColumn();
            Hora = new DataGridViewTextBoxColumn();
            ETA = new DataGridViewTextBoxColumn();
            Ordend = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            gridContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(button4);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1309, 35);
            panel1.TabIndex = 0;
            // 
            // button4
            // 
            button4.Image = Properties.Resources.entrada;
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(908, 5);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(187, 26);
            button4.TabIndex = 4;
            button4.Text = "Registrar arribo";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(reloadBtn);
            flowLayoutPanel1.Controls.Add(AddBtn);
            flowLayoutPanel1.Dock = DockStyle.Left;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(18, 0, 0, 0);
            flowLayoutPanel1.Size = new Size(859, 35);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // reloadBtn
            // 
            reloadBtn.Image = Properties.Resources.update;
            reloadBtn.ImageAlign = ContentAlignment.MiddleLeft;
            reloadBtn.Location = new Point(21, 2);
            reloadBtn.Margin = new Padding(3, 2, 3, 2);
            reloadBtn.Name = "reloadBtn";
            reloadBtn.Size = new Size(108, 26);
            reloadBtn.TabIndex = 5;
            reloadBtn.Text = "Actualizar";
            reloadBtn.UseVisualStyleBackColor = true;
            reloadBtn.Click += reloadBtn_Click;
            // 
            // AddBtn
            // 
            AddBtn.Image = Properties.Resources.plusM;
            AddBtn.ImageAlign = ContentAlignment.MiddleLeft;
            AddBtn.Location = new Point(135, 2);
            AddBtn.Margin = new Padding(3, 2, 3, 2);
            AddBtn.Name = "AddBtn";
            AddBtn.Size = new Size(122, 26);
            AddBtn.TabIndex = 0;
            AddBtn.Text = "Nuevo ASN";
            AddBtn.UseVisualStyleBackColor = true;
            AddBtn.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ASN, Cliente, Proyecto, Factura, Guia, Status, Bultos, Origen, Fecha, Hora, ETA, Ordend });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1309, 131);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // gridContainer
            // 
            gridContainer.Controls.Add(splitContainer1);
            gridContainer.Dock = DockStyle.Fill;
            gridContainer.Location = new Point(0, 35);
            gridContainer.Margin = new Padding(3, 2, 3, 2);
            gridContainer.Name = "gridContainer";
            gridContainer.Size = new Size(1309, 506);
            gridContainer.TabIndex = 5;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1309, 506);
            splitContainer1.SplitterDistance = 131;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(dataGridView2);
            splitContainer2.Panel1.Controls.Add(panel2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(dataGridView3);
            splitContainer2.Size = new Size(1309, 372);
            splitContainer2.SplitterDistance = 229;
            splitContainer2.TabIndex = 3;
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10 });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(0, 44);
            dataGridView2.Margin = new Padding(3, 2, 3, 2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(1309, 185);
            dataGridView2.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Número de PArte";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Descripción";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Cantidad";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 80;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Status";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 80;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Número de Lote";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Fecha de Caducidad";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Referencia de Cliente";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Tipo de Cambio";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Orden de Compra";
            dataGridViewTextBoxColumn9.MinimumWidth = 6;
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.Width = 125;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Pedimento";
            dataGridViewTextBoxColumn10.MinimumWidth = 6;
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.Width = 125;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1309, 44);
            panel2.TabIndex = 3;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.codigoBarras;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(3, 14);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(192, 26);
            button1.TabIndex = 45;
            button1.Text = "Escanear";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // dataGridView3
            // 
            dataGridViewCellStyle3.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView3.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Columns.AddRange(new DataGridViewColumn[] { Esta, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn12, Estandar, Maxima, Envia, SD, Recibida, Statuss, Ubic, Lote, Cad, Rf });
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(0, 0);
            dataGridView3.Margin = new Padding(3, 2, 3, 2);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.Size = new Size(1309, 139);
            dataGridView3.TabIndex = 3;
            // 
            // Esta
            // 
            Esta.HeaderText = "EstandarID";
            Esta.MinimumWidth = 6;
            Esta.Name = "Esta";
            Esta.Width = 125;
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "Número de PArte";
            dataGridViewTextBoxColumn11.MinimumWidth = 6;
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.Width = 200;
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewTextBoxColumn12.HeaderText = "Descripción";
            dataGridViewTextBoxColumn12.MinimumWidth = 6;
            dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            dataGridViewTextBoxColumn12.Width = 200;
            // 
            // Estandar
            // 
            Estandar.HeaderText = "Estandar";
            Estandar.MinimumWidth = 6;
            Estandar.Name = "Estandar";
            Estandar.Width = 125;
            // 
            // Maxima
            // 
            Maxima.HeaderText = "Maxima";
            Maxima.MinimumWidth = 6;
            Maxima.Name = "Maxima";
            Maxima.Width = 125;
            // 
            // Envia
            // 
            Envia.HeaderText = "Enviado";
            Envia.MinimumWidth = 6;
            Envia.Name = "Envia";
            Envia.Width = 125;
            // 
            // SD
            // 
            SD.HeaderText = "SD";
            SD.MinimumWidth = 6;
            SD.Name = "SD";
            SD.Width = 125;
            // 
            // Recibida
            // 
            Recibida.HeaderText = "Recibida";
            Recibida.MinimumWidth = 6;
            Recibida.Name = "Recibida";
            Recibida.Width = 125;
            // 
            // Statuss
            // 
            Statuss.HeaderText = "Status";
            Statuss.MinimumWidth = 6;
            Statuss.Name = "Statuss";
            Statuss.Width = 125;
            // 
            // Ubic
            // 
            Ubic.HeaderText = "Ubicación";
            Ubic.MinimumWidth = 6;
            Ubic.Name = "Ubic";
            Ubic.Width = 125;
            // 
            // Lote
            // 
            Lote.HeaderText = "Número de Lote";
            Lote.MinimumWidth = 6;
            Lote.Name = "Lote";
            Lote.Width = 125;
            // 
            // Cad
            // 
            Cad.HeaderText = "Fecha de Caducidad";
            Cad.MinimumWidth = 6;
            Cad.Name = "Cad";
            Cad.Width = 125;
            // 
            // Rf
            // 
            Rf.HeaderText = "Referencia";
            Rf.MinimumWidth = 6;
            Rf.Name = "Rf";
            Rf.Width = 125;
            // 
            // ASN
            // 
            ASN.HeaderText = "ASN";
            ASN.MinimumWidth = 6;
            ASN.Name = "ASN";
            ASN.Width = 125;
            // 
            // Cliente
            // 
            Cliente.HeaderText = "Cliente";
            Cliente.MinimumWidth = 6;
            Cliente.Name = "Cliente";
            Cliente.Width = 125;
            // 
            // Proyecto
            // 
            Proyecto.HeaderText = "Proyecto";
            Proyecto.MinimumWidth = 6;
            Proyecto.Name = "Proyecto";
            Proyecto.Width = 125;
            // 
            // Factura
            // 
            Factura.HeaderText = "Factura";
            Factura.MinimumWidth = 6;
            Factura.Name = "Factura";
            Factura.Width = 125;
            // 
            // Guia
            // 
            Guia.HeaderText = "Guia";
            Guia.MinimumWidth = 6;
            Guia.Name = "Guia";
            Guia.Width = 125;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.Width = 70;
            // 
            // Bultos
            // 
            Bultos.HeaderText = "Bultos";
            Bultos.MinimumWidth = 6;
            Bultos.Name = "Bultos";
            Bultos.Width = 70;
            // 
            // Origen
            // 
            Origen.HeaderText = "ORigen";
            Origen.MinimumWidth = 6;
            Origen.Name = "Origen";
            Origen.Width = 125;
            // 
            // Fecha
            // 
            Fecha.HeaderText = "Fecha";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 80;
            // 
            // Hora
            // 
            Hora.HeaderText = "Hora";
            Hora.MinimumWidth = 6;
            Hora.Name = "Hora";
            Hora.Width = 70;
            // 
            // ETA
            // 
            ETA.HeaderText = "ETA";
            ETA.MinimumWidth = 6;
            ETA.Name = "ETA";
            ETA.Width = 125;
            // 
            // Ordend
            // 
            Ordend.HeaderText = "Orden de compra";
            Ordend.MinimumWidth = 6;
            Ordend.Name = "Ordend";
            Ordend.Width = 125;
            // 
            // FrmASN
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1309, 541);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmASN";
            Text = "ASN";
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            gridContainer.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        
        private FlowLayoutPanel flowLayoutPanel1;
        private Button AddBtn;
        private DataGridView dataGridView1;
        private Button button4;
        private Button reloadBtn;
        private Panel gridContainer;
        private SplitContainer splitContainer1;
        private DataGridView dataGridView2;
        private SplitContainer splitContainer2;
        private DataGridView dataGridView3;
        private Panel panel2;
        private Button button1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn Esta;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn Estandar;
        private DataGridViewTextBoxColumn Maxima;
        private DataGridViewTextBoxColumn Envia;
        private DataGridViewTextBoxColumn SD;
        private DataGridViewTextBoxColumn Recibida;
        private DataGridViewTextBoxColumn Statuss;
        private DataGridViewTextBoxColumn Ubic;
        private DataGridViewTextBoxColumn Lote;
        private DataGridViewTextBoxColumn Cad;
        private DataGridViewTextBoxColumn Rf;
        private DataGridViewTextBoxColumn ASN;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn Factura;
        private DataGridViewTextBoxColumn Guia;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Bultos;
        private DataGridViewTextBoxColumn Origen;
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Hora;
        private DataGridViewTextBoxColumn ETA;
        private DataGridViewTextBoxColumn Ordend;
    }
}
