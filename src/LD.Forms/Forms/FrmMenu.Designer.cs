namespace LD.Forms
{
    partial class FrmMenu
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
            searchWindow = new TextBox();
            lblClose = new Label();
            pictureBox1 = new PictureBox();
            flwMenu = new FlowLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button15 = new Button();
            button9 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button16 = new Button();
            button13 = new Button();
            button14 = new Button();
            button17 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flwMenu.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(searchWindow);
            panel1.Controls.Add(lblClose);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1285, 131);
            panel1.TabIndex = 1;
            // 
            // searchWindow
            // 
            searchWindow.BorderStyle = BorderStyle.None;
            searchWindow.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchWindow.Location = new Point(363, 27);
            searchWindow.Margin = new Padding(3, 2, 3, 2);
            searchWindow.Name = "searchWindow";
            searchWindow.PlaceholderText = "Escriba para buscar";
            searchWindow.Size = new Size(534, 22);
            searchWindow.TabIndex = 0;
            // 
            // lblClose
            // 
            lblClose.AutoSize = true;
            lblClose.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblClose.ForeColor = SystemColors.ControlDarkDark;
            lblClose.Location = new Point(928, 29);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(17, 19);
            lblClose.TabIndex = 3;
            lblClose.Text = "X";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.searchBar;
            pictureBox1.Location = new Point(317, 15);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(695, 53);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // flwMenu
            // 
            flwMenu.BackColor = Color.White;
            flwMenu.Controls.Add(button1);
            flwMenu.Controls.Add(button2);
            flwMenu.Controls.Add(button3);
            flwMenu.Controls.Add(button4);
            flwMenu.Controls.Add(button5);
            flwMenu.Controls.Add(button15);
            flwMenu.Controls.Add(button9);
            flwMenu.Controls.Add(button6);
            flwMenu.Controls.Add(button7);
            flwMenu.Controls.Add(button8);
            flwMenu.Controls.Add(button10);
            flwMenu.Controls.Add(button11);
            flwMenu.Controls.Add(button12);
            flwMenu.Controls.Add(button16);
            flwMenu.Controls.Add(button13);
            flwMenu.Controls.Add(button14);
            flwMenu.Controls.Add(button17);
            flwMenu.Dock = DockStyle.Fill;
            flwMenu.Location = new Point(0, 131);
            flwMenu.Margin = new Padding(3, 2, 3, 2);
            flwMenu.Name = "flwMenu";
            flwMenu.Padding = new Padding(9, 8, 9, 8);
            flwMenu.Size = new Size(1285, 470);
            flwMenu.TabIndex = 1;
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.Image = Properties.Resources.clientes;
            button1.Location = new Point(12, 10);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(153, 60);
            button1.TabIndex = 1;
            button1.Text = "Clientes";
            button1.TextImageRelation = TextImageRelation.ImageAboveText;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Cursor = Cursors.Hand;
            button2.Image = Properties.Resources.proyectos;
            button2.Location = new Point(171, 10);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(153, 60);
            button2.TabIndex = 2;
            button2.Text = "Proyectos";
            button2.TextImageRelation = TextImageRelation.ImageAboveText;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Cursor = Cursors.Hand;
            button3.Image = Properties.Resources.almacen;
            button3.Location = new Point(330, 10);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(153, 60);
            button3.TabIndex = 3;
            button3.Text = "Almacenes";
            button3.TextImageRelation = TextImageRelation.ImageAboveText;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Cursor = Cursors.Hand;
            button4.Image = Properties.Resources.ubicaciones;
            button4.Location = new Point(489, 10);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(153, 60);
            button4.TabIndex = 4;
            button4.Text = "Ubicaciones";
            button4.TextImageRelation = TextImageRelation.ImageAboveText;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Cursor = Cursors.Hand;
            button5.Image = Properties.Resources.articulos;
            button5.Location = new Point(648, 10);
            button5.Margin = new Padding(3, 2, 3, 2);
            button5.Name = "button5";
            button5.Size = new Size(153, 60);
            button5.TabIndex = 5;
            button5.Text = "Artículos";
            button5.TextImageRelation = TextImageRelation.ImageAboveText;
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button15
            // 
            button15.Cursor = Cursors.Hand;
            button15.Image = Properties.Resources.transaccion;
            button15.Location = new Point(807, 10);
            button15.Margin = new Padding(3, 2, 3, 2);
            button15.Name = "button15";
            button15.Size = new Size(153, 60);
            button15.TabIndex = 15;
            button15.Text = "Movimientos";
            button15.TextImageRelation = TextImageRelation.ImageAboveText;
            button15.UseVisualStyleBackColor = true;
            button15.Click += button15_Click;
            // 
            // button9
            // 
            button9.Cursor = Cursors.Hand;
            button9.Image = Properties.Resources.entrada;
            button9.Location = new Point(966, 10);
            button9.Margin = new Padding(3, 2, 3, 2);
            button9.Name = "button9";
            button9.Size = new Size(153, 60);
            button9.TabIndex = 9;
            button9.Text = "Recepción de Material";
            button9.TextImageRelation = TextImageRelation.ImageAboveText;
            button9.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Cursor = Cursors.Hand;
            button6.Image = Properties.Resources.controlPatio;
            button6.Location = new Point(12, 74);
            button6.Margin = new Padding(3, 2, 3, 2);
            button6.Name = "button6";
            button6.Size = new Size(153, 60);
            button6.TabIndex = 6;
            button6.Text = "Control de Patio";
            button6.TextImageRelation = TextImageRelation.ImageAboveText;
            button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Cursor = Cursors.Hand;
            button7.Image = Properties.Resources.asn;
            button7.Location = new Point(171, 74);
            button7.Margin = new Padding(3, 2, 3, 2);
            button7.Name = "button7";
            button7.Size = new Size(153, 60);
            button7.TabIndex = 7;
            button7.Text = "ASN";
            button7.TextImageRelation = TextImageRelation.ImageAboveText;
            button7.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Cursor = Cursors.Hand;
            button8.Image = Properties.Resources.validarRecepcion;
            button8.Location = new Point(330, 74);
            button8.Margin = new Padding(3, 2, 3, 2);
            button8.Name = "button8";
            button8.Size = new Size(153, 60);
            button8.TabIndex = 8;
            button8.Text = "Validación de Recepción";
            button8.TextImageRelation = TextImageRelation.ImageAboveText;
            button8.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Cursor = Cursors.Hand;
            button10.Image = Properties.Resources.surtido;
            button10.Location = new Point(489, 74);
            button10.Margin = new Padding(3, 2, 3, 2);
            button10.Name = "button10";
            button10.Size = new Size(153, 60);
            button10.TabIndex = 10;
            button10.Text = "Surtido";
            button10.TextImageRelation = TextImageRelation.ImageAboveText;
            button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Cursor = Cursors.Hand;
            button11.Image = Properties.Resources.auditar;
            button11.Location = new Point(648, 74);
            button11.Margin = new Padding(3, 2, 3, 2);
            button11.Name = "button11";
            button11.Size = new Size(153, 60);
            button11.TabIndex = 11;
            button11.Text = "Auditar";
            button11.TextImageRelation = TextImageRelation.ImageAboveText;
            button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Cursor = Cursors.Hand;
            button12.Image = Properties.Resources.salida1;
            button12.Location = new Point(807, 74);
            button12.Margin = new Padding(3, 2, 3, 2);
            button12.Name = "button12";
            button12.Size = new Size(153, 60);
            button12.TabIndex = 12;
            button12.Text = "Embarques";
            button12.TextImageRelation = TextImageRelation.ImageAboveText;
            button12.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            button16.Cursor = Cursors.Hand;
            button16.Image = Properties.Resources.inventario;
            button16.Location = new Point(966, 74);
            button16.Margin = new Padding(3, 2, 3, 2);
            button16.Name = "button16";
            button16.Size = new Size(153, 60);
            button16.TabIndex = 16;
            button16.Text = "Inventario";
            button16.TextImageRelation = TextImageRelation.ImageAboveText;
            button16.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            button13.Cursor = Cursors.Hand;
            button13.Image = Properties.Resources.aleatorio;
            button13.Location = new Point(12, 138);
            button13.Margin = new Padding(3, 2, 3, 2);
            button13.Name = "button13";
            button13.Size = new Size(153, 60);
            button13.TabIndex = 13;
            button13.Text = "Inventario Aleatorio";
            button13.TextImageRelation = TextImageRelation.ImageAboveText;
            button13.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            button14.Cursor = Cursors.Hand;
            button14.Image = Properties.Resources.grafica1;
            button14.Location = new Point(171, 138);
            button14.Margin = new Padding(3, 2, 3, 2);
            button14.Name = "button14";
            button14.Size = new Size(153, 60);
            button14.TabIndex = 14;
            button14.Text = "Reportes";
            button14.TextImageRelation = TextImageRelation.ImageAboveText;
            button14.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            button17.Cursor = Cursors.Hand;
            button17.Image = Properties.Resources.acceso;
            button17.Location = new Point(330, 138);
            button17.Margin = new Padding(3, 2, 3, 2);
            button17.Name = "button17";
            button17.Size = new Size(153, 60);
            button17.TabIndex = 17;
            button17.Text = "Usuarios";
            button17.TextImageRelation = TextImageRelation.ImageAboveText;
            button17.UseVisualStyleBackColor = true;
            // 
            // FrmMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1285, 601);
            Controls.Add(flwMenu);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmMenu";
            Text = "FrmMenu";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flwMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private FlowLayoutPanel flwMenu;
        private Button button1;
        private PictureBox pictureBox1;
        private Button button2;
        private Label lblClose;
        private TextBox searchWindow;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button16;
        private Button button17;
    }
}