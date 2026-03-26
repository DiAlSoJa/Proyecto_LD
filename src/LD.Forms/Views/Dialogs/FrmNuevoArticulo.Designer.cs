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
            tabControl1 = new TabControl();
            tabGeneral = new TabPage();
            cmbFamilia = new ComboBox();
            label14 = new Label();
            groupBox6 = new GroupBox();
            txtPeso = new LD.Controls.TextBoxControl();
            txtAncho = new LD.Controls.TextBoxControl();
            txtLargo = new LD.Controls.TextBoxControl();
            txtAlto = new LD.Controls.TextBoxControl();
            label27 = new Label();
            label28 = new Label();
            label29 = new Label();
            label30 = new Label();
            chkBOM = new CheckBox();
            chkVMI = new CheckBox();
            chkActivo = new CheckBox();
            chkTemperatura = new CheckBox();
            groupBox5 = new GroupBox();
            txtTiempoEntrega = new LD.Controls.TextBoxControl();
            txtMinimos = new LD.Controls.TextBoxControl();
            label21 = new Label();
            txtMaximos = new LD.Controls.TextBoxControl();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            txtOrden = new TextBox();
            label17 = new Label();
            chkNotificacionMax = new CheckBox();
            chkNotificacionMin = new CheckBox();
            groupBox3 = new GroupBox();
            txtFactorProduccion = new LD.Controls.TextBoxControl();
            txtFactorAlmacen = new LD.Controls.TextBoxControl();
            txtCostos = new LD.Controls.TextBoxControl();
            cmbUnidadProduccion = new ComboBox();
            label16 = new Label();
            cmbEstatusProduccion = new ComboBox();
            label15 = new Label();
            label4 = new Label();
            label2 = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            chkSolicitarReferencia = new CheckBox();
            chkSolicitarTipoCambio = new CheckBox();
            chkSolicitarOC = new CheckBox();
            chkSolicitarPedimento = new CheckBox();
            chkSolicitarLote = new CheckBox();
            chkSolicitarCaducidad = new CheckBox();
            groupBox4 = new GroupBox();
            txtValorPaqueteEst = new LD.Controls.TextBoxControl();
            txtValorUnidadMax = new LD.Controls.TextBoxControl();
            txtValorUnidadMedia = new LD.Controls.TextBoxControl();
            cmbPaqueteEstandar = new ComboBox();
            label10 = new Label();
            cmbUnidadMaxima = new ComboBox();
            label11 = new Label();
            cmbUnidadMedia = new ComboBox();
            label12 = new Label();
            cmbUnidadMinima = new ComboBox();
            label13 = new Label();
            cmbCategoria = new ComboBox();
            label9 = new Label();
            groupBox1 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            label6 = new Label();
            txtNoParte = new TextBox();
            label7 = new Label();
            txtDescripcion = new TextBox();
            tabAvanzada = new TabPage();
            txtNoParteProv = new TextBox();
            groupBox7 = new GroupBox();
            rdNotificacionEmail = new RadioButton();
            txtCorreoAlterno = new TextBox();
            rdNotificacionArchivos = new RadioButton();
            label24 = new Label();
            label22 = new Label();
            cmbRutaNotificacion = new ComboBox();
            cmbListaDistribucion = new ComboBox();
            label23 = new Label();
            label26 = new Label();
            cmbProveedor = new ComboBox();
            label25 = new Label();
            panel3 = new Panel();
            cmbCliente = new ComboBox();
            label8 = new Label();
            cmbProyecto = new ComboBox();
            label5 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabGeneral.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox1.SuspendLayout();
            tabAvanzada.SuspendLayout();
            groupBox7.SuspendLayout();
            panel3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(tabControl1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1194, 561);
            panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabGeneral);
            tabControl1.Controls.Add(tabAvanzada);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 75);
            tabControl1.Margin = new Padding(3, 2, 3, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1192, 443);
            tabControl1.TabIndex = 51;
            // 
            // tabGeneral
            // 
            tabGeneral.Controls.Add(cmbFamilia);
            tabGeneral.Controls.Add(label14);
            tabGeneral.Controls.Add(groupBox6);
            tabGeneral.Controls.Add(chkBOM);
            tabGeneral.Controls.Add(chkVMI);
            tabGeneral.Controls.Add(chkActivo);
            tabGeneral.Controls.Add(chkTemperatura);
            tabGeneral.Controls.Add(groupBox5);
            tabGeneral.Controls.Add(groupBox3);
            tabGeneral.Controls.Add(groupBox2);
            tabGeneral.Controls.Add(groupBox4);
            tabGeneral.Controls.Add(cmbCategoria);
            tabGeneral.Controls.Add(label9);
            tabGeneral.Controls.Add(groupBox1);
            tabGeneral.Controls.Add(label6);
            tabGeneral.Controls.Add(txtNoParte);
            tabGeneral.Controls.Add(label7);
            tabGeneral.Controls.Add(txtDescripcion);
            tabGeneral.Location = new Point(4, 24);
            tabGeneral.Margin = new Padding(3, 2, 3, 2);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new Padding(3, 2, 3, 2);
            tabGeneral.Size = new Size(1184, 415);
            tabGeneral.TabIndex = 0;
            tabGeneral.Text = "General";
            tabGeneral.UseVisualStyleBackColor = true;
            // 
            // cmbFamilia
            // 
            cmbFamilia.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbFamilia.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbFamilia.Font = new Font("Segoe UI", 9.75F);
            cmbFamilia.FormattingEnabled = true;
            cmbFamilia.Location = new Point(155, 109);
            cmbFamilia.Margin = new Padding(3, 2, 3, 2);
            cmbFamilia.Name = "cmbFamilia";
            cmbFamilia.Size = new Size(280, 25);
            cmbFamilia.TabIndex = 6;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(14, 116);
            label14.Name = "label14";
            label14.Size = new Size(48, 15);
            label14.TabIndex = 63;
            label14.Text = "Familia:";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(txtPeso);
            groupBox6.Controls.Add(txtAncho);
            groupBox6.Controls.Add(txtLargo);
            groupBox6.Controls.Add(txtAlto);
            groupBox6.Controls.Add(label27);
            groupBox6.Controls.Add(label28);
            groupBox6.Controls.Add(label29);
            groupBox6.Controls.Add(label30);
            groupBox6.Location = new Point(565, 271);
            groupBox6.Margin = new Padding(3, 2, 3, 2);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(3, 2, 3, 2);
            groupBox6.Size = new Size(573, 107);
            groupBox6.TabIndex = 61;
            groupBox6.TabStop = false;
            // 
            // txtPeso
            // 
            txtPeso.BackColor = Color.White;
            txtPeso.BorderColor = SystemColors.ControlDark;
            txtPeso.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPeso.IsNumber = true;
            txtPeso.Location = new Point(312, 12);
            txtPeso.Margin = new Padding(3, 2, 3, 2);
            txtPeso.Name = "txtPeso";
            txtPeso.Padding = new Padding(6, 5, 6, 5);
            txtPeso.Size = new Size(95, 26);
            txtPeso.TabIndex = 43;
            // 
            // txtAncho
            // 
            txtAncho.BackColor = Color.White;
            txtAncho.BorderColor = SystemColors.ControlDark;
            txtAncho.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAncho.IsNumber = true;
            txtAncho.Location = new Point(101, 69);
            txtAncho.Margin = new Padding(3, 2, 3, 2);
            txtAncho.Name = "txtAncho";
            txtAncho.Padding = new Padding(6, 5, 6, 5);
            txtAncho.Size = new Size(95, 26);
            txtAncho.TabIndex = 42;
            // 
            // txtLargo
            // 
            txtLargo.BackColor = Color.White;
            txtLargo.BorderColor = SystemColors.ControlDark;
            txtLargo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLargo.IsNumber = true;
            txtLargo.Location = new Point(101, 43);
            txtLargo.Margin = new Padding(3, 2, 3, 2);
            txtLargo.Name = "txtLargo";
            txtLargo.Padding = new Padding(6, 5, 6, 5);
            txtLargo.Size = new Size(95, 26);
            txtLargo.TabIndex = 41;
            // 
            // txtAlto
            // 
            txtAlto.BackColor = Color.White;
            txtAlto.BorderColor = SystemColors.ControlDark;
            txtAlto.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAlto.IsNumber = true;
            txtAlto.Location = new Point(101, 17);
            txtAlto.Margin = new Padding(3, 2, 3, 2);
            txtAlto.Name = "txtAlto";
            txtAlto.Padding = new Padding(6, 5, 6, 5);
            txtAlto.Size = new Size(95, 26);
            txtAlto.TabIndex = 40;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(270, 22);
            label27.Name = "label27";
            label27.Size = new Size(35, 15);
            label27.TabIndex = 64;
            label27.Text = "Peso:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(22, 74);
            label28.Name = "label28";
            label28.Size = new Size(45, 15);
            label28.TabIndex = 62;
            label28.Text = "Ancho:";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(29, 46);
            label29.Name = "label29";
            label29.Size = new Size(40, 15);
            label29.TabIndex = 60;
            label29.Text = "Largo:";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(35, 20);
            label30.Name = "label30";
            label30.Size = new Size(32, 15);
            label30.TabIndex = 58;
            label30.Text = "Alto:";
            // 
            // chkBOM
            // 
            chkBOM.AutoSize = true;
            chkBOM.Location = new Point(387, 136);
            chkBOM.Margin = new Padding(3, 2, 3, 2);
            chkBOM.Name = "chkBOM";
            chkBOM.Size = new Size(53, 19);
            chkBOM.TabIndex = 10;
            chkBOM.Text = "BOM";
            chkBOM.UseVisualStyleBackColor = true;
            // 
            // chkVMI
            // 
            chkVMI.AutoSize = true;
            chkVMI.Location = new Point(332, 136);
            chkVMI.Margin = new Padding(3, 2, 3, 2);
            chkVMI.Name = "chkVMI";
            chkVMI.Size = new Size(47, 19);
            chkVMI.TabIndex = 9;
            chkVMI.Text = "VMI";
            chkVMI.UseVisualStyleBackColor = true;
            // 
            // chkActivo
            // 
            chkActivo.AutoSize = true;
            chkActivo.Location = new Point(18, 136);
            chkActivo.Margin = new Padding(3, 2, 3, 2);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(60, 19);
            chkActivo.TabIndex = 7;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // chkTemperatura
            // 
            chkTemperatura.AutoSize = true;
            chkTemperatura.Location = new Point(155, 136);
            chkTemperatura.Margin = new Padding(3, 2, 3, 2);
            chkTemperatura.Name = "chkTemperatura";
            chkTemperatura.Size = new Size(152, 19);
            chkTemperatura.TabIndex = 8;
            chkTemperatura.Text = "Temperatura controlada";
            chkTemperatura.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(txtTiempoEntrega);
            groupBox5.Controls.Add(txtMinimos);
            groupBox5.Controls.Add(label21);
            groupBox5.Controls.Add(txtMaximos);
            groupBox5.Controls.Add(label20);
            groupBox5.Controls.Add(label19);
            groupBox5.Controls.Add(label18);
            groupBox5.Controls.Add(txtOrden);
            groupBox5.Controls.Add(label17);
            groupBox5.Controls.Add(chkNotificacionMax);
            groupBox5.Controls.Add(chkNotificacionMin);
            groupBox5.Location = new Point(14, 271);
            groupBox5.Margin = new Padding(3, 2, 3, 2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 2, 3, 2);
            groupBox5.Size = new Size(526, 107);
            groupBox5.TabIndex = 56;
            groupBox5.TabStop = false;
            // 
            // txtTiempoEntrega
            // 
            txtTiempoEntrega.BackColor = Color.White;
            txtTiempoEntrega.BorderColor = SystemColors.ControlDark;
            txtTiempoEntrega.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTiempoEntrega.IsNumber = true;
            txtTiempoEntrega.Location = new Point(373, 68);
            txtTiempoEntrega.Margin = new Padding(3, 2, 3, 2);
            txtTiempoEntrega.Name = "txtTiempoEntrega";
            txtTiempoEntrega.Padding = new Padding(6, 5, 6, 5);
            txtTiempoEntrega.Size = new Size(95, 26);
            txtTiempoEntrega.TabIndex = 39;
            // 
            // txtMinimos
            // 
            txtMinimos.BackColor = Color.White;
            txtMinimos.BorderColor = SystemColors.ControlDark;
            txtMinimos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMinimos.IsNumber = true;
            txtMinimos.Location = new Point(373, 42);
            txtMinimos.Margin = new Padding(3, 2, 3, 2);
            txtMinimos.Name = "txtMinimos";
            txtMinimos.Padding = new Padding(6, 5, 6, 5);
            txtMinimos.Size = new Size(95, 26);
            txtMinimos.TabIndex = 38;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(473, 74);
            label21.Name = "label21";
            label21.Size = new Size(28, 15);
            label21.TabIndex = 68;
            label21.Text = "días";
            // 
            // txtMaximos
            // 
            txtMaximos.BackColor = Color.White;
            txtMaximos.BorderColor = SystemColors.ControlDark;
            txtMaximos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMaximos.IsNumber = true;
            txtMaximos.Location = new Point(373, 16);
            txtMaximos.Margin = new Padding(3, 2, 3, 2);
            txtMaximos.Name = "txtMaximos";
            txtMaximos.Padding = new Padding(6, 5, 6, 5);
            txtMaximos.Size = new Size(95, 26);
            txtMaximos.TabIndex = 37;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(240, 71);
            label20.Name = "label20";
            label20.Size = new Size(109, 15);
            label20.TabIndex = 64;
            label20.Text = "Tiempo de entrega:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(240, 46);
            label19.Name = "label19";
            label19.Size = new Size(57, 15);
            label19.TabIndex = 62;
            label19.Text = "Mínimos:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(240, 20);
            label18.Name = "label18";
            label18.Size = new Size(59, 15);
            label18.TabIndex = 60;
            label18.Text = "Máximos:";
            // 
            // txtOrden
            // 
            txtOrden.BorderStyle = BorderStyle.FixedSingle;
            txtOrden.Font = new Font("Segoe UI", 9.75F);
            txtOrden.Location = new Point(93, 68);
            txtOrden.Margin = new Padding(3, 2, 3, 2);
            txtOrden.Name = "txtOrden";
            txtOrden.Size = new Size(96, 25);
            txtOrden.TabIndex = 36;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(14, 70);
            label17.Name = "label17";
            label17.Size = new Size(54, 15);
            label17.TabIndex = 58;
            label17.Text = "Reorden:";
            // 
            // chkNotificacionMax
            // 
            chkNotificacionMax.AutoSize = true;
            chkNotificacionMax.Location = new Point(16, 20);
            chkNotificacionMax.Margin = new Padding(3, 2, 3, 2);
            chkNotificacionMax.Name = "chkNotificacionMax";
            chkNotificacionMax.Size = new Size(159, 19);
            chkNotificacionMax.TabIndex = 34;
            chkNotificacionMax.Text = "Notificación de máximos";
            chkNotificacionMax.UseVisualStyleBackColor = true;
            // 
            // chkNotificacionMin
            // 
            chkNotificacionMin.AutoSize = true;
            chkNotificacionMin.Location = new Point(16, 42);
            chkNotificacionMin.Margin = new Padding(3, 2, 3, 2);
            chkNotificacionMin.Name = "chkNotificacionMin";
            chkNotificacionMin.Size = new Size(157, 19);
            chkNotificacionMin.TabIndex = 35;
            chkNotificacionMin.Text = "Notificación de mínimos";
            chkNotificacionMin.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtFactorProduccion);
            groupBox3.Controls.Add(txtFactorAlmacen);
            groupBox3.Controls.Add(txtCostos);
            groupBox3.Controls.Add(cmbUnidadProduccion);
            groupBox3.Controls.Add(label16);
            groupBox3.Controls.Add(cmbEstatusProduccion);
            groupBox3.Controls.Add(label15);
            groupBox3.Controls.Add(label4);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(486, 159);
            groupBox3.Margin = new Padding(3, 2, 3, 2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 2, 3, 2);
            groupBox3.Size = new Size(653, 107);
            groupBox3.TabIndex = 55;
            groupBox3.TabStop = false;
            // 
            // txtFactorProduccion
            // 
            txtFactorProduccion.BackColor = Color.White;
            txtFactorProduccion.BorderColor = SystemColors.ControlDark;
            txtFactorProduccion.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFactorProduccion.IsNumber = true;
            txtFactorProduccion.Location = new Point(145, 71);
            txtFactorProduccion.Margin = new Padding(3, 2, 3, 2);
            txtFactorProduccion.Name = "txtFactorProduccion";
            txtFactorProduccion.Padding = new Padding(6, 5, 6, 5);
            txtFactorProduccion.Size = new Size(95, 26);
            txtFactorProduccion.TabIndex = 30;
            // 
            // txtFactorAlmacen
            // 
            txtFactorAlmacen.BackColor = Color.White;
            txtFactorAlmacen.BorderColor = SystemColors.ControlDark;
            txtFactorAlmacen.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFactorAlmacen.IsNumber = true;
            txtFactorAlmacen.Location = new Point(145, 42);
            txtFactorAlmacen.Margin = new Padding(3, 2, 3, 2);
            txtFactorAlmacen.Name = "txtFactorAlmacen";
            txtFactorAlmacen.Padding = new Padding(6, 5, 6, 5);
            txtFactorAlmacen.Size = new Size(95, 26);
            txtFactorAlmacen.TabIndex = 29;
            // 
            // txtCostos
            // 
            txtCostos.BackColor = Color.White;
            txtCostos.BorderColor = SystemColors.ControlDark;
            txtCostos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCostos.IsNumber = true;
            txtCostos.Location = new Point(145, 13);
            txtCostos.Margin = new Padding(3, 2, 3, 2);
            txtCostos.Name = "txtCostos";
            txtCostos.Padding = new Padding(6, 5, 6, 5);
            txtCostos.Size = new Size(95, 26);
            txtCostos.TabIndex = 28;
            // 
            // cmbUnidadProduccion
            // 
            cmbUnidadProduccion.Font = new Font("Segoe UI", 9.75F);
            cmbUnidadProduccion.FormattingEnabled = true;
            cmbUnidadProduccion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbUnidadProduccion.Location = new Point(459, 71);
            cmbUnidadProduccion.Margin = new Padding(3, 2, 3, 2);
            cmbUnidadProduccion.Name = "cmbUnidadProduccion";
            cmbUnidadProduccion.Size = new Size(117, 25);
            cmbUnidadProduccion.TabIndex = 33;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(302, 74);
            label16.Name = "label16";
            label16.Size = new Size(128, 15);
            label16.TabIndex = 67;
            label16.Text = "Unidad de producción:";
            // 
            // cmbEstatusProduccion
            // 
            cmbEstatusProduccion.Font = new Font("Segoe UI", 9.75F);
            cmbEstatusProduccion.FormattingEnabled = true;
            cmbEstatusProduccion.Location = new Point(459, 45);
            cmbEstatusProduccion.Margin = new Padding(3, 2, 3, 2);
            cmbEstatusProduccion.Name = "cmbEstatusProduccion";
            cmbEstatusProduccion.Size = new Size(181, 25);
            cmbEstatusProduccion.TabIndex = 31;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(322, 48);
            label15.Name = "label15";
            label15.Size = new Size(111, 15);
            label15.TabIndex = 65;
            label15.Text = "Estatus producción:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(24, 74);
            label4.Name = "label4";
            label4.Size = new Size(107, 15);
            label4.TabIndex = 60;
            label4.Text = "Factor producción:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 48);
            label2.Name = "label2";
            label2.Size = new Size(91, 15);
            label2.TabIndex = 58;
            label2.Text = "Factor almacen:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 22);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 56;
            label3.Text = "Costos:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(chkSolicitarReferencia);
            groupBox2.Controls.Add(chkSolicitarTipoCambio);
            groupBox2.Controls.Add(chkSolicitarOC);
            groupBox2.Controls.Add(chkSolicitarPedimento);
            groupBox2.Controls.Add(chkSolicitarLote);
            groupBox2.Controls.Add(chkSolicitarCaducidad);
            groupBox2.Location = new Point(14, 159);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(455, 107);
            groupBox2.TabIndex = 54;
            groupBox2.TabStop = false;
            // 
            // chkSolicitarReferencia
            // 
            chkSolicitarReferencia.AutoSize = true;
            chkSolicitarReferencia.Location = new Point(254, 64);
            chkSolicitarReferencia.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarReferencia.Name = "chkSolicitarReferencia";
            chkSolicitarReferencia.Size = new Size(123, 19);
            chkSolicitarReferencia.TabIndex = 27;
            chkSolicitarReferencia.Text = "Solicitar referencia";
            chkSolicitarReferencia.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarTipoCambio
            // 
            chkSolicitarTipoCambio.AutoSize = true;
            chkSolicitarTipoCambio.Location = new Point(254, 20);
            chkSolicitarTipoCambio.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarTipoCambio.Name = "chkSolicitarTipoCambio";
            chkSolicitarTipoCambio.Size = new Size(151, 19);
            chkSolicitarTipoCambio.TabIndex = 25;
            chkSolicitarTipoCambio.Text = "Solicitar tipo de cambio";
            chkSolicitarTipoCambio.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarOC
            // 
            chkSolicitarOC.AutoSize = true;
            chkSolicitarOC.Location = new Point(254, 42);
            chkSolicitarOC.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarOC.Name = "chkSolicitarOC";
            chkSolicitarOC.Size = new Size(162, 19);
            chkSolicitarOC.TabIndex = 26;
            chkSolicitarOC.Text = "Solicitar orden de compra";
            chkSolicitarOC.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarPedimento
            // 
            chkSolicitarPedimento.AutoSize = true;
            chkSolicitarPedimento.Location = new Point(16, 64);
            chkSolicitarPedimento.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarPedimento.Name = "chkSolicitarPedimento";
            chkSolicitarPedimento.Size = new Size(190, 19);
            chkSolicitarPedimento.TabIndex = 24;
            chkSolicitarPedimento.Text = "Solicitar número de pedimento";
            chkSolicitarPedimento.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarLote
            // 
            chkSolicitarLote.AutoSize = true;
            chkSolicitarLote.Location = new Point(16, 20);
            chkSolicitarLote.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarLote.Name = "chkSolicitarLote";
            chkSolicitarLote.Size = new Size(152, 19);
            chkSolicitarLote.TabIndex = 22;
            chkSolicitarLote.Text = "Solicitar número de lote";
            chkSolicitarLote.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarCaducidad
            // 
            chkSolicitarCaducidad.AutoSize = true;
            chkSolicitarCaducidad.Location = new Point(16, 42);
            chkSolicitarCaducidad.Margin = new Padding(3, 2, 3, 2);
            chkSolicitarCaducidad.Name = "chkSolicitarCaducidad";
            chkSolicitarCaducidad.Size = new Size(174, 19);
            chkSolicitarCaducidad.TabIndex = 23;
            chkSolicitarCaducidad.Text = "Solicitar fecha de caducidad";
            chkSolicitarCaducidad.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtValorPaqueteEst);
            groupBox4.Controls.Add(txtValorUnidadMax);
            groupBox4.Controls.Add(txtValorUnidadMedia);
            groupBox4.Controls.Add(cmbPaqueteEstandar);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(cmbUnidadMaxima);
            groupBox4.Controls.Add(label11);
            groupBox4.Controls.Add(cmbUnidadMedia);
            groupBox4.Controls.Add(label12);
            groupBox4.Controls.Add(cmbUnidadMinima);
            groupBox4.Controls.Add(label13);
            groupBox4.Location = new Point(759, 16);
            groupBox4.Margin = new Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 2, 3, 2);
            groupBox4.Size = new Size(380, 137);
            groupBox4.TabIndex = 53;
            groupBox4.TabStop = false;
            groupBox4.Text = "Unidades";
            // 
            // txtValorPaqueteEst
            // 
            txtValorPaqueteEst.BackColor = Color.White;
            txtValorPaqueteEst.BorderColor = SystemColors.ControlDark;
            txtValorPaqueteEst.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtValorPaqueteEst.IsNumber = true;
            txtValorPaqueteEst.Location = new Point(311, 105);
            txtValorPaqueteEst.Margin = new Padding(3, 2, 3, 2);
            txtValorPaqueteEst.Name = "txtValorPaqueteEst";
            txtValorPaqueteEst.Padding = new Padding(6, 5, 6, 5);
            txtValorPaqueteEst.Size = new Size(56, 26);
            txtValorPaqueteEst.TabIndex = 18;
            // 
            // txtValorUnidadMax
            // 
            txtValorUnidadMax.BackColor = Color.White;
            txtValorUnidadMax.BorderColor = SystemColors.ControlDark;
            txtValorUnidadMax.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtValorUnidadMax.IsNumber = true;
            txtValorUnidadMax.Location = new Point(311, 79);
            txtValorUnidadMax.Margin = new Padding(3, 2, 3, 2);
            txtValorUnidadMax.Name = "txtValorUnidadMax";
            txtValorUnidadMax.Padding = new Padding(6, 5, 6, 5);
            txtValorUnidadMax.Size = new Size(56, 26);
            txtValorUnidadMax.TabIndex = 17;
            // 
            // txtValorUnidadMedia
            // 
            txtValorUnidadMedia.BackColor = Color.White;
            txtValorUnidadMedia.BorderColor = SystemColors.ControlDark;
            txtValorUnidadMedia.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtValorUnidadMedia.IsNumber = true;
            txtValorUnidadMedia.Location = new Point(311, 50);
            txtValorUnidadMedia.Margin = new Padding(3, 2, 3, 2);
            txtValorUnidadMedia.Name = "txtValorUnidadMedia";
            txtValorUnidadMedia.Padding = new Padding(6, 5, 6, 5);
            txtValorUnidadMedia.Size = new Size(56, 26);
            txtValorUnidadMedia.TabIndex = 16;
            // 
            // cmbPaqueteEstandar
            // 
            cmbPaqueteEstandar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPaqueteEstandar.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbPaqueteEstandar.Font = new Font("Segoe UI", 9.75F);
            cmbPaqueteEstandar.FormattingEnabled = true;
            cmbPaqueteEstandar.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbPaqueteEstandar.Location = new Point(141, 105);
            cmbPaqueteEstandar.Margin = new Padding(3, 2, 3, 2);
            cmbPaqueteEstandar.Name = "cmbPaqueteEstandar";
            cmbPaqueteEstandar.Size = new Size(162, 25);
            cmbPaqueteEstandar.TabIndex = 20;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 112);
            label10.Name = "label10";
            label10.Size = new Size(101, 15);
            label10.TabIndex = 50;
            label10.Text = "Paquete estandar:";
            // 
            // cmbUnidadMaxima
            // 
            cmbUnidadMaxima.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbUnidadMaxima.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbUnidadMaxima.Font = new Font("Segoe UI", 9.75F);
            cmbUnidadMaxima.FormattingEnabled = true;
            cmbUnidadMaxima.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbUnidadMaxima.Location = new Point(141, 79);
            cmbUnidadMaxima.Margin = new Padding(3, 2, 3, 2);
            cmbUnidadMaxima.Name = "cmbUnidadMaxima";
            cmbUnidadMaxima.Size = new Size(162, 25);
            cmbUnidadMaxima.TabIndex = 18;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(20, 86);
            label11.Name = "label11";
            label11.Size = new Size(94, 15);
            label11.TabIndex = 48;
            label11.Text = "Unidad máxima:";
            // 
            // cmbUnidadMedia
            // 
            cmbUnidadMedia.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbUnidadMedia.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbUnidadMedia.Font = new Font("Segoe UI", 9.75F);
            cmbUnidadMedia.FormattingEnabled = true;
            cmbUnidadMedia.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbUnidadMedia.Location = new Point(141, 52);
            cmbUnidadMedia.Margin = new Padding(3, 2, 3, 2);
            cmbUnidadMedia.Name = "cmbUnidadMedia";
            cmbUnidadMedia.Size = new Size(162, 25);
            cmbUnidadMedia.TabIndex = 16;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(20, 59);
            label12.Name = "label12";
            label12.Size = new Size(84, 15);
            label12.TabIndex = 46;
            label12.Text = "Unidad media:";
            // 
            // cmbUnidadMinima
            // 
            cmbUnidadMinima.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbUnidadMinima.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbUnidadMinima.Font = new Font("Segoe UI", 9.75F);
            cmbUnidadMinima.FormattingEnabled = true;
            cmbUnidadMinima.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbUnidadMinima.Location = new Point(141, 26);
            cmbUnidadMinima.Margin = new Padding(3, 2, 3, 2);
            cmbUnidadMinima.Name = "cmbUnidadMinima";
            cmbUnidadMinima.Size = new Size(162, 25);
            cmbUnidadMinima.TabIndex = 15;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(20, 33);
            label13.Name = "label13";
            label13.Size = new Size(92, 15);
            label13.TabIndex = 22;
            label13.Text = "Unidad mínima:";
            // 
            // cmbCategoria
            // 
            cmbCategoria.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCategoria.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCategoria.Font = new Font("Segoe UI", 9.75F);
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(155, 82);
            cmbCategoria.Margin = new Padding(3, 2, 3, 2);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(280, 25);
            cmbCategoria.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(14, 89);
            label9.Name = "label9";
            label9.Size = new Size(61, 15);
            label9.TabIndex = 51;
            label9.Text = "Categoria:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton4);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new Point(565, 16);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(179, 137);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tipo de almacenamiento";
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(19, 87);
            radioButton4.Margin = new Padding(3, 2, 3, 2);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(130, 19);
            radioButton4.TabIndex = 14;
            radioButton4.TabStop = true;
            radioButton4.Text = "Fecha de caducidad";
            radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(19, 64);
            radioButton3.Margin = new Padding(3, 2, 3, 2);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(108, 19);
            radioButton3.TabIndex = 13;
            radioButton3.TabStop = true;
            radioButton3.Text = "Número de lote";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(19, 42);
            radioButton2.Margin = new Padding(3, 2, 3, 2);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(142, 19);
            radioButton2.TabIndex = 12;
            radioButton2.TabStop = true;
            radioButton2.Text = "LIFO - Last In First Out";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(19, 20);
            radioButton1.Margin = new Padding(3, 2, 3, 2);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(143, 19);
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "FIFO - First In First Out";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 37);
            label6.Name = "label6";
            label6.Size = new Size(59, 15);
            label6.TabIndex = 36;
            label6.Text = "No. Parte:";
            // 
            // txtNoParte
            // 
            txtNoParte.BorderStyle = BorderStyle.FixedSingle;
            txtNoParte.Font = new Font("Segoe UI", 9.75F);
            txtNoParte.Location = new Point(155, 32);
            txtNoParte.Margin = new Padding(3, 2, 3, 2);
            txtNoParte.Name = "txtNoParte";
            txtNoParte.Size = new Size(355, 25);
            txtNoParte.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 63);
            label7.Name = "label7";
            label7.Size = new Size(72, 15);
            label7.TabIndex = 38;
            label7.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Font = new Font("Segoe UI", 9.75F);
            txtDescripcion.Location = new Point(155, 58);
            txtDescripcion.Margin = new Padding(3, 2, 3, 2);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(390, 25);
            txtDescripcion.TabIndex = 4;
            // 
            // tabAvanzada
            // 
            tabAvanzada.Controls.Add(txtNoParteProv);
            tabAvanzada.Controls.Add(groupBox7);
            tabAvanzada.Controls.Add(label26);
            tabAvanzada.Controls.Add(cmbProveedor);
            tabAvanzada.Controls.Add(label25);
            tabAvanzada.Location = new Point(4, 24);
            tabAvanzada.Margin = new Padding(3, 2, 3, 2);
            tabAvanzada.Name = "tabAvanzada";
            tabAvanzada.Padding = new Padding(3, 2, 3, 2);
            tabAvanzada.Size = new Size(1185, 415);
            tabAvanzada.TabIndex = 1;
            tabAvanzada.Text = "Avanzada";
            tabAvanzada.UseVisualStyleBackColor = true;
            // 
            // txtNoParteProv
            // 
            txtNoParteProv.BorderStyle = BorderStyle.FixedSingle;
            txtNoParteProv.Font = new Font("Segoe UI", 9.75F);
            txtNoParteProv.Location = new Point(215, 45);
            txtNoParteProv.Margin = new Padding(3, 2, 3, 2);
            txtNoParteProv.Name = "txtNoParteProv";
            txtNoParteProv.Size = new Size(367, 25);
            txtNoParteProv.TabIndex = 48;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(rdNotificacionEmail);
            groupBox7.Controls.Add(txtCorreoAlterno);
            groupBox7.Controls.Add(rdNotificacionArchivos);
            groupBox7.Controls.Add(label24);
            groupBox7.Controls.Add(label22);
            groupBox7.Controls.Add(cmbRutaNotificacion);
            groupBox7.Controls.Add(cmbListaDistribucion);
            groupBox7.Controls.Add(label23);
            groupBox7.Location = new Point(18, 91);
            groupBox7.Margin = new Padding(3, 2, 3, 2);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(3, 2, 3, 2);
            groupBox7.Size = new Size(620, 134);
            groupBox7.TabIndex = 82;
            groupBox7.TabStop = false;
            // 
            // rdNotificacionEmail
            // 
            rdNotificacionEmail.AutoSize = true;
            rdNotificacionEmail.Location = new Point(37, 29);
            rdNotificacionEmail.Margin = new Padding(3, 2, 3, 2);
            rdNotificacionEmail.Name = "rdNotificacionEmail";
            rdNotificacionEmail.Size = new Size(140, 19);
            rdNotificacionEmail.TabIndex = 49;
            rdNotificacionEmail.TabStop = true;
            rdNotificacionEmail.Text = "Notificación via email";
            rdNotificacionEmail.UseVisualStyleBackColor = true;
            // 
            // txtCorreoAlterno
            // 
            txtCorreoAlterno.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoAlterno.Font = new Font("Segoe UI", 9.75F);
            txtCorreoAlterno.Location = new Point(230, 80);
            txtCorreoAlterno.Margin = new Padding(3, 2, 3, 2);
            txtCorreoAlterno.Name = "txtCorreoAlterno";
            txtCorreoAlterno.Size = new Size(367, 25);
            txtCorreoAlterno.TabIndex = 53;
            // 
            // rdNotificacionArchivos
            // 
            rdNotificacionArchivos.AutoSize = true;
            rdNotificacionArchivos.Location = new Point(37, 52);
            rdNotificacionArchivos.Margin = new Padding(3, 2, 3, 2);
            rdNotificacionArchivos.Name = "rdNotificacionArchivos";
            rdNotificacionArchivos.Size = new Size(155, 19);
            rdNotificacionArchivos.TabIndex = 50;
            rdNotificacionArchivos.Text = "Notificación via archivos";
            rdNotificacionArchivos.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(130, 82);
            label24.Name = "label24";
            label24.Size = new Size(86, 15);
            label24.TabIndex = 81;
            label24.Text = "Correo alterno:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(226, 26);
            label22.Name = "label22";
            label22.Size = new Size(116, 15);
            label22.TabIndex = 79;
            label22.Text = "Lista de distribución:";
            // 
            // cmbRutaNotificacion
            // 
            cmbRutaNotificacion.Font = new Font("Segoe UI", 9.75F);
            cmbRutaNotificacion.FormattingEnabled = true;
            cmbRutaNotificacion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbRutaNotificacion.Location = new Point(371, 52);
            cmbRutaNotificacion.Margin = new Padding(3, 2, 3, 2);
            cmbRutaNotificacion.Name = "cmbRutaNotificacion";
            cmbRutaNotificacion.Size = new Size(226, 25);
            cmbRutaNotificacion.TabIndex = 52;
            // 
            // cmbListaDistribucion
            // 
            cmbListaDistribucion.Font = new Font("Segoe UI", 9.75F);
            cmbListaDistribucion.FormattingEnabled = true;
            cmbListaDistribucion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbListaDistribucion.Location = new Point(371, 23);
            cmbListaDistribucion.Margin = new Padding(3, 2, 3, 2);
            cmbListaDistribucion.Name = "cmbListaDistribucion";
            cmbListaDistribucion.Size = new Size(226, 25);
            cmbListaDistribucion.TabIndex = 51;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(226, 55);
            label23.Name = "label23";
            label23.Size = new Size(116, 15);
            label23.TabIndex = 80;
            label23.Text = "Ruta de notificación:";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(18, 52);
            label26.Name = "label26";
            label26.Size = new Size(157, 15);
            label26.TabIndex = 55;
            label26.Text = "Número de parte proveedor:";
            // 
            // cmbProveedor
            // 
            cmbProveedor.Font = new Font("Segoe UI", 9.75F);
            cmbProveedor.FormattingEnabled = true;
            cmbProveedor.Location = new Point(215, 15);
            cmbProveedor.Margin = new Padding(3, 2, 3, 2);
            cmbProveedor.Name = "cmbProveedor";
            cmbProveedor.Size = new Size(350, 25);
            cmbProveedor.TabIndex = 47;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(21, 22);
            label25.Name = "label25";
            label25.Size = new Size(64, 15);
            label25.TabIndex = 53;
            label25.Text = "Proveedor:";
            // 
            // panel3
            // 
            panel3.Controls.Add(cmbCliente);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(cmbProyecto);
            panel3.Controls.Add(label5);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 26);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(1192, 49);
            panel3.TabIndex = 52;
            // 
            // cmbCliente
            // 
            cmbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCliente.Font = new Font("Segoe UI", 9.75F);
            cmbCliente.FormattingEnabled = true;
            cmbCliente.Location = new Point(92, 12);
            cmbCliente.Margin = new Padding(3, 2, 3, 2);
            cmbCliente.Name = "cmbCliente";
            cmbCliente.Size = new Size(407, 25);
            cmbCliente.TabIndex = 1;
            cmbCliente.SelectedIndexChanged += cmbCliente_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(22, 19);
            label8.Name = "label8";
            label8.Size = new Size(47, 15);
            label8.TabIndex = 34;
            label8.Text = "Cliente:";
            // 
            // cmbProyecto
            // 
            cmbProyecto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProyecto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbProyecto.Font = new Font("Segoe UI", 9.75F);
            cmbProyecto.FormattingEnabled = true;
            cmbProyecto.Location = new Point(598, 12);
            cmbProyecto.Margin = new Padding(3, 2, 3, 2);
            cmbProyecto.Name = "cmbProyecto";
            cmbProyecto.Size = new Size(407, 25);
            cmbProyecto.TabIndex = 2;
            cmbProyecto.SelectedIndexChanged += cmbProyecto_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(522, 15);
            label5.Name = "label5";
            label5.Size = new Size(57, 15);
            label5.TabIndex = 35;
            label5.Text = "Proyecto:";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = SystemColors.Control;
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 518);
            flowLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(4, 4, 4, 4);
            flowLayoutPanel1.Size = new Size(1192, 41);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1031, 6);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(150, 26);
            button2.TabIndex = 45;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(875, 6);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 26);
            btnSave.TabIndex = 44;
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
            panel2.Size = new Size(1192, 26);
            panel2.TabIndex = 1;
            panel2.DoubleClick += panel2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(18, 4);
            label1.Name = "label1";
            label1.Size = new Size(90, 16);
            label1.TabIndex = 3;
            label1.Text = "Nuevo artcículo";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1160, 0);
            pictureBox2.Margin = new Padding(3, 2, 3, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(4, 8, 0, 0);
            pictureBox2.Size = new Size(32, 26);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevoArticulo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1194, 561);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmNuevoArticulo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabGeneral.ResumeLayout(false);
            tabGeneral.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabAvanzada.ResumeLayout(false);
            tabAvanzada.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
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
        private GroupBox groupBox1;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private ComboBox cmbProyecto;
        private ComboBox cmbCliente;
        private CheckBox chkSolicitarCaducidad;
        private CheckBox chkSolicitarLote;
        private TextBox txtDescripcion;
        private Label label7;
        private TextBox txtNoParte;
        private Label label6;
        private Label label5;
        private Label label8;
        private TabControl tabControl1;
        private TabPage tabGeneral;
        private TabPage tabAvanzada;
        private Panel panel3;
        private ComboBox cmbCategoria;
        private Label label9;
        private GroupBox groupBox4;
        private ComboBox cmbPaqueteEstandar;
        private Label label10;
        private ComboBox cmbUnidadMaxima;
        private Label label11;
        private ComboBox cmbUnidadMedia;
        private Label label12;
        private ComboBox cmbUnidadMinima;
        private Label label13;
        private GroupBox groupBox2;
        private CheckBox chkSolicitarPedimento;
        private CheckBox chkSolicitarReferencia;
        private CheckBox chkSolicitarTipoCambio;
        private CheckBox chkSolicitarOC;
        private GroupBox groupBox3;
        private TextBox textBox4;
        private Label label3;
        private Label label4;
        private Label label2;
        private ComboBox comboBox5;
        private Label label16;
        private ComboBox cmbEstatusProduccion;
        private Label label15;
        private GroupBox groupBox5;
        private CheckBox chkNotificacionMax;
        private CheckBox chkNotificacionMin;
        private TextBox txtOrden;
        private Label label17;
        private Label label18;
        private Label label20;
        private Label label19;
        private Label label21;
        private ComboBox cmbProveedor;
        private Label label25;
        private Label label26;
        private CheckBox chkBOM;
        private CheckBox chkVMI;
        private CheckBox chkActivo;
        private CheckBox chkTemperatura;
        private TextBox txtCorreoAlterno;
        private Label label24;
        private ComboBox cmbRutaNotificacion;
        private Label label23;
        private ComboBox cmbListaDistribucion;
        private Label label22;
        private RadioButton rdNotificacionArchivos;
        private RadioButton rdNotificacionEmail;
        private GroupBox groupBox6;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label30;
        private GroupBox groupBox7;
        private ComboBox cmbFamilia;
        private ComboBox cmbUnidadProduccion;
        private Label label14;
        private TextBox txtNoParteProv;
        private LD.Controls.TextBoxControl txtCostos;
        private LD.Controls.TextBoxControl txtFactorAlmacen;
        private LD.Controls.TextBoxControl txtFactorProduccion;
        private LD.Controls.TextBoxControl txtAncho;
        private LD.Controls.TextBoxControl txtLargo;
        private LD.Controls.TextBoxControl txtAlto;
        private LD.Controls.TextBoxControl txtPeso;
        private LD.Controls.TextBoxControl txtMaximos;
        private LD.Controls.TextBoxControl txtMinimos;
        private LD.Controls.TextBoxControl txtTiempoEntrega;
        private LD.Controls.TextBoxControl txtValorPaqueteEst;
        private LD.Controls.TextBoxControl txtValorUnidadMax;
        private LD.Controls.TextBoxControl txtValorUnidadMedia;
    }
}