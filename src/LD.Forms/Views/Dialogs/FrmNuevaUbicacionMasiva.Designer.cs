namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevaUbicacionMasiva
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
            panel4 = new Panel();
            panel3 = new Panel();
            label12 = new Label();
            txtHasta = new TextBox();
            txtNiveles = new TextBox();
            label7 = new Label();
            label6 = new Label();
            txtDesde = new TextBox();
            label9 = new Label();
            cmbAlmacenN = new ComboBox();
            txtRack = new TextBox();
            label10 = new Label();
            label11 = new Label();
            textBox3 = new TextBox();
            label5 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            
            txtNombreUbicacion = new TextBox();
            label8 = new Label();
            label2 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(675, 341);
            panel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(panel3);
            panel4.Controls.Add(textBox3);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(textBox1);
            panel4.Controls.Add(label3);
            
            panel4.Controls.Add(txtNombreUbicacion);
            panel4.Controls.Add(label8);
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 35);
            panel4.Name = "panel4";
            panel4.Size = new Size(673, 249);
            panel4.TabIndex = 10;
            // 
            // panel3
            // 
            panel3.Controls.Add(label12);
            panel3.Controls.Add(txtHasta);
            panel3.Controls.Add(txtNiveles);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(txtDesde);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(cmbAlmacenN);
            panel3.Controls.Add(txtRack);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(label11);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(673, 249);
            panel3.TabIndex = 46;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(313, 105);
            label12.Name = "label12";
            label12.Size = new Size(50, 20);
            label12.TabIndex = 47;
            label12.Text = "Hasta:";
            // 
            // txtHasta
            // 
            txtHasta.BorderStyle = BorderStyle.FixedSingle;
            txtHasta.Font = new Font("Segoe UI", 9.75F);
            txtHasta.Location = new Point(315, 128);
            txtHasta.Name = "txtHasta";
            txtHasta.Size = new Size(91, 29);
            txtHasta.TabIndex = 4;
            // 
            // txtNiveles
            // 
            txtNiveles.BorderStyle = BorderStyle.FixedSingle;
            txtNiveles.Font = new Font("Segoe UI", 9.75F);
            txtNiveles.Location = new Point(197, 174);
            txtNiveles.Name = "txtNiveles";
            txtNiveles.Size = new Size(92, 29);
            txtNiveles.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(36, 181);
            label7.Name = "label7";
            label7.Size = new Size(151, 20);
            label7.TabIndex = 45;
            label7.Text = "Niveles (entre 1 y 36):";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(195, 105);
            label6.Name = "label6";
            label6.Size = new Size(54, 20);
            label6.TabIndex = 43;
            label6.Text = "Desde:";
            // 
            // txtDesde
            // 
            txtDesde.BorderStyle = BorderStyle.FixedSingle;
            txtDesde.Font = new Font("Segoe UI", 9.75F);
            txtDesde.Location = new Point(197, 128);
            txtDesde.Name = "txtDesde";
            txtDesde.Size = new Size(92, 29);
            txtDesde.TabIndex = 3;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(36, 135);
            label9.Name = "label9";
            label9.Size = new Size(80, 20);
            label9.TabIndex = 41;
            label9.Text = "Posiciones:";
            // 
            // cmbAlmacenN
            // 
            cmbAlmacenN.FormattingEnabled = true;
            cmbAlmacenN.Location = new Point(195, 22);
            cmbAlmacenN.Name = "cmbAlmacenN";
            cmbAlmacenN.Size = new Size(402, 28);
            cmbAlmacenN.TabIndex = 1;
            // 
            // txtRack
            // 
            txtRack.BorderStyle = BorderStyle.FixedSingle;
            txtRack.Font = new Font("Segoe UI", 9.75F);
            txtRack.Location = new Point(195, 56);
            txtRack.Name = "txtRack";
            txtRack.Size = new Size(252, 29);
            txtRack.TabIndex = 2;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(34, 63);
            label10.Name = "label10";
            label10.Size = new Size(43, 20);
            label10.TabIndex = 39;
            label10.Text = "Rack:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(34, 30);
            label11.Name = "label11";
            label11.Size = new Size(70, 20);
            label11.TabIndex = 38;
            label11.Text = "Almacén:";
            // 
            // textBox3
            // 
            textBox3.BorderStyle = BorderStyle.FixedSingle;
            textBox3.Font = new Font("Segoe UI", 9.75F);
            textBox3.Location = new Point(195, 287);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(252, 29);
            textBox3.TabIndex = 44;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(34, 294);
            label5.Name = "label5";
            label5.Size = new Size(43, 20);
            label5.TabIndex = 45;
            label5.Text = "Rack:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(195, 105);
            label4.Name = "label4";
            label4.Size = new Size(54, 20);
            label4.TabIndex = 43;
            label4.Text = "Desde:";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9.75F);
            textBox1.Location = new Point(195, 139);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(252, 29);
            textBox1.TabIndex = 40;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(34, 146);
            label3.Name = "label3";
            label3.Size = new Size(80, 20);
            label3.TabIndex = 41;
            label3.Text = "Posiciones:";
            // 
            // cmbAlmacen
            // 
            cmbAlmacenN.FormattingEnabled = true;
            cmbAlmacenN.Location = new Point(195, 22);
            cmbAlmacenN.Name = "cmbAlmacen";
            cmbAlmacenN.Size = new Size(402, 28);
            cmbAlmacenN.TabIndex = 36;
            // 
            // txtNombreUbicacion
            // 
            txtNombreUbicacion.BorderStyle = BorderStyle.FixedSingle;
            txtNombreUbicacion.Font = new Font("Segoe UI", 9.75F);
            txtNombreUbicacion.Location = new Point(195, 56);
            txtNombreUbicacion.Name = "txtNombreUbicacion";
            txtNombreUbicacion.Size = new Size(252, 29);
            txtNombreUbicacion.TabIndex = 37;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(34, 63);
            label8.Name = "label8";
            label8.Size = new Size(43, 20);
            label8.TabIndex = 39;
            label8.Text = "Rack:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 30);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 38;
            label2.Text = "Almacén:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 284);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(673, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(504, 8);
            button2.Name = "button2";
            button2.Size = new Size(156, 35);
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
            btnSave.Size = new Size(172, 35);
            btnSave.TabIndex = 6;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.ForestGreen;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(673, 35);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(20, 5);
            label1.Name = "label1";
            label1.Size = new Size(177, 22);
            label1.TabIndex = 3;
            label1.Text = "Nueva ubicación másiva";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(637, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevaUbicacionMasiva
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(675, 341);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevaUbicacionMasiva";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
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
        private Panel panel4;
        private TextBox textBox3;
        private Label label5;
        private Label label4;
        private TextBox textBox1;
        private Label label3;
        private ComboBox cmbAlmacen;
        private TextBox txtNombreUbicacion;
        private Label label8;
        private Label label2;
        private Panel panel3;
        private Label label12;
        private TextBox txtHasta;
        private TextBox txtNiveles;
        private Label label7;
        private Label label6;
        private TextBox txtDesde;
        private Label label9;
        private ComboBox cmbAlmacenN;
        private TextBox txtRack;
        private Label label10;
        private Label label11;
    }
}