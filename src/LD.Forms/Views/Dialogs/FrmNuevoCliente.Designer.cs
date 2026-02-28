namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevoCliente
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
            tabControl1 = new TabControl();
            tabComercial = new TabPage();
            label3 = new Label();
            txtColoniaComercial = new TextBox();
            txtId = new TextBox();
            checkIsActive = new CheckBox();
            label5 = new Label();
            label9 = new Label();
            txtDomicilioComercial = new TextBox();
            txtCPComercial = new TextBox();
            label8 = new Label();
            label2 = new Label();
            label6 = new Label();
            txtComercialName = new TextBox();
            txtTelefonoComercial = new TextBox();
            txtCiudadComercial = new TextBox();
            label7 = new Label();
            tabFiscal = new TabPage();
            txtEmail = new TextBox();
            label4 = new Label();
            txtCPFiscal = new TextBox();
            txtTelefonoFiscal = new TextBox();
            label10 = new Label();
            txtDomicilioFiscal = new TextBox();
            txtRFC = new TextBox();
            label12 = new Label();
            label13 = new Label();
            label15 = new Label();
            txtCiudadFiscal = new TextBox();
            txtRazonSocial = new TextBox();
            label16 = new Label();
            label17 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            txtColonia = new TextBox();
            label11 = new Label();
            checkBox1 = new CheckBox();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabComercial.SuspendLayout();
            tabFiscal.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(tabControl1);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(829, 488);
            panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabComercial);
            tabControl1.Controls.Add(tabFiscal);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 35);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(827, 396);
            tabControl1.TabIndex = 37;
            // 
            // tabComercial
            // 
            tabComercial.Controls.Add(checkBox1);
            tabComercial.Controls.Add(label3);
            tabComercial.Controls.Add(txtColoniaComercial);
            tabComercial.Controls.Add(txtId);
            tabComercial.Controls.Add(checkIsActive);
            tabComercial.Controls.Add(label5);
            tabComercial.Controls.Add(label9);
            tabComercial.Controls.Add(txtDomicilioComercial);
            tabComercial.Controls.Add(txtCPComercial);
            tabComercial.Controls.Add(label8);
            tabComercial.Controls.Add(label2);
            tabComercial.Controls.Add(label6);
            tabComercial.Controls.Add(txtComercialName);
            tabComercial.Controls.Add(txtTelefonoComercial);
            tabComercial.Controls.Add(txtCiudadComercial);
            tabComercial.Controls.Add(label7);
            tabComercial.Location = new Point(4, 29);
            tabComercial.Margin = new Padding(3, 4, 3, 4);
            tabComercial.Name = "tabComercial";
            tabComercial.Padding = new Padding(3, 4, 3, 4);
            tabComercial.Size = new Size(819, 363);
            tabComercial.TabIndex = 0;
            tabComercial.Text = "Información Comercial";
            tabComercial.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 128);
            label3.Name = "label3";
            label3.Size = new Size(63, 20);
            label3.TabIndex = 36;
            label3.Text = "Colonia:";
            // 
            // txtColoniaComercial
            // 
            txtColoniaComercial.BorderStyle = BorderStyle.FixedSingle;
            txtColoniaComercial.Font = new Font("Segoe UI", 9.75F);
            txtColoniaComercial.Location = new Point(170, 120);
            txtColoniaComercial.Name = "txtColoniaComercial";
            txtColoniaComercial.Size = new Size(402, 29);
            txtColoniaComercial.TabIndex = 4;
            // 
            // txtId
            // 
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.Location = new Point(170, 7);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(109, 29);
            txtId.TabIndex = 1;
            // 
            // checkIsActive
            // 
            checkIsActive.AutoSize = true;
            checkIsActive.Location = new Point(170, 275);
            checkIsActive.Name = "checkIsActive";
            checkIsActive.Size = new Size(73, 24);
            checkIsActive.TabIndex = 8;
            checkIsActive.Text = "Activo";
            checkIsActive.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(9, 89);
            label5.Name = "label5";
            label5.Size = new Size(146, 20);
            label5.TabIndex = 27;
            label5.Text = "Domicilio comercial:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(9, 13);
            label9.Name = "label9";
            label9.Size = new Size(25, 20);
            label9.TabIndex = 19;
            label9.Text = "Id:";
            // 
            // txtDomicilioComercial
            // 
            txtDomicilioComercial.BorderStyle = BorderStyle.FixedSingle;
            txtDomicilioComercial.Font = new Font("Segoe UI", 9.75F);
            txtDomicilioComercial.Location = new Point(170, 81);
            txtDomicilioComercial.Name = "txtDomicilioComercial";
            txtDomicilioComercial.Size = new Size(402, 29);
            txtDomicilioComercial.TabIndex = 3;
            // 
            // txtCPComercial
            // 
            txtCPComercial.BorderStyle = BorderStyle.FixedSingle;
            txtCPComercial.Font = new Font("Segoe UI", 9.75F);
            txtCPComercial.Location = new Point(170, 194);
            txtCPComercial.Name = "txtCPComercial";
            txtCPComercial.Size = new Size(109, 29);
            txtCPComercial.TabIndex = 6;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(9, 197);
            label8.Name = "label8";
            label8.Size = new Size(29, 20);
            label8.TabIndex = 33;
            label8.Text = "CP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 49);
            label2.Name = "label2";
            label2.Size = new Size(136, 20);
            label2.TabIndex = 21;
            label2.Text = "Nombre comercial:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(9, 235);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 29;
            label6.Text = "Teléfono:";
            // 
            // txtComercialName
            // 
            txtComercialName.BorderStyle = BorderStyle.FixedSingle;
            txtComercialName.Font = new Font("Segoe UI", 9.75F);
            txtComercialName.Location = new Point(170, 43);
            txtComercialName.Name = "txtComercialName";
            txtComercialName.Size = new Size(527, 29);
            txtComercialName.TabIndex = 2;
            // 
            // txtTelefonoComercial
            // 
            txtTelefonoComercial.BorderStyle = BorderStyle.FixedSingle;
            txtTelefonoComercial.Font = new Font("Segoe UI", 9.75F);
            txtTelefonoComercial.Location = new Point(170, 229);
            txtTelefonoComercial.Name = "txtTelefonoComercial";
            txtTelefonoComercial.Size = new Size(240, 29);
            txtTelefonoComercial.TabIndex = 7;
            // 
            // txtCiudadComercial
            // 
            txtCiudadComercial.BorderStyle = BorderStyle.FixedSingle;
            txtCiudadComercial.Font = new Font("Segoe UI", 9.75F);
            txtCiudadComercial.Location = new Point(170, 155);
            txtCiudadComercial.Name = "txtCiudadComercial";
            txtCiudadComercial.Size = new Size(402, 29);
            txtCiudadComercial.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(9, 162);
            label7.Name = "label7";
            label7.Size = new Size(59, 20);
            label7.TabIndex = 31;
            label7.Text = "Ciudad:";
            // 
            // tabFiscal
            // 
            tabFiscal.Controls.Add(txtColonia);
            tabFiscal.Controls.Add(label11);
            tabFiscal.Controls.Add(txtEmail);
            tabFiscal.Controls.Add(label4);
            tabFiscal.Controls.Add(txtCPFiscal);
            tabFiscal.Controls.Add(txtTelefonoFiscal);
            tabFiscal.Controls.Add(label10);
            tabFiscal.Controls.Add(txtDomicilioFiscal);
            tabFiscal.Controls.Add(txtRFC);
            tabFiscal.Controls.Add(label12);
            tabFiscal.Controls.Add(label13);
            tabFiscal.Controls.Add(label15);
            tabFiscal.Controls.Add(txtCiudadFiscal);
            tabFiscal.Controls.Add(txtRazonSocial);
            tabFiscal.Controls.Add(label16);
            tabFiscal.Controls.Add(label17);
            tabFiscal.Location = new Point(4, 29);
            tabFiscal.Margin = new Padding(3, 4, 3, 4);
            tabFiscal.Name = "tabFiscal";
            tabFiscal.Padding = new Padding(3, 4, 3, 4);
            tabFiscal.Size = new Size(819, 363);
            tabFiscal.TabIndex = 1;
            tabFiscal.Text = "Información Fiscal";
            tabFiscal.UseVisualStyleBackColor = true;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9.75F);
            txtEmail.Location = new Point(185, 256);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(402, 29);
            txtEmail.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 259);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 55;
            label4.Text = "Email:";
            // 
            // txtCPFiscal
            // 
            txtCPFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtCPFiscal.Font = new Font("Segoe UI", 9.75F);
            txtCPFiscal.Location = new Point(185, 217);
            txtCPFiscal.Name = "txtCPFiscal";
            txtCPFiscal.Size = new Size(109, 29);
            txtCPFiscal.TabIndex = 15;
            // 
            // txtTelefonoFiscal
            // 
            txtTelefonoFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtTelefonoFiscal.Font = new Font("Segoe UI", 9.75F);
            txtTelefonoFiscal.Location = new Point(185, 295);
            txtTelefonoFiscal.Name = "txtTelefonoFiscal";
            txtTelefonoFiscal.Size = new Size(269, 29);
            txtTelefonoFiscal.TabIndex = 17;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(16, 107);
            label10.Name = "label10";
            label10.Size = new Size(115, 20);
            label10.TabIndex = 44;
            label10.Text = "Domicilio fiscal:";
            // 
            // txtDomicilioFiscal
            // 
            txtDomicilioFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtDomicilioFiscal.Font = new Font("Segoe UI", 9.75F);
            txtDomicilioFiscal.Location = new Point(185, 101);
            txtDomicilioFiscal.Name = "txtDomicilioFiscal";
            txtDomicilioFiscal.Size = new Size(527, 29);
            txtDomicilioFiscal.TabIndex = 12;
            // 
            // txtRFC
            // 
            txtRFC.BorderStyle = BorderStyle.FixedSingle;
            txtRFC.Font = new Font("Segoe UI", 9.75F);
            txtRFC.Location = new Point(185, 63);
            txtRFC.Name = "txtRFC";
            txtRFC.Size = new Size(269, 29);
            txtRFC.TabIndex = 11;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(17, 220);
            label12.Name = "label12";
            label12.Size = new Size(29, 20);
            label12.TabIndex = 50;
            label12.Text = "CP:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(16, 68);
            label13.Name = "label13";
            label13.Size = new Size(37, 20);
            label13.TabIndex = 42;
            label13.Text = "RFC:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(16, 300);
            label15.Name = "label15";
            label15.Size = new Size(70, 20);
            label15.TabIndex = 46;
            label15.Text = "Teléfono:";
            // 
            // txtCiudadFiscal
            // 
            txtCiudadFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtCiudadFiscal.Font = new Font("Segoe UI", 9.75F);
            txtCiudadFiscal.Location = new Point(185, 179);
            txtCiudadFiscal.Name = "txtCiudadFiscal";
            txtCiudadFiscal.Size = new Size(402, 29);
            txtCiudadFiscal.TabIndex = 14;
            // 
            // txtRazonSocial
            // 
            txtRazonSocial.BorderStyle = BorderStyle.FixedSingle;
            txtRazonSocial.Font = new Font("Segoe UI", 9.75F);
            txtRazonSocial.Location = new Point(185, 24);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(527, 29);
            txtRazonSocial.TabIndex = 10;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(16, 29);
            label16.Name = "label16";
            label16.Size = new Size(95, 20);
            label16.TabIndex = 40;
            label16.Text = "Razón social:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(17, 182);
            label17.Name = "label17";
            label17.Size = new Size(59, 20);
            label17.TabIndex = 48;
            label17.Text = "Ciudad:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 431);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5, 5, 5, 5);
            flowLayoutPanel1.Size = new Size(827, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(643, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 19;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(466, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(171, 35);
            btnSave.TabIndex = 18;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(827, 35);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(21, 5);
            label1.Name = "label1";
            label1.Size = new Size(104, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo cliente";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(790, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // txtColonia
            // 
            txtColonia.BorderStyle = BorderStyle.FixedSingle;
            txtColonia.Font = new Font("Segoe UI", 9.75F);
            txtColonia.Location = new Point(185, 141);
            txtColonia.Name = "txtColonia";
            txtColonia.Size = new Size(402, 29);
            txtColonia.TabIndex = 13;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(17, 144);
            label11.Name = "label11";
            label11.Size = new Size(63, 20);
            label11.TabIndex = 57;
            label11.Text = "Colonia:";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(170, 305);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(129, 24);
            checkBox1.TabIndex = 9;
            checkBox1.Text = "Proveedor VMI";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // FrmNuevoCliente
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(829, 488);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoCliente";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabComercial.ResumeLayout(false);
            tabComercial.PerformLayout();
            tabFiscal.ResumeLayout(false);
            tabFiscal.PerformLayout();
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
        private TextBox txtCPComercial;
        private Label label8;
        private TextBox txtCiudadComercial;
        private Label label7;
        private TextBox txtTelefonoComercial;
        private Label label6;
        private TextBox txtDomicilioComercial;
        private Label label5;
        private TextBox txtComercialName;
        private Label label2;
        private TextBox txtId;
        private Label label9;
        private TabControl tabControl1;
        private TabPage tabComercial;
        private TabPage tabFiscal;
        private Label label3;
        private TextBox txtColoniaComercial;
        private Label label10;
        private TextBox txtDomicilioFiscal;
        private TextBox txtRFC;
        private Label label12;
        private Label label13;
        private Label label15;
        private TextBox textBox6;
        private TextBox txtCiudadFiscal;
        private TextBox txtRazonSocial;
        private Label label16;
        private Label label17;
        private TextBox txtTelefonoFiscal;
        private TextBox txtCPFiscal;
        private TextBox txtEmail;
        private Label label4;
        private TextBox txtColonia;
        private Label label11;
        private CheckBox checkBox1;
    }
}