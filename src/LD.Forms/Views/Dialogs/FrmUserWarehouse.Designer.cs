namespace LD.Forms.Views.Dialogs
{
    partial class FrmUserWarehouse
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panelContainer = new Panel();
            removeBtn = new Button();
            addWBtn = new Button();
            gridWarehouseFaltantes = new DataGridView();
            gridWarehouseAdded = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            panelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridWarehouseFaltantes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridWarehouseAdded).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panelContainer);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1087, 569);
            panel1.TabIndex = 0;
            // 
            // panelContainer
            // 
            panelContainer.Controls.Add(removeBtn);
            panelContainer.Controls.Add(addWBtn);
            panelContainer.Controls.Add(gridWarehouseFaltantes);
            panelContainer.Controls.Add(gridWarehouseAdded);
            panelContainer.Location = new Point(19, 60);
            panelContainer.Name = "panelContainer";
            panelContainer.Size = new Size(1042, 435);
            panelContainer.TabIndex = 11;
            // 
            // removeBtn
            // 
            removeBtn.Location = new Point(489, 58);
            removeBtn.Name = "removeBtn";
            removeBtn.Size = new Size(55, 32);
            removeBtn.TabIndex = 13;
            removeBtn.Text = ">>";
            removeBtn.UseVisualStyleBackColor = true;
            removeBtn.Click += removeBtn_Click;
            // 
            // addWBtn
            // 
            addWBtn.Location = new Point(489, 18);
            addWBtn.Name = "addWBtn";
            addWBtn.Size = new Size(55, 34);
            addWBtn.TabIndex = 12;
            addWBtn.Text = "<<";
            addWBtn.UseVisualStyleBackColor = true;
            addWBtn.Click += addWBtn_Click;
            // 
            // gridWarehouseFaltantes
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            gridWarehouseFaltantes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            gridWarehouseFaltantes.BackgroundColor = SystemColors.ButtonHighlight;
            gridWarehouseFaltantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridWarehouseFaltantes.Dock = DockStyle.Right;
            gridWarehouseFaltantes.Location = new Point(550, 0);
            gridWarehouseFaltantes.Name = "gridWarehouseFaltantes";
            gridWarehouseFaltantes.RowHeadersWidth = 51;
            gridWarehouseFaltantes.Size = new Size(492, 435);
            gridWarehouseFaltantes.TabIndex = 11;
            gridWarehouseFaltantes.SelectionChanged += gridWarehouseFaltantes_SelectionChanged;
            // 
            // gridWarehouseAdded
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            gridWarehouseAdded.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            gridWarehouseAdded.BackgroundColor = SystemColors.ButtonHighlight;
            gridWarehouseAdded.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridWarehouseAdded.Dock = DockStyle.Left;
            gridWarehouseAdded.Location = new Point(0, 0);
            gridWarehouseAdded.Name = "gridWarehouseAdded";
            gridWarehouseAdded.RowHeadersWidth = 51;
            gridWarehouseAdded.Size = new Size(483, 435);
            gridWarehouseAdded.TabIndex = 10;
            gridWarehouseAdded.SelectionChanged += gridWarehouseAdded_SelectionChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 512);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1085, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(920, 8);
            button2.Name = "button2";
            button2.Size = new Size(152, 35);
            button2.TabIndex = 7;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Green;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1085, 35);
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
            label1.Size = new Size(85, 22);
            label1.TabIndex = 3;
            label1.Text = "Almacenes";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1049, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmUserWarehouse
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1087, 569);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmUserWarehouse";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo usuario";
            panel1.ResumeLayout(false);
            panelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridWarehouseFaltantes).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridWarehouseAdded).EndInit();
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
        private FlowLayoutPanel flowLayoutPanel1;
        private DataGridView gridWarehouseAdded;
        private Panel panelContainer;
        private DataGridView gridWarehouseFaltantes;
        private Button removeBtn;
        private Button addWBtn;
    }
}