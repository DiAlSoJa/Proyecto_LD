namespace LD.Views.Dialogs
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

            checkIsActive = new CheckBox();
     
            label8 = new Label();
           
            label7 = new Label();
            label6 = new Label();
            txtDomicilioComercial = new TextBox();
            label5 = new Label();
            txtRFC = new TextBox();
            label4 = new Label();
            txtRazonSocial = new TextBox();
            label3 = new Label();
            txtComercialName = new TextBox();
            label2 = new Label();
            txtId = new TextBox();
            label9 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();

            txtTelefonoFiscal = new TextBox();
            txtCPFiscal = new TextBox();
            txtEmail = new TextBox();
            label4 = new Label();
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
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(653, 344);

            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 

            // checkIsActive
            // 
            checkIsActive.AutoSize = true;
            checkIsActive.Location = new Point(179, 350);
            checkIsActive.Name = "checkIsActive";
            checkIsActive.Size = new Size(73, 24);
            checkIsActive.TabIndex = 35;
            checkIsActive.Text = "Activo";
            checkIsActive.UseVisualStyleBackColor = true;

            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(18, 310);
            label8.Name = "label8";
            label8.Size = new Size(29, 20);
            label8.TabIndex = 33;
            label8.Text = "CP:";

            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 275);
            label7.Name = "label7";
            label7.Size = new Size(59, 20);
            label7.TabIndex = 31;
            label7.Text = "Ciudad:";

            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 240);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 29;
            label6.Text = "Teléfono:";
            // 
            // txtDomicilioComercial
            // 
            txtDomicilioComercial.BorderStyle = BorderStyle.FixedSingle;
            txtDomicilioComercial.Font = new Font("Segoe UI", 9.75F);

            txtDomicilioComercial.Location = new Point(149, 61);
            txtDomicilioComercial.Margin = new Padding(3, 2, 3, 2);
            txtDomicilioComercial.Name = "txtDomicilioComercial";
            txtDomicilioComercial.Size = new Size(352, 25);
            txtDomicilioComercial.TabIndex = 3;
            // 
            // txtCPComercial
            // 
            txtCPComercial.BorderStyle = BorderStyle.FixedSingle;
            txtCPComercial.Font = new Font("Segoe UI", 9.75F);
            txtCPComercial.Location = new Point(149, 177);
            txtCPComercial.Margin = new Padding(3, 2, 3, 2);
            txtCPComercial.Name = "txtCPComercial";
            txtCPComercial.Size = new Size(96, 25);
            txtCPComercial.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(8, 179);
            label8.Name = "label8";
            label8.Size = new Size(25, 15);
            label8.TabIndex = 33;
            label8.Text = "CP:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 37);
            label2.Name = "label2";
            label2.Size = new Size(109, 15);
            label2.TabIndex = 21;
            label2.Text = "Nombre comercial:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(8, 124);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 29;
            label6.Text = "Teléfono:";
            // 
            // txtComercialName
            // 
            txtComercialName.BorderStyle = BorderStyle.FixedSingle;
            txtComercialName.Font = new Font("Segoe UI", 9.75F);
            txtComercialName.Location = new Point(149, 32);
            txtComercialName.Margin = new Padding(3, 2, 3, 2);
            txtComercialName.Name = "txtComercialName";
            txtComercialName.Size = new Size(461, 25);
            txtComercialName.TabIndex = 2;
            // 
            // txtTelefonoComercial
            // 
            txtTelefonoComercial.BorderStyle = BorderStyle.FixedSingle;
            txtTelefonoComercial.Font = new Font("Segoe UI", 9.75F);
            txtTelefonoComercial.Location = new Point(149, 119);
            txtTelefonoComercial.Margin = new Padding(3, 2, 3, 2);
            txtTelefonoComercial.Name = "txtTelefonoComercial";
            txtTelefonoComercial.Size = new Size(210, 25);
            txtTelefonoComercial.TabIndex = 5;
            // 
            // txtCiudadComercial
            // 
            txtCiudadComercial.BorderStyle = BorderStyle.FixedSingle;
            txtCiudadComercial.Font = new Font("Segoe UI", 9.75F);
            txtCiudadComercial.Location = new Point(149, 148);
            txtCiudadComercial.Margin = new Padding(3, 2, 3, 2);
            txtCiudadComercial.Name = "txtCiudadComercial";
            txtCiudadComercial.Size = new Size(352, 25);
            txtCiudadComercial.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 153);
            label7.Name = "label7";
            label7.Size = new Size(48, 15);
            label7.TabIndex = 31;
            label7.Text = "Ciudad:";
            // 
            // tabFiscal
            // 
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
            tabFiscal.Location = new Point(4, 24);
            tabFiscal.Name = "tabFiscal";
            tabFiscal.Padding = new Padding(3);
            tabFiscal.Size = new Size(643, 247);
            tabFiscal.TabIndex = 1;
            tabFiscal.Text = "Información Fiscal";
            tabFiscal.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(14, 80);
            label10.Name = "label10";
            label10.Size = new Size(91, 15);
            label10.TabIndex = 44;
            label10.Text = "Domicilio fiscal:";
            // 
            // txtDomicilioFiscal
            // 
            txtDomicilioFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtDomicilioFiscal.Font = new Font("Segoe UI", 9.75F);
            txtDomicilioFiscal.Location = new Point(162, 76);
            txtDomicilioFiscal.Margin = new Padding(3, 2, 3, 2);
            txtDomicilioFiscal.Name = "txtDomicilioFiscal";
            txtDomicilioFiscal.Size = new Size(461, 25);
            txtDomicilioFiscal.TabIndex = 11;

            // 
            // txtRFC
            // 
            txtRFC.BorderStyle = BorderStyle.FixedSingle;
            txtRFC.Font = new Font("Segoe UI", 9.75F);

            txtRFC.Location = new Point(162, 47);
            txtRFC.Margin = new Padding(3, 2, 3, 2);
            txtRFC.Name = "txtRFC";
            txtRFC.Size = new Size(236, 25);
            txtRFC.TabIndex = 10;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(14, 167);
            label12.Name = "label12";
            label12.Size = new Size(25, 15);
            label12.TabIndex = 50;
            label12.Text = "CP:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(14, 51);
            label13.Name = "label13";
            label13.Size = new Size(31, 15);
            label13.TabIndex = 42;
            label13.Text = "RFC:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(14, 109);
            label15.Name = "label15";
            label15.Size = new Size(55, 15);
            label15.TabIndex = 46;
            label15.Text = "Teléfono:";
            // 
            // txtCiudadFiscal
            // 
            txtCiudadFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtCiudadFiscal.Font = new Font("Segoe UI", 9.75F);
            txtCiudadFiscal.Location = new Point(162, 134);
            txtCiudadFiscal.Margin = new Padding(3, 2, 3, 2);
            txtCiudadFiscal.Name = "txtCiudadFiscal";
            txtCiudadFiscal.Size = new Size(352, 25);
            txtCiudadFiscal.TabIndex = 13;

            // 
            // txtRazonSocial
            // 
            txtRazonSocial.BorderStyle = BorderStyle.FixedSingle;
            txtRazonSocial.Font = new Font("Segoe UI", 9.75F);

            txtRazonSocial.Location = new Point(162, 18);
            txtRazonSocial.Margin = new Padding(3, 2, 3, 2);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(461, 25);
            txtRazonSocial.TabIndex = 9;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(14, 22);
            label16.Name = "label16";
            label16.Size = new Size(75, 15);
            label16.TabIndex = 40;
            label16.Text = "Razón social:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(14, 138);
            label17.Name = "label17";
            label17.Size = new Size(48, 15);
            label17.TabIndex = 48;
            label17.Text = "Ciudad:";

            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;

            flowLayoutPanel1.Location = new Point(0, 301);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(4);
            flowLayoutPanel1.Size = new Size(651, 41);

            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = LD.Forms.Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;

            button2.Location = new Point(490, 6);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(150, 26);
            button2.TabIndex = 17;

            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = LD.Forms.Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(334, 6);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 26);
            btnSave.TabIndex = 16;

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
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(651, 26);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            panel2.MouseDown += panel1_MouseDown;
            panel2.MouseMove += panel1_MouseMove;
            panel2.MouseUp += panel1_MouseUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(18, 4);
            label1.Name = "label1";
            label1.Size = new Size(79, 16);

            label1.TabIndex = 3;
            label1.Text = "Nuevo cliente";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;

            pictureBox2.Image = Forms.Properties.Resources.cancelar;
            pictureBox2.Location = new Point(619, 0);


            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(4, 8, 0, 0);
            pictureBox2.Size = new Size(32, 26);

            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // txtTelefonoFiscal
            // 
            txtTelefonoFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtTelefonoFiscal.Font = new Font("Segoe UI", 9.75F);
            txtTelefonoFiscal.Location = new Point(162, 105);
            txtTelefonoFiscal.Margin = new Padding(3, 2, 3, 2);
            txtTelefonoFiscal.Name = "txtTelefonoFiscal";
            txtTelefonoFiscal.Size = new Size(236, 25);
            txtTelefonoFiscal.TabIndex = 12;
            // 
            // txtCPFiscal
            // 
            txtCPFiscal.BorderStyle = BorderStyle.FixedSingle;
            txtCPFiscal.Font = new Font("Segoe UI", 9.75F);
            txtCPFiscal.Location = new Point(162, 163);
            txtCPFiscal.Margin = new Padding(3, 2, 3, 2);
            txtCPFiscal.Name = "txtCPFiscal";
            txtCPFiscal.Size = new Size(236, 25);
            txtCPFiscal.TabIndex = 14;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Font = new Font("Segoe UI", 9.75F);
            txtEmail.Location = new Point(162, 192);
            txtEmail.Margin = new Padding(3, 2, 3, 2);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(352, 25);
            txtEmail.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 196);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 55;
            label4.Text = "Email:";
            // 
            // FrmNuevoCliente
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(653, 344);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);

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
    }
}