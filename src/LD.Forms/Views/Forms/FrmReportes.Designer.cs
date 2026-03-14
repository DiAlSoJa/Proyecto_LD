using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmReportes
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
            dataGridView1 = new DataGridView();
            button3 = new Button();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            dataGridView2 = new DataGridView();
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
            Nombre = new DataGridViewTextBoxColumn();
            Parametros = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Nombre, Parametros });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 62);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1395, 153);
            dataGridView1.TabIndex = 1;
            // 
            // button3
            // 
            button3.Image = Properties.Resources.enviar;
            button3.ImageAlign = ContentAlignment.MiddleLeft;
            button3.Location = new Point(8, 8);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(178, 26);
            button3.TabIndex = 9;
            button3.Text = "Ejecutar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10);
            panel1.Size = new Size(1421, 238);
            panel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Controls.Add(panel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(10, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1401, 218);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Querys";
            // 
            // panel2
            // 
            panel2.Controls.Add(button3);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(3, 19);
            panel2.Name = "panel2";
            panel2.Size = new Size(1395, 43);
            panel2.TabIndex = 11;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 238);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1421, 420);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "Resultado";
            // 
            // dataGridView2
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView2.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Columns.AddRange(new DataGridViewColumn[] { Fecha, Almacen, Cliente, Proyecto, Esta, FechaR, FEchaS, AS, Ubicacion, Parti, Orig, NumPArte, Descripcion, Rrec, Dis, Statu, NoLo, Cadu, Oc, Ped, Tipoc, SD, Estado, DiasTras });
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 19);
            dataGridView2.Margin = new Padding(3, 2, 3, 2);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.Size = new Size(1415, 398);
            dataGridView2.TabIndex = 2;
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
            // Nombre
            // 
            Nombre.HeaderText = "Nombre del query";
            Nombre.Name = "Nombre";
            Nombre.Width = 400;
            // 
            // Parametros
            // 
            Parametros.HeaderText = "Parametros";
            Parametros.Name = "Parametros";
            Parametros.Width = 200;
            // 
            // FrmReportes
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1421, 658);
            Controls.Add(groupBox2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmReportes";
            Text = "FrmPlantillaForm";
            Load += FrmReportes_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView1;
        private Button button3;
        private Panel panel1;
        private GroupBox groupBox1;
        private Panel panel2;
        private GroupBox groupBox2;
        private DataGridView dataGridView2;
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
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Parametros;
    }
}