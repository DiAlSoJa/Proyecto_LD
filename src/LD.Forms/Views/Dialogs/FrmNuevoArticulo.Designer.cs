namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevoArticulo
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            comboBox5 = new ComboBox();
            label4 = new Label();
            comboBox4 = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            groupBox1 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            comboBox3 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            checkBox5 = new CheckBox();
            checkBox4 = new CheckBox();
            textBox7 = new TextBox();
            label7 = new Label();
            textBox6 = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label8 = new Label();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(comboBox5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(comboBox4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(comboBox3);
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(checkBox5);
            panel1.Controls.Add(checkBox4);
            panel1.Controls.Add(textBox7);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(textBox6);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(692, 539);
            panel1.TabIndex = 0;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 482);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(690, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image =Forms.Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(505, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image =Forms.Properties.Resources.save;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(327, 8);
            button1.Name = "button1";
            button1.Size = new Size(172, 35);
            button1.TabIndex = 6;
            button1.Text = "Guardar";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(690, 35);
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
            label1.Size = new Size(119, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo artcículo";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image =Forms.Properties.Resources.cancelar;
            pictureBox2.Location = new Point(654, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // comboBox5
            // 
            comboBox5.Font = new Font("Segoe UI", 9.75F);
            comboBox5.FormattingEnabled = true;
            comboBox5.Location = new Point(516, 295);
            comboBox5.Name = "comboBox5";
            comboBox5.Size = new Size(141, 29);
            comboBox5.TabIndex = 50;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(346, 299);
            label4.Name = "label4";
            label4.Size = new Size(117, 20);
            label4.TabIndex = 49;
            label4.Text = "Unidad máxima:";
            // 
            // comboBox4
            // 
            comboBox4.Font = new Font("Segoe UI", 9.75F);
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(516, 260);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(141, 29);
            comboBox4.TabIndex = 48;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(346, 266);
            label3.Name = "label3";
            label3.Size = new Size(106, 20);
            label3.TabIndex = 47;
            label3.Text = "Unidad media:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(346, 234);
            label2.Name = "label2";
            label2.Size = new Size(114, 20);
            label2.TabIndex = 46;
            label2.Text = "Unidad mínima:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton4);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new Point(40, 222);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(233, 160);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(22, 116);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(162, 24);
            radioButton4.TabIndex = 3;
            radioButton4.TabStop = true;
            radioButton4.Text = "Fecha de caducidad";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(22, 86);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(135, 24);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.Text = "Número de lote";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(22, 56);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(174, 24);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "LIFO - Last In First Out";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(22, 26);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(175, 24);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "FIFO - First In First Out";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            comboBox3.Font = new Font("Segoe UI", 9.75F);
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(516, 225);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(141, 29);
            comboBox3.TabIndex = 44;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 9.75F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(194, 95);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(465, 29);
            comboBox2.TabIndex = 43;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 9.75F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(194, 60);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(465, 29);
            comboBox1.TabIndex = 42;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(40, 431);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(219, 24);
            checkBox5.TabIndex = 41;
            checkBox5.Text = "Solicitar fecha de caducidad";
            checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(40, 401);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(191, 24);
            checkBox4.TabIndex = 40;
            checkBox4.Text = "Solicitar número de lote";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // textBox7
            // 
            textBox7.BorderStyle = BorderStyle.FixedSingle;
            textBox7.Font = new Font("Segoe UI", 9.75F);
            textBox7.Location = new Point(194, 167);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(463, 29);
            textBox7.TabIndex = 39;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(33, 174);
            label7.Name = "label7";
            label7.Size = new Size(90, 20);
            label7.TabIndex = 38;
            label7.Text = "Descripción:";
            // 
            // textBox6
            // 
            textBox6.BorderStyle = BorderStyle.FixedSingle;
            textBox6.Font = new Font("Segoe UI", 9.75F);
            textBox6.Location = new Point(194, 132);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(405, 29);
            textBox6.TabIndex = 37;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(33, 139);
            label6.Name = "label6";
            label6.Size = new Size(72, 20);
            label6.TabIndex = 36;
            label6.Text = "No. Parte:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 104);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
            label5.TabIndex = 35;
            label5.Text = "Proyecto:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(33, 68);
            label8.Name = "label8";
            label8.Size = new Size(58, 20);
            label8.TabIndex = 34;
            label8.Text = "Cliente:";
            // 
            // FrmNuevoArticulo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(692, 539);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoArticulo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private PictureBox pictureBox2;
        private Button button2;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private ComboBox comboBox5;
        private Label label4;
        private ComboBox comboBox4;
        private Label label3;
        private Label label2;
        private GroupBox groupBox1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private ComboBox comboBox3;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private TextBox textBox7;
        private Label label7;
        private TextBox textBox6;
        private Label label6;
        private Label label5;
        private Label label8;
    }
}