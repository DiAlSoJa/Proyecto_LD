using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmInventario
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
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Fecha = new DataGridViewTextBoxColumn();
            Almacen = new DataGridViewTextBoxColumn();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            Esta = new DataGridViewTextBoxColumn();
            FechaR = new DataGridViewTextBoxColumn();
            FEchaS = new DataGridViewTextBoxColumn();
            AS = new DataGridViewTextBoxColumn();
            Ubicacion = new DataGridViewTextBoxColumn();
            Parti = new DataGridViewTextBoxColumn();
            Orig = new DataGridViewTextBoxColumn();
            NumPArte = new DataGridViewTextBoxColumn();
            Descripcion = new DataGridViewTextBoxColumn();
            Rrec = new DataGridViewTextBoxColumn();
            Dis = new DataGridViewTextBoxColumn();
            Statu = new DataGridViewTextBoxColumn();
            NoLo = new DataGridViewTextBoxColumn();
            Cadu = new DataGridViewTextBoxColumn();
            Oc = new DataGridViewTextBoxColumn();
            Ped = new DataGridViewTextBoxColumn();
            Tipoc = new DataGridViewTextBoxColumn();
            SD = new DataGridViewTextBoxColumn();
            Estado = new DataGridViewTextBoxColumn();
            DiasTras = new DataGridViewTextBoxColumn();
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
            panel1.Size = new Size(1624, 47);
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
            flowLayoutPanel1.Size = new Size(448, 47);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.update;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(23, 3);
            button6.Name = "button6";
            button6.Size = new Size(124, 35);
            button6.TabIndex = 6;
            button6.Text = "Actualizar";
            button6.UseVisualStyleBackColor = true;
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Fecha, Almacen, Cliente, Proyecto, Esta, FechaR, FEchaS, AS, Ubicacion, Parti, Orig, NumPArte, Descripcion, Rrec, Dis, Statu, NoLo, Cadu, Oc, Ped, Tipoc, SD, Estado, DiasTras });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 47);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1624, 674);
            dataGridView1.TabIndex = 1;
            // 
            // Fecha
            // 
            Fecha.HeaderText = "Fecha";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 125;
            // 
            // Almacen
            // 
            Almacen.HeaderText = "Almacén";
            Almacen.MinimumWidth = 6;
            Almacen.Name = "Almacen";
            Almacen.Width = 125;
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
            // Esta
            // 
            Esta.HeaderText = "EstandarID";
            Esta.MinimumWidth = 6;
            Esta.Name = "Esta";
            Esta.Width = 125;
            // 
            // FechaR
            // 
            FechaR.HeaderText = "Fecha Recepción";
            FechaR.MinimumWidth = 6;
            FechaR.Name = "FechaR";
            FechaR.Width = 125;
            // 
            // FEchaS
            // 
            FEchaS.HeaderText = "Fecha Storing";
            FEchaS.MinimumWidth = 6;
            FEchaS.Name = "FEchaS";
            FEchaS.Width = 125;
            // 
            // AS
            // 
            AS.HeaderText = "ASN";
            AS.MinimumWidth = 6;
            AS.Name = "AS";
            AS.Width = 125;
            // 
            // Ubicacion
            // 
            Ubicacion.HeaderText = "Ubicación";
            Ubicacion.MinimumWidth = 6;
            Ubicacion.Name = "Ubicacion";
            Ubicacion.Width = 125;
            // 
            // Parti
            // 
            Parti.HeaderText = "Particion";
            Parti.MinimumWidth = 6;
            Parti.Name = "Parti";
            Parti.Width = 125;
            // 
            // Orig
            // 
            Orig.HeaderText = "Origen";
            Orig.MinimumWidth = 6;
            Orig.Name = "Orig";
            Orig.Width = 125;
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
            // Rrec
            // 
            Rrec.HeaderText = "Recibida";
            Rrec.MinimumWidth = 6;
            Rrec.Name = "Rrec";
            Rrec.Width = 125;
            // 
            // Dis
            // 
            Dis.HeaderText = "Disponible";
            Dis.MinimumWidth = 6;
            Dis.Name = "Dis";
            Dis.Width = 125;
            // 
            // Statu
            // 
            Statu.HeaderText = "Status";
            Statu.MinimumWidth = 6;
            Statu.Name = "Statu";
            Statu.Width = 125;
            // 
            // NoLo
            // 
            NoLo.HeaderText = "Número de Lote";
            NoLo.MinimumWidth = 6;
            NoLo.Name = "NoLo";
            NoLo.Width = 125;
            // 
            // Cadu
            // 
            Cadu.HeaderText = "Caducidad";
            Cadu.MinimumWidth = 6;
            Cadu.Name = "Cadu";
            Cadu.Width = 125;
            // 
            // Oc
            // 
            Oc.HeaderText = "Orden de Compra";
            Oc.MinimumWidth = 6;
            Oc.Name = "Oc";
            Oc.Width = 125;
            // 
            // Ped
            // 
            Ped.HeaderText = "Pedimento";
            Ped.MinimumWidth = 6;
            Ped.Name = "Ped";
            Ped.Width = 125;
            // 
            // Tipoc
            // 
            Tipoc.HeaderText = "Tipo de Cambio";
            Tipoc.MinimumWidth = 6;
            Tipoc.Name = "Tipoc";
            Tipoc.Width = 125;
            // 
            // SD
            // 
            SD.HeaderText = "SD";
            SD.MinimumWidth = 6;
            SD.Name = "SD";
            SD.Width = 125;
            // 
            // Estado
            // 
            Estado.HeaderText = "Estado";
            Estado.MinimumWidth = 6;
            Estado.Name = "Estado";
            Estado.Width = 125;
            // 
            // DiasTras
            // 
            DiasTras.HeaderText = "Días Transcurridos";
            DiasTras.MinimumWidth = 6;
            DiasTras.Name = "DiasTras";
            DiasTras.Width = 125;
            // 
            // FrmInventario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1624, 721);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmInventario";
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
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Almacen;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn Esta;
        private DataGridViewTextBoxColumn FechaR;
        private DataGridViewTextBoxColumn FEchaS;
        private DataGridViewTextBoxColumn AS;
        private DataGridViewTextBoxColumn Ubicacion;
        private DataGridViewTextBoxColumn Parti;
        private DataGridViewTextBoxColumn Orig;
        private DataGridViewTextBoxColumn NumPArte;
        private DataGridViewTextBoxColumn Descripcion;
        private DataGridViewTextBoxColumn Rrec;
        private DataGridViewTextBoxColumn Dis;
        private DataGridViewTextBoxColumn Statu;
        private DataGridViewTextBoxColumn NoLo;
        private DataGridViewTextBoxColumn Cadu;
        private DataGridViewTextBoxColumn Oc;
        private DataGridViewTextBoxColumn Ped;
        private DataGridViewTextBoxColumn Tipoc;
        private DataGridViewTextBoxColumn SD;
        private DataGridViewTextBoxColumn Estado;
        private DataGridViewTextBoxColumn DiasTras;
    }
}