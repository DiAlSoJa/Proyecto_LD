namespace LD.Forms.Views.Dialogs
{
    partial class FrmNewDimensioner
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
            txtPeso = new LD.Controls.TextBoxControl();
            txtAncho = new LD.Controls.TextBoxControl();
            txtLargo = new LD.Controls.TextBoxControl();
            txtAlto = new LD.Controls.TextBoxControl();
            label27 = new Label();
            label28 = new Label();
            label29 = new Label();
            label30 = new Label();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(txtPeso);
            panel1.Controls.Add(txtAncho);
            panel1.Controls.Add(txtLargo);
            panel1.Controls.Add(txtAlto);
            panel1.Controls.Add(label27);
            panel1.Controls.Add(label28);
            panel1.Controls.Add(label29);
            panel1.Controls.Add(label30);
            panel1.Controls.Add(txtNombre);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(txtId);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(675, 381);
            panel1.TabIndex = 1;
            // 
            // txtNombre
            // 
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Font = new Font("Segoe UI", 9.75F);
            txtNombre.Location = new Point(143, 101);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(456, 29);
            txtNombre.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 324);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(673, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(489, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 8;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(312, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(171, 35);
            btnSave.TabIndex = 7;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtId
            // 
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Font = new Font("Segoe UI", 9.75F);
            txtId.Location = new Point(143, 60);
            txtId.Name = "txtId";
            txtId.Size = new Size(68, 29);
            txtId.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 115);
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
            panel2.Size = new Size(673, 35);
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
            label1.Size = new Size(83, 22);
            label1.TabIndex = 3;
            label1.Text = "Dimensión";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(636, 0);
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
            label2.Location = new Point(23, 68);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 66;
            label2.Text = "Dimensión";
            // 
            // txtPeso
            // 
            txtPeso.BackColor = Color.White;
            txtPeso.BorderColor = SystemColors.ControlDark;
            txtPeso.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPeso.IsNumber = true;
            txtPeso.Location = new Point(143, 251);
            txtPeso.Name = "txtPeso";
            txtPeso.Padding = new Padding(7);
            txtPeso.Size = new Size(109, 33);
            txtPeso.TabIndex = 6;
            // 
            // txtAncho
            // 
            txtAncho.BackColor = Color.White;
            txtAncho.BorderColor = SystemColors.ControlDark;
            txtAncho.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAncho.IsNumber = true;
            txtAncho.Location = new Point(143, 215);
            txtAncho.Name = "txtAncho";
            txtAncho.Padding = new Padding(7);
            txtAncho.Size = new Size(109, 33);
            txtAncho.TabIndex = 5;
            // 
            // txtLargo
            // 
            txtLargo.BackColor = Color.White;
            txtLargo.BorderColor = SystemColors.ControlDark;
            txtLargo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLargo.IsNumber = true;
            txtLargo.Location = new Point(143, 180);
            txtLargo.Name = "txtLargo";
            txtLargo.Padding = new Padding(7);
            txtLargo.Size = new Size(109, 33);
            txtLargo.TabIndex = 4;
            // 
            // txtAlto
            // 
            txtAlto.BackColor = Color.White;
            txtAlto.BorderColor = SystemColors.ControlDark;
            txtAlto.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAlto.IsNumber = true;
            txtAlto.Location = new Point(143, 146);
            txtAlto.Name = "txtAlto";
            txtAlto.Padding = new Padding(7);
            txtAlto.Size = new Size(109, 33);
            txtAlto.TabIndex = 3;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(65, 264);
            label27.Name = "label27";
            label27.Size = new Size(42, 20);
            label27.TabIndex = 75;
            label27.Text = "Peso:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(53, 222);
            label28.Name = "label28";
            label28.Size = new Size(54, 20);
            label28.TabIndex = 74;
            label28.Text = "Ancho:";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(61, 184);
            label29.Name = "label29";
            label29.Size = new Size(50, 20);
            label29.TabIndex = 73;
            label29.Text = "Largo:";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(68, 150);
            label30.Name = "label30";
            label30.Size = new Size(40, 20);
            label30.TabIndex = 72;
            label30.Text = "Alto:";
            // 
            // FrmNewDimensioner
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(675, 381);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNewDimensioner";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Monedas";
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
        private LD.Controls.TextBoxControl txtPeso;
        private LD.Controls.TextBoxControl txtAncho;
        private LD.Controls.TextBoxControl txtLargo;
        private LD.Controls.TextBoxControl txtAlto;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label30;
    }
}