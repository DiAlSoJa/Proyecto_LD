namespace LD.Forms.Views.Forms
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
            clientBtn = new Button();
            projectBtn = new Button();
            warehouseBtn = new Button();
            locationBtn = new Button();
            productBtn = new Button();
            movementBtn = new Button();
            asnBtn = new Button();
            auditBtn = new Button();
            inventoryBtn = new Button();
            randomInventoryBtn = new Button();
            userBtn = new Button();
            checkListBtn = new Button();
            button8 = new Button();
            button10 = new Button();
            shipmentBtn = new Button();
            reportBtn = new Button();
            yardControlBtn = new Button();
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
            panel1.Name = "panel1";
            panel1.Size = new Size(1469, 175);
            panel1.TabIndex = 1;
            // 
            // searchWindow
            // 
            searchWindow.BorderStyle = BorderStyle.None;
            searchWindow.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            searchWindow.Location = new Point(431, 36);
            searchWindow.Name = "searchWindow";
            searchWindow.PlaceholderText = "Escriba para buscar";
            searchWindow.Size = new Size(610, 27);
            searchWindow.TabIndex = 0;
            // 
            // lblClose
            // 
            lblClose.AutoSize = true;
            lblClose.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblClose.ForeColor = SystemColors.ControlDarkDark;
            lblClose.Location = new Point(1061, 41);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(20, 23);
            lblClose.TabIndex = 3;
            lblClose.Text = "X";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.searchBar;
            pictureBox1.Location = new Point(362, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(767, 62);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // flwMenu
            // 
            flwMenu.BackColor = Color.White;
            flwMenu.Controls.Add(clientBtn);
            flwMenu.Controls.Add(warehouseBtn);
            flwMenu.Controls.Add(locationBtn);
            flwMenu.Controls.Add(productBtn);
            flwMenu.Controls.Add(movementBtn);
            flwMenu.Controls.Add(asnBtn);
            flwMenu.Controls.Add(auditBtn);
            flwMenu.Controls.Add(inventoryBtn);
            flwMenu.Controls.Add(projectBtn);
            flwMenu.Controls.Add(randomInventoryBtn);
            flwMenu.Controls.Add(userBtn);
            flwMenu.Controls.Add(checkListBtn);
            flwMenu.Controls.Add(button8);
            flwMenu.Controls.Add(button10);
            flwMenu.Controls.Add(shipmentBtn);
            flwMenu.Controls.Add(reportBtn);
            flwMenu.Controls.Add(yardControlBtn);
            flwMenu.Dock = DockStyle.Fill;
            flwMenu.Location = new Point(0, 175);
            flwMenu.Name = "flwMenu";
            flwMenu.Padding = new Padding(10, 11, 10, 11);
            flwMenu.Size = new Size(1469, 626);
            flwMenu.TabIndex = 1;
            // 
            // clientBtn
            // 
            clientBtn.Cursor = Cursors.Hand;
            clientBtn.Image = Properties.Resources.clientes;
            clientBtn.Location = new Point(13, 14);
            clientBtn.Name = "clientBtn";
            clientBtn.Size = new Size(175, 80);
            clientBtn.TabIndex = 1;
            clientBtn.Text = "Clientes";
            clientBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            clientBtn.UseVisualStyleBackColor = true;
            clientBtn.Click += button1_Click;
            // 
            // projectBtn
            // 
            projectBtn.Cursor = Cursors.Hand;
            projectBtn.Image = Properties.Resources.proyectos;
            projectBtn.Location = new Point(13, 100);
            projectBtn.Name = "projectBtn";
            projectBtn.Size = new Size(175, 80);
            projectBtn.TabIndex = 2;
            projectBtn.Text = "Proyectos";
            projectBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            projectBtn.UseVisualStyleBackColor = true;
            projectBtn.Click += button2_Click;
            // 
            // warehouseBtn
            // 
            warehouseBtn.Cursor = Cursors.Hand;
            warehouseBtn.Image = Properties.Resources.almacen;
            warehouseBtn.Location = new Point(194, 14);
            warehouseBtn.Name = "warehouseBtn";
            warehouseBtn.Size = new Size(175, 80);
            warehouseBtn.TabIndex = 3;
            warehouseBtn.Text = "Almacenes";
            warehouseBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            warehouseBtn.UseVisualStyleBackColor = true;
            warehouseBtn.Click += button3_Click;
            // 
            // locationBtn
            // 
            locationBtn.Cursor = Cursors.Hand;
            locationBtn.Image = Properties.Resources.ubicaciones;
            locationBtn.Location = new Point(375, 14);
            locationBtn.Name = "locationBtn";
            locationBtn.Size = new Size(175, 80);
            locationBtn.TabIndex = 4;
            locationBtn.Text = "Ubicaciones";
            locationBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            locationBtn.UseVisualStyleBackColor = true;
            locationBtn.Click += button4_Click;
            // 
            // productBtn
            // 
            productBtn.Cursor = Cursors.Hand;
            productBtn.Image = Properties.Resources.articulos;
            productBtn.Location = new Point(556, 14);
            productBtn.Name = "productBtn";
            productBtn.Size = new Size(175, 80);
            productBtn.TabIndex = 5;
            productBtn.Text = "Artículos";
            productBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            productBtn.UseVisualStyleBackColor = true;
            productBtn.Click += button5_Click;
            // 
            // movementBtn
            // 
            movementBtn.Cursor = Cursors.Hand;
            movementBtn.Image = Properties.Resources.transaccion;
            movementBtn.Location = new Point(737, 14);
            movementBtn.Name = "movementBtn";
            movementBtn.Size = new Size(175, 80);
            movementBtn.TabIndex = 15;
            movementBtn.Text = "Movimientos";
            movementBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            movementBtn.UseVisualStyleBackColor = true;
            movementBtn.Click += button15_Click;
            // 
            // asnBtn
            // 
            asnBtn.Cursor = Cursors.Hand;
            asnBtn.Image = Properties.Resources.asn;
            asnBtn.Location = new Point(918, 14);
            asnBtn.Name = "asnBtn";
            asnBtn.Size = new Size(175, 80);
            asnBtn.TabIndex = 7;
            asnBtn.Text = "ASN";
            asnBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            asnBtn.UseVisualStyleBackColor = true;
            asnBtn.Click += button7_Click;
            // 
            // auditBtn
            // 
            auditBtn.Cursor = Cursors.Hand;
            auditBtn.Image = Properties.Resources.auditar;
            auditBtn.Location = new Point(1099, 14);
            auditBtn.Name = "auditBtn";
            auditBtn.Size = new Size(175, 80);
            auditBtn.TabIndex = 11;
            auditBtn.Text = "Auditar";
            auditBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            auditBtn.UseVisualStyleBackColor = true;
            auditBtn.Click += button11_Click;
            // 
            // inventoryBtn
            // 
            inventoryBtn.Cursor = Cursors.Hand;
            inventoryBtn.Image = Properties.Resources.inventario;
            inventoryBtn.Location = new Point(1280, 14);
            inventoryBtn.Name = "inventoryBtn";
            inventoryBtn.Size = new Size(175, 80);
            inventoryBtn.TabIndex = 16;
            inventoryBtn.Text = "Inventario";
            inventoryBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            inventoryBtn.UseVisualStyleBackColor = true;
            inventoryBtn.Click += button16_Click;
            // 
            // randomInventoryBtn
            // 
            randomInventoryBtn.Cursor = Cursors.Hand;
            randomInventoryBtn.Image = Properties.Resources.aleatorio;
            randomInventoryBtn.Location = new Point(194, 100);
            randomInventoryBtn.Name = "randomInventoryBtn";
            randomInventoryBtn.Size = new Size(175, 80);
            randomInventoryBtn.TabIndex = 13;
            randomInventoryBtn.Text = "Inventario Aleatorio";
            randomInventoryBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            randomInventoryBtn.UseVisualStyleBackColor = true;
            randomInventoryBtn.Click += button13_Click;
            // 
            // userBtn
            // 
            userBtn.Cursor = Cursors.Hand;
            userBtn.Image = Properties.Resources.acceso;
            userBtn.Location = new Point(375, 100);
            userBtn.Name = "userBtn";
            userBtn.Size = new Size(175, 80);
            userBtn.TabIndex = 17;
            userBtn.Text = "Usuarios";
            userBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            userBtn.UseVisualStyleBackColor = true;
            userBtn.Click += button17_Click;
            // 
            // checkListBtn
            // 
            checkListBtn.Cursor = Cursors.Hand;
            checkListBtn.Image = Properties.Resources.montacargas;
            checkListBtn.Location = new Point(556, 100);
            checkListBtn.Name = "checkListBtn";
            checkListBtn.Size = new Size(175, 80);
            checkListBtn.TabIndex = 6;
            checkListBtn.Text = "CheckList Montacargas";
            checkListBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            checkListBtn.UseVisualStyleBackColor = true;
            checkListBtn.Click += button6_Click;
            // 
            // button8
            // 
            button8.Cursor = Cursors.Hand;
            button8.Image = Properties.Resources.catalogos;
            button8.Location = new Point(737, 100);
            button8.Name = "button8";
            button8.Size = new Size(175, 80);
            button8.TabIndex = 20;
            button8.Text = "Catálogos";
            button8.TextImageRelation = TextImageRelation.ImageAboveText;
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button10
            // 
            button10.Cursor = Cursors.Hand;
            button10.Image = Properties.Resources.surtido;
            button10.Location = new Point(918, 100);
            button10.Name = "button10";
            button10.Size = new Size(175, 80);
            button10.TabIndex = 10;
            button10.Text = "Surtido";
            button10.TextImageRelation = TextImageRelation.ImageAboveText;
            button10.UseVisualStyleBackColor = true;
            // 
            // shipmentBtn
            // 
            shipmentBtn.Cursor = Cursors.Hand;
            shipmentBtn.Image = Properties.Resources.salida1;
            shipmentBtn.Location = new Point(1099, 100);
            shipmentBtn.Name = "shipmentBtn";
            shipmentBtn.Size = new Size(175, 80);
            shipmentBtn.TabIndex = 12;
            shipmentBtn.Text = "Embarques";
            shipmentBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            shipmentBtn.UseVisualStyleBackColor = true;
            // 
            // reportBtn
            // 
            reportBtn.Cursor = Cursors.Hand;
            reportBtn.Image = Properties.Resources.grafica1;
            reportBtn.Location = new Point(1280, 100);
            reportBtn.Name = "reportBtn";
            reportBtn.Size = new Size(175, 80);
            reportBtn.TabIndex = 14;
            reportBtn.Text = "Reportes";
            reportBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            reportBtn.UseVisualStyleBackColor = true;
            // 
            // yardControlBtn
            // 
            yardControlBtn.Cursor = Cursors.Hand;
            yardControlBtn.Image = Properties.Resources.controlPatio;
            yardControlBtn.Location = new Point(13, 186);
            yardControlBtn.Name = "yardControlBtn";
            yardControlBtn.Size = new Size(175, 80);
            yardControlBtn.TabIndex = 19;
            yardControlBtn.Text = "Control de Patio";
            yardControlBtn.TextImageRelation = TextImageRelation.ImageAboveText;
            yardControlBtn.UseVisualStyleBackColor = true;
            // 
            // FrmMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1469, 801);
            Controls.Add(flwMenu);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
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
        private Button clientBtn;
        private PictureBox pictureBox1;
        private Button projectBtn;
        private Label lblClose;
        private TextBox searchWindow;
        private Button warehouseBtn;
        private Button locationBtn;
        private Button productBtn;
        private Button checkListBtn;
        private Button asnBtn;
        private Button button10;
        private Button auditBtn;
        private Button shipmentBtn;
        private Button randomInventoryBtn;
        private Button reportBtn;
        private Button movementBtn;
        private Button inventoryBtn;
        private Button userBtn;
        private Button yardControlBtn;
        private Button button8;
    }
}