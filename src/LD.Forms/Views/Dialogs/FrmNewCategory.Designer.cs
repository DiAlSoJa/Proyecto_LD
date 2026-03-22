namespace LD.Forms.Views.Dialogs
{
    partial class FrmNewCategory
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
            panel1 = new Panel();
            txtFrecuencia = new TextBox();
            label6 = new Label();
            cmbProyecto = new ComboBox();
            label4 = new Label();
            cmbCliente = new ComboBox();
            label3 = new Label();
            txtNombre = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            txtId = new TextBox();
            label5 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            label2 = new Label();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(txtFrecuencia);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(cmbProyecto);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(cmbCliente);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtNombre);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(txtId);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(689, 346);
            panel1.TabIndex = 1;
            // 
            // txtFrecuencia
            // 
            txtFrecuencia.BorderStyle = BorderStyle.FixedSingle;
            txtFrecuencia.Font = new Font("Segoe UI", 9.75F);
            txtFrecuencia.Location = new Point(140, 229);
            txtFrecuencia.Name = "txtFrecuencia";
            txtFrecuencia.Size = new Size(68, 29);
            txtFrecuencia.TabIndex = 5;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 237);
            label6.Name = "label6";
            label6.Size = new Size(82, 20);
            label6.TabIndex = 73;
            label6.Text = "Frecuencia:";
            // 
            // cmbProyecto
            // 
            cmbProyecto.FormattingEnabled = true;
            cmbProyecto.Location = new Point(140, 110);
            cmbProyecto.Name = "cmbProyecto";
            cmbProyecto.Size = new Size(402, 28);
            cmbProyecto.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 113);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 71;
            label4.Text = "Proyecto:";
            // 
            // cmbCliente
            // 
            cmbCliente.FormattingEnabled = true;
            cmbCliente.Location = new Point(140, 72);
            cmbCliente.Name = "cmbCliente";
            cmbCliente.Size = new Size(402, 28);
            cmbCliente.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 80);
            label3.Name = "label3";
            label3.Size = new Size(58, 20);
            label3.TabIndex = 69;
            label3.Text = "Cliente:";
            // 
            // txtNombre
            // 
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 9.75F);
            txtNombre.Location = new Point(140, 185);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(456, 29);
            txtNombre.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 289);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(687, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(503, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(326, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(171, 35);
            btnSave.TabIndex = 6;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtId
            // 
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.Location = new Point(140, 144);
            txtId.Name = "txtId";
            txtId.Size = new Size(68, 29);
            txtId.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 199);
            label5.Name = "label5";
            label5.Size = new Size(67, 20);
            label5.TabIndex = 67;
            label5.Text = "Nombre:";
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(687, 35);
            panel2.TabIndex = 4;
            panel2.DoubleClick += panel2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(21, 5);
            label1.Name = "label1";
            label1.Size = new Size(85, 22);
            label1.TabIndex = 3;
            label1.Text = "Categorias";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(650, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 152);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 66;
            label2.Text = "Categoría:";
            // 
            // FrmNewCategory
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(689, 346);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNewCategory";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Categorias";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private Button button2;
        private Button btnSave;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox6;
        private TextBox txtNombre;
        private TextBox txtId;
        private Label label5;
        private Label label2;
        private ComboBox cmbProyecto;
        private Label label4;
        private ComboBox cmbCliente;
        private Label label3;
        private TextBox txtFrecuencia;
        private Label label6;
    }
}