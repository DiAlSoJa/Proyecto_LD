namespace LD.Dialogs
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
            txtCodigoPostal = new TextBox();
            label8 = new Label();
            txtCiudad = new TextBox();
            label7 = new Label();
            txtTelefono = new TextBox();
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
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(checkIsActive);
            panel1.Controls.Add(txtCodigoPostal);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(txtCiudad);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(txtTelefono);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtDomicilioComercial);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(txtRFC);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(txtRazonSocial);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(txtComercialName);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(txtId);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(756, 454);
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
            // txtCodigoPostal
            // 
            txtCodigoPostal.BorderStyle = BorderStyle.FixedSingle;
            txtCodigoPostal.Font = new Font("Segoe UI", 9.75F);
            txtCodigoPostal.Location = new Point(179, 303);
            txtCodigoPostal.Name = "txtCodigoPostal";
            txtCodigoPostal.Size = new Size(109, 29);
            txtCodigoPostal.TabIndex = 34;
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
            // txtCiudad
            // 
            txtCiudad.BorderStyle = BorderStyle.FixedSingle;
            txtCiudad.Font = new Font("Segoe UI", 9.75F);
            txtCiudad.Location = new Point(179, 268);
            txtCiudad.Name = "txtCiudad";
            txtCiudad.Size = new Size(402, 29);
            txtCiudad.TabIndex = 32;
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
            // txtTelefono
            // 
            txtTelefono.BorderStyle = BorderStyle.FixedSingle;
            txtTelefono.Font = new Font("Segoe UI", 9.75F);
            txtTelefono.Location = new Point(179, 233);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(402, 29);
            txtTelefono.TabIndex = 30;
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
            txtDomicilioComercial.Location = new Point(179, 198);
            txtDomicilioComercial.Name = "txtDomicilioComercial";
            txtDomicilioComercial.Size = new Size(402, 29);
            txtDomicilioComercial.TabIndex = 28;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 205);
            label5.Name = "label5";
            label5.Size = new Size(146, 20);
            label5.TabIndex = 27;
            label5.Text = "Domicilio comercial:";
            // 
            // txtRFC
            // 
            txtRFC.BorderStyle = BorderStyle.FixedSingle;
            txtRFC.Font = new Font("Segoe UI", 9.75F);
            txtRFC.Location = new Point(179, 163);
            txtRFC.Name = "txtRFC";
            txtRFC.Size = new Size(270, 29);
            txtRFC.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(18, 170);
            label4.Name = "label4";
            label4.Size = new Size(37, 20);
            label4.TabIndex = 25;
            label4.Text = "RFC:";
            // 
            // txtRazonSocial
            // 
            txtRazonSocial.BorderStyle = BorderStyle.FixedSingle;
            txtRazonSocial.Font = new Font("Segoe UI", 9.75F);
            txtRazonSocial.Location = new Point(179, 128);
            txtRazonSocial.Name = "txtRazonSocial";
            txtRazonSocial.Size = new Size(527, 29);
            txtRazonSocial.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 135);
            label3.Name = "label3";
            label3.Size = new Size(95, 20);
            label3.TabIndex = 23;
            label3.Text = "Razón social:";
            // 
            // txtComercialName
            // 
            txtComercialName.BorderStyle = BorderStyle.FixedSingle;
            txtComercialName.Font = new Font("Segoe UI", 9.75F);
            txtComercialName.Location = new Point(179, 93);
            txtComercialName.Name = "txtComercialName";
            txtComercialName.Size = new Size(527, 29);
            txtComercialName.TabIndex = 22;
            txtComercialName.Text = "JABIL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 100);
            label2.Name = "label2";
            label2.Size = new Size(136, 20);
            label2.TabIndex = 21;
            label2.Text = "Nombre comercial:";
            // 
            // txtId
            // 
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.Location = new Point(179, 57);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(109, 29);
            txtId.TabIndex = 20;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(18, 64);
            label9.Name = "label9";
            label9.Size = new Size(25, 20);
            label9.TabIndex = 19;
            label9.Text = "Id:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 397);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(754, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Forms.Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(569, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Forms.Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(391, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(172, 35);
            btnSave.TabIndex = 6;
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
            panel2.Size = new Size(754, 35);
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
            label1.Location = new Point(20, 5);
            label1.Name = "label1";
            label1.Size = new Size(104, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo cliente";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Forms.Properties.Resources.cancelar;
            pictureBox2.Location = new Point(718, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevoCliente
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(756, 454);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoCliente";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
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
        private CheckBox checkIsActive;
        private TextBox txtCodigoPostal;
        private Label label8;
        private TextBox txtCiudad;
        private Label label7;
        private TextBox txtTelefono;
        private Label label6;
        private TextBox txtDomicilioComercial;
        private Label label5;
        private TextBox txtRFC;
        private Label label4;
        private TextBox txtRazonSocial;
        private Label label3;
        private TextBox txtComercialName;
        private Label label2;
        private TextBox txtId;
        private Label label9;
    }
}