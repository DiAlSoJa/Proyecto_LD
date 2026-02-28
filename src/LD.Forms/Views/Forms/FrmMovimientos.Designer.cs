using System;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    partial class FrmMovimientos
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
            button6 = new Button();
            dataGridView1 = new DataGridView();
            Cliente = new DataGridViewTextBoxColumn();
            Proyecto = new DataGridViewTextBoxColumn();
            NumPArte = new DataGridViewTextBoxColumn();
            Descripcion = new DataGridViewTextBoxColumn();
            Fecha = new DataGridViewTextBoxColumn();
            Hora = new DataGridViewTextBoxColumn();
            Usuario = new DataGridViewTextBoxColumn();
            Documento = new DataGridViewTextBoxColumn();
            TipoDoc = new DataGridViewTextBoxColumn();
            DUB = new DataGridViewTextBoxColumn();
            UbicacionOr = new DataGridViewTextBoxColumn();
            UbicacionF = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            StatusF = new DataGridViewTextBoxColumn();
            StandarId = new DataGridViewTextBoxColumn();
            gridContainer = new Panel();
            groupBox1 = new GroupBox();
            label2 = new Label();
            txtComercialName = new TextBox();
            comboBox1 = new ComboBox();
            label8 = new Label();
            comboBox2 = new ComboBox();
            label5 = new Label();
            label1 = new Label();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            label3 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            gridContainer.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1791, 194);
            panel1.TabIndex = 0;
            // 
            // button6
            // 
            button6.Image = Properties.Resources.search2;
            button6.ImageAlign = ContentAlignment.MiddleLeft;
            button6.Location = new Point(760, 104);
            button6.Name = "button6";
            button6.Size = new Size(147, 35);
            button6.TabIndex = 6;
            button6.Text = "Buscar";
            button6.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Cliente, Proyecto, NumPArte, Descripcion, Fecha, Hora, Usuario, Documento, TipoDoc, DUB, UbicacionOr, UbicacionF, Status, StatusF, StandarId });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1791, 527);
            dataGridView1.TabIndex = 1;
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
            // Fecha
            // 
            Fecha.HeaderText = "Fecha";
            Fecha.MinimumWidth = 6;
            Fecha.Name = "Fecha";
            Fecha.Width = 125;
            // 
            // Hora
            // 
            Hora.HeaderText = "Hora";
            Hora.MinimumWidth = 6;
            Hora.Name = "Hora";
            Hora.Width = 125;
            // 
            // Usuario
            // 
            Usuario.HeaderText = "Usuario";
            Usuario.MinimumWidth = 6;
            Usuario.Name = "Usuario";
            Usuario.Width = 125;
            // 
            // Documento
            // 
            Documento.HeaderText = "Documento";
            Documento.MinimumWidth = 6;
            Documento.Name = "Documento";
            Documento.Width = 125;
            // 
            // TipoDoc
            // 
            TipoDoc.HeaderText = "Tipo de documento";
            TipoDoc.MinimumWidth = 6;
            TipoDoc.Name = "TipoDoc";
            TipoDoc.Width = 125;
            // 
            // DUB
            // 
            DUB.HeaderText = "DUB";
            DUB.MinimumWidth = 6;
            DUB.Name = "DUB";
            DUB.Width = 125;
            // 
            // UbicacionOr
            // 
            UbicacionOr.HeaderText = "Ubicación origen";
            UbicacionOr.MinimumWidth = 6;
            UbicacionOr.Name = "UbicacionOr";
            UbicacionOr.Width = 125;
            // 
            // UbicacionF
            // 
            UbicacionF.HeaderText = "Ubicación fnial";
            UbicacionF.MinimumWidth = 6;
            UbicacionF.Name = "UbicacionF";
            UbicacionF.Width = 125;
            // 
            // Status
            // 
            Status.HeaderText = "Status origen";
            Status.MinimumWidth = 6;
            Status.Name = "Status";
            Status.Width = 125;
            // 
            // StatusF
            // 
            StatusF.HeaderText = "Status final";
            StatusF.MinimumWidth = 6;
            StatusF.Name = "StatusF";
            StatusF.Width = 125;
            // 
            // StandarId
            // 
            StandarId.HeaderText = "StantarID";
            StandarId.MinimumWidth = 6;
            StandarId.Name = "StandarId";
            StandarId.Width = 125;
            // 
            // gridContainer
            // 
            gridContainer.Controls.Add(dataGridView1);
            gridContainer.Dock = DockStyle.Fill;
            gridContainer.Location = new Point(0, 194);
            gridContainer.Name = "gridContainer";
            gridContainer.Size = new Size(1791, 527);
            gridContainer.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dateTimePicker2);
            groupBox1.Controls.Add(button6);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtComercialName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1304, 164);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtros";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 76);
            label2.Name = "label2";
            label2.Size = new Size(124, 20);
            label2.TabIndex = 23;
            label2.Text = "Número de Parte:";
            // 
            // txtComercialName
            // 
            txtComercialName.BorderStyle = BorderStyle.FixedSingle;
            txtComercialName.Font = new Font("Segoe UI", 9.75F);
            txtComercialName.Location = new Point(181, 73);
            txtComercialName.Name = "txtComercialName";
            txtComercialName.Size = new Size(527, 29);
            txtComercialName.TabIndex = 22;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 9.75F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(181, 26);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(465, 29);
            comboBox1.TabIndex = 36;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(101, 35);
            label8.Name = "label8";
            label8.Size = new Size(58, 20);
            label8.TabIndex = 38;
            label8.Text = "Cliente:";
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 9.75F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(760, 26);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(465, 29);
            comboBox2.TabIndex = 37;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(672, 30);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 39;
            label5.Text = "Proyecto:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 119);
            label1.Name = "label1";
            label1.Size = new Size(86, 20);
            label1.TabIndex = 40;
            label1.Text = "Fecha Incio:";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(181, 114);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(149, 27);
            dateTimePicker1.TabIndex = 41;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(559, 112);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(149, 27);
            dateTimePicker2.TabIndex = 43;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(436, 117);
            label3.Name = "label3";
            label3.Size = new Size(73, 20);
            label3.TabIndex = 42;
            label3.Text = "Fecha Fin:";
            // 
            // FrmMovimientos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1791, 721);
            Controls.Add(gridContainer);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmMovimientos";
            Text = "FrmPlantillaForm";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            gridContainer.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView1;
        private Button button6;
        private Panel gridContainer;
        private DataGridViewTextBoxColumn Cliente;
        private DataGridViewTextBoxColumn Proyecto;
        private DataGridViewTextBoxColumn NumPArte;
        private DataGridViewTextBoxColumn Descripcion;
        private DataGridViewTextBoxColumn Fecha;
        private DataGridViewTextBoxColumn Hora;
        private DataGridViewTextBoxColumn Usuario;
        private DataGridViewTextBoxColumn Documento;
        private DataGridViewTextBoxColumn TipoDoc;
        private DataGridViewTextBoxColumn DUB;
        private DataGridViewTextBoxColumn UbicacionOr;
        private DataGridViewTextBoxColumn UbicacionF;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn StatusF;
        private DataGridViewTextBoxColumn StandarId;
        private GroupBox groupBox1;
        private Label label2;
        private TextBox txtComercialName;
        private DateTimePicker dateTimePicker2;
        private Label label3;
        private DateTimePicker dateTimePicker1;
        private Label label1;
        private ComboBox comboBox1;
        private Label label8;
        private ComboBox comboBox2;
        private Label label5;
    }
}