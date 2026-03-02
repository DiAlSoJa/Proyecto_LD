namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevaUbicacion
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
            groupBox4 = new GroupBox();
            checkCortina = new CheckBox();
            checkPaso = new CheckBox();
            groupBox2 = new GroupBox();
            groupBox5 = new GroupBox();
            radioSencillo = new RadioButton();
            radioDoble = new RadioButton();
            groupBox3 = new GroupBox();
            radioCompartidoType = new RadioButton();
            radioRack = new RadioButton();
            radioCompartido = new RadioButton();
            radioCuarentena = new RadioButton();
            radioReciboEmbarque = new RadioButton();
            radioEmbarque = new RadioButton();
            radioGeneral = new RadioButton();
            groupBox1 = new GroupBox();
            label12 = new Label();
            txtProfundidadCm = new TextBox();
            label13 = new Label();
            label10 = new Label();
            txtAnchoCm = new TextBox();
            label11 = new Label();
            label9 = new Label();
            txtAltoCm = new TextBox();
            label3 = new Label();
            cmbAlmacen = new ComboBox();
            checkIsActive = new CheckBox();
            checkTemperatura = new CheckBox();
            checkIsFiscal = new CheckBox();
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
            groupBox4.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(groupBox4);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(cmbAlmacen);
            panel1.Controls.Add(checkIsActive);
            panel1.Controls.Add(checkTemperatura);
            panel1.Controls.Add(checkIsFiscal);
            panel1.Controls.Add(txtNombreUbicacion);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1130, 472);
            panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(checkCortina);
            groupBox4.Controls.Add(checkPaso);
            groupBox4.Location = new Point(460, 346);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(610, 63);
            groupBox4.TabIndex = 47;
            groupBox4.TabStop = false;
            // 
            // checkCortina
            // 
            checkCortina.AutoSize = true;
            checkCortina.Location = new Point(134, 26);
            checkCortina.Name = "checkCortina";
            checkCortina.Size = new Size(79, 24);
            checkCortina.TabIndex = 17;
            checkCortina.Text = "Cortina";
            checkCortina.UseVisualStyleBackColor = true;
            // 
            // checkPaso
            // 
            checkPaso.AutoSize = true;
            checkPaso.Location = new Point(44, 26);
            checkPaso.Name = "checkPaso";
            checkPaso.Size = new Size(61, 24);
            checkPaso.TabIndex = 16;
            checkPaso.Text = "Paso";
            checkPaso.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(radioCompartido);
            groupBox2.Controls.Add(radioCuarentena);
            groupBox2.Controls.Add(radioReciboEmbarque);
            groupBox2.Controls.Add(radioEmbarque);
            groupBox2.Controls.Add(radioGeneral);
            groupBox2.Location = new Point(460, 120);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(610, 288);
            groupBox2.TabIndex = 46;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tipo de ubicación";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(radioSencillo);
            groupBox5.Controls.Add(radioDoble);
            groupBox5.Location = new Point(24, 130);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(205, 93);
            groupBox5.TabIndex = 50;
            groupBox5.TabStop = false;
            // 
            // radioSencillo
            // 
            radioSencillo.AutoSize = true;
            radioSencillo.Location = new Point(25, 56);
            radioSencillo.Name = "radioSencillo";
            radioSencillo.Size = new Size(82, 24);
            radioSencillo.TabIndex = 10;
            radioSencillo.TabStop = true;
            radioSencillo.Text = "Sencillo";
            radioSencillo.UseVisualStyleBackColor = true;
            // 
            // radioDoble
            // 
            radioDoble.AutoSize = true;
            radioDoble.Location = new Point(25, 26);
            radioDoble.Name = "radioDoble";
            radioDoble.Size = new Size(71, 24);
            radioDoble.TabIndex = 9;
            radioDoble.TabStop = true;
            radioDoble.Text = "Doble";
            radioDoble.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioCompartidoType);
            groupBox3.Controls.Add(radioRack);
            groupBox3.Location = new Point(24, 26);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(205, 103);
            groupBox3.TabIndex = 49;
            groupBox3.TabStop = false;
            // 
            // radioCompartidoType
            // 
            radioCompartidoType.AutoSize = true;
            radioCompartidoType.Location = new Point(25, 56);
            radioCompartidoType.Name = "radioCompartidoType";
            radioCompartidoType.Size = new Size(110, 24);
            radioCompartidoType.TabIndex = 10;
            radioCompartidoType.TabStop = true;
            radioCompartidoType.Text = "Compartido";
            radioCompartidoType.UseVisualStyleBackColor = true;
            // 
            // radioRack
            // 
            radioRack.AutoSize = true;
            radioRack.Location = new Point(25, 26);
            radioRack.Name = "radioRack";
            radioRack.Size = new Size(61, 24);
            radioRack.TabIndex = 9;
            radioRack.TabStop = true;
            radioRack.Text = "Rack";
            radioRack.UseVisualStyleBackColor = true;
            // 
            // radioCompartido
            // 
            radioCompartido.AutoSize = true;
            radioCompartido.Location = new Point(259, 74);
            radioCompartido.Name = "radioCompartido";
            radioCompartido.Size = new Size(110, 24);
            radioCompartido.TabIndex = 12;
            radioCompartido.TabStop = true;
            radioCompartido.Text = "Compartido";
            radioCompartido.UseVisualStyleBackColor = true;
            // 
            // radioCuarentena
            // 
            radioCuarentena.AutoSize = true;
            radioCuarentena.Location = new Point(457, 42);
            radioCuarentena.Name = "radioCuarentena";
            radioCuarentena.Size = new Size(105, 24);
            radioCuarentena.TabIndex = 14;
            radioCuarentena.TabStop = true;
            radioCuarentena.Text = "Cuarentena";
            radioCuarentena.UseVisualStyleBackColor = true;
            // 
            // radioReciboEmbarque
            // 
            radioReciboEmbarque.AutoSize = true;
            radioReciboEmbarque.Location = new Point(259, 105);
            radioReciboEmbarque.Name = "radioReciboEmbarque";
            radioReciboEmbarque.Size = new Size(159, 24);
            radioReciboEmbarque.TabIndex = 13;
            radioReciboEmbarque.TabStop = true;
            radioReciboEmbarque.Text = "Recibo y embarque";
            radioReciboEmbarque.UseVisualStyleBackColor = true;
            // 
            // radioEmbarque
            // 
            radioEmbarque.AutoSize = true;
            radioEmbarque.Location = new Point(457, 71);
            radioEmbarque.Name = "radioEmbarque";
            radioEmbarque.Size = new Size(98, 24);
            radioEmbarque.TabIndex = 15;
            radioEmbarque.TabStop = true;
            radioEmbarque.Text = "Embarque";
            radioEmbarque.UseVisualStyleBackColor = true;
            // 
            // radioGeneral
            // 
            radioGeneral.AutoSize = true;
            radioGeneral.Location = new Point(259, 44);
            radioGeneral.Name = "radioGeneral";
            radioGeneral.Size = new Size(81, 24);
            radioGeneral.TabIndex = 11;
            radioGeneral.TabStop = true;
            radioGeneral.Text = "General";
            radioGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(txtProfundidadCm);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(txtAnchoCm);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(txtAltoCm);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(27, 210);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(413, 168);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            groupBox1.Text = "Dimensiones";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(276, 114);
            label12.Name = "label12";
            label12.Size = new Size(32, 20);
            label12.TabIndex = 46;
            label12.Text = "cm.";
            // 
            // txtProfundidadCm
            // 
            txtProfundidadCm.BorderStyle = BorderStyle.FixedSingle;
            txtProfundidadCm.Font = new Font("Segoe UI", 9.75F);
            txtProfundidadCm.Location = new Point(161, 105);
            txtProfundidadCm.Name = "txtProfundidadCm";
            txtProfundidadCm.Size = new Size(109, 29);
            txtProfundidadCm.TabIndex = 8;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(24, 114);
            label13.Name = "label13";
            label13.Size = new Size(94, 20);
            label13.TabIndex = 44;
            label13.Text = "Profundidad:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(276, 79);
            label10.Name = "label10";
            label10.Size = new Size(32, 20);
            label10.TabIndex = 43;
            label10.Text = "cm.";
            // 
            // txtAnchoCm
            // 
            txtAnchoCm.BorderStyle = BorderStyle.FixedSingle;
            txtAnchoCm.Font = new Font("Segoe UI", 9.75F);
            txtAnchoCm.Location = new Point(161, 70);
            txtAnchoCm.Name = "txtAnchoCm";
            txtAnchoCm.Size = new Size(109, 29);
            txtAnchoCm.TabIndex = 7;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(24, 79);
            label11.Name = "label11";
            label11.Size = new Size(54, 20);
            label11.TabIndex = 41;
            label11.Text = "Ancho:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(276, 44);
            label9.Name = "label9";
            label9.Size = new Size(32, 20);
            label9.TabIndex = 40;
            label9.Text = "cm.";
            // 
            // txtAltoCm
            // 
            txtAltoCm.BorderStyle = BorderStyle.FixedSingle;
            txtAltoCm.Font = new Font("Segoe UI", 9.75F);
            txtAltoCm.Location = new Point(161, 35);
            txtAltoCm.Name = "txtAltoCm";
            txtAltoCm.Size = new Size(109, 29);
            txtAltoCm.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 44);
            label3.Name = "label3";
            label3.Size = new Size(40, 20);
            label3.TabIndex = 38;
            label3.Text = "Alto:";
            // 
            // cmbAlmacen
            // 
            cmbAlmacen.FormattingEnabled = true;
            cmbAlmacen.Location = new Point(188, 51);
            cmbAlmacen.Name = "cmbAlmacen";
            cmbAlmacen.Size = new Size(402, 28);
            cmbAlmacen.TabIndex = 1;
            // 
            // checkIsActive
            // 
            checkIsActive.AutoSize = true;
            checkIsActive.Location = new Point(188, 120);
            checkIsActive.Name = "checkIsActive";
            checkIsActive.Size = new Size(73, 24);
            checkIsActive.TabIndex = 3;
            checkIsActive.Text = "Activo";
            checkIsActive.UseVisualStyleBackColor = true;
            // 
            // checkTemperatura
            // 
            checkTemperatura.AutoSize = true;
            checkTemperatura.Location = new Point(188, 180);
            checkTemperatura.Name = "checkTemperatura";
            checkTemperatura.Size = new Size(191, 24);
            checkTemperatura.TabIndex = 5;
            checkTemperatura.Text = "Temperatura controlada";
            checkTemperatura.UseVisualStyleBackColor = true;
            // 
            // checkIsFiscal
            // 
            checkIsFiscal.AutoSize = true;
            checkIsFiscal.Location = new Point(188, 150);
            checkIsFiscal.Name = "checkIsFiscal";
            checkIsFiscal.Size = new Size(67, 24);
            checkIsFiscal.TabIndex = 4;
            checkIsFiscal.Text = "Fiscal";
            checkIsFiscal.UseVisualStyleBackColor = true;
            // 
            // txtNombreUbicacion
            // 
            txtNombreUbicacion.BorderStyle = BorderStyle.FixedSingle;
            txtNombreUbicacion.Font = new Font("Segoe UI", 9.75F);
            txtNombreUbicacion.Location = new Point(188, 85);
            txtNombreUbicacion.Name = "txtNombreUbicacion";
            txtNombreUbicacion.Size = new Size(252, 29);
            txtNombreUbicacion.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(27, 92);
            label8.Name = "label8";
            label8.Size = new Size(75, 20);
            label8.TabIndex = 35;
            label8.Text = "Ubicacion";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 59);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 27;
            label2.Text = "Almacén:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 415);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1128, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(943, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 19;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(765, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(172, 35);
            btnSave.TabIndex = 18;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.ForestGreen;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1128, 35);
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
            label1.Size = new Size(125, 22);
            label1.TabIndex = 3;
            label1.Text = "Nueva ubicación";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1092, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevaUbicacion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 472);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevaUbicacion";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private CheckBox checkIsActive;
        private CheckBox checkTemperatura;
        private TextBox txtAltoCm;
        private Label label3;
        private CheckBox checkIsFiscal;
        private TextBox txtNombreUbicacion;
        private Label label8;
        private Label label2;
        private ComboBox cmbAlmacen;
        private GroupBox groupBox1;
        private Label label12;
        private TextBox txtProfundidadCm;
        private Label label13;
        private Label label10;
        private TextBox txtAnchoCm;
        private Label label11;
        private Label label9;
        private GroupBox groupBox2;
        private RadioButton radioCompartidoType;
        private RadioButton radioRack;
        private RadioButton radioGeneral;
        private RadioButton radioEmbarque;
        private RadioButton radioCuarentena;
        private RadioButton radioReciboEmbarque;
        private RadioButton radioCompartido;
        private CheckBox checkPaso;
        private CheckBox checkCortina;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox5;
        private RadioButton radioSencillo;
        private RadioButton radioDoble;
    }
}