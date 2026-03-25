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
            panel1.Name = "panel1";
            panel1.Size = new Size(1365, 748);
            panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabGeneral);
            tabControl1.Controls.Add(tabAvanzada);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 100);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1363, 591);
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
            tabGeneral.Location = new Point(4, 29);
            tabGeneral.Name = "tabGeneral";
            tabGeneral.Padding = new Padding(3);
            tabGeneral.Size = new Size(1355, 558);
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
            cmbFamilia.Location = new Point(177, 145);
            cmbFamilia.Name = "cmbFamilia";
            cmbFamilia.Size = new Size(320, 29);
            cmbFamilia.TabIndex = 6;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(16, 154);
            label14.Name = "label14";
            label14.Size = new Size(59, 20);
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
            groupBox6.Location = new Point(646, 361);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(655, 143);
            groupBox6.TabIndex = 61;
            groupBox6.TabStop = false;
            // 
            // txtPeso
            // 
            txtPeso.BackColor = Color.White;
            txtPeso.BorderColor = SystemColors.ControlDark;
            txtPeso.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPeso.IsNumber = true;
            txtPeso.Location = new Point(356, 16);
            txtPeso.Name = "txtPeso";
            txtPeso.Padding = new Padding(7);
            txtPeso.Size = new Size(109, 33);
            txtPeso.TabIndex = 43;
            // 
            // txtAncho
            // 
            txtAncho.BackColor = Color.White;
            txtAncho.BorderColor = SystemColors.ControlDark;
            txtAncho.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAncho.IsNumber = true;
            txtAncho.Location = new Point(115, 90);
            txtAncho.Name = "txtAncho";
            txtAncho.Padding = new Padding(7);
            txtAncho.Size = new Size(109, 33);
            txtAncho.TabIndex = 42;
            // 
            // txtLargo
            // 
            txtLargo.BackColor = Color.White;
            txtLargo.BorderColor = SystemColors.ControlDark;
            txtLargo.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLargo.IsNumber = true;
            txtLargo.Location = new Point(115, 56);
            txtLargo.Name = "txtLargo";
            txtLargo.Padding = new Padding(7);
            txtLargo.Size = new Size(109, 33);
            txtLargo.TabIndex = 41;
            // 
            // txtAlto
            // 
            txtAlto.BackColor = Color.White;
            txtAlto.BorderColor = SystemColors.ControlDark;
            txtAlto.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAlto.IsNumber = true;
            txtAlto.Location = new Point(115, 23);
            txtAlto.Name = "txtAlto";
            txtAlto.Padding = new Padding(7);
            txtAlto.Size = new Size(109, 33);
            txtAlto.TabIndex = 40;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(308, 29);
            label27.Name = "label27";
            label27.Size = new Size(42, 20);
            label27.TabIndex = 64;
            label27.Text = "Peso:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(25, 95);
            label28.Name = "label28";
            label28.Size = new Size(54, 20);
            label28.TabIndex = 62;
            label28.Text = "Ancho:";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(22, 60);
            label29.Name = "label29";
            label29.Size = new Size(50, 20);
            label29.TabIndex = 60;
            label29.Text = "Largo:";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(25, 30);
            label30.Name = "label30";
            label30.Size = new Size(40, 20);
            label30.TabIndex = 58;
            label30.Text = "Alto:";
            // 
            // chkBOM
            // 
            chkBOM.AutoSize = true;
            chkBOM.Location = new Point(442, 182);
            chkBOM.Name = "chkBOM";
            chkBOM.Size = new Size(64, 24);
            chkBOM.TabIndex = 10;
            chkBOM.Text = "BOM";
            chkBOM.UseVisualStyleBackColor = true;
            // 
            // chkVMI
            // 
            chkVMI.AutoSize = true;
            chkVMI.Location = new Point(379, 182);
            chkVMI.Name = "chkVMI";
            chkVMI.Size = new Size(57, 24);
            chkVMI.TabIndex = 9;
            chkVMI.Text = "VMI";
            chkVMI.UseVisualStyleBackColor = true;
            // 
            // chkActivo
            // 
            chkActivo.AutoSize = true;
            chkActivo.Location = new Point(21, 181);
            chkActivo.Name = "chkActivo";
            chkActivo.Size = new Size(73, 24);
            chkActivo.TabIndex = 7;
            chkActivo.Text = "Activo";
            chkActivo.UseVisualStyleBackColor = true;
            // 
            // chkTemperatura
            // 
            chkTemperatura.AutoSize = true;
            chkTemperatura.Location = new Point(177, 182);
            chkTemperatura.Name = "chkTemperatura";
            chkTemperatura.Size = new Size(191, 24);
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
            groupBox5.Location = new Point(16, 361);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(601, 143);
            groupBox5.TabIndex = 56;
            groupBox5.TabStop = false;
            // 
            // txtTiempoEntrega
            // 
            txtTiempoEntrega.BackColor = Color.White;
            txtTiempoEntrega.BorderColor = SystemColors.ControlDark;
            txtTiempoEntrega.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTiempoEntrega.IsNumber = true;
            txtTiempoEntrega.Location = new Point(426, 90);
            txtTiempoEntrega.Name = "txtTiempoEntrega";
            txtTiempoEntrega.Padding = new Padding(7);
            txtTiempoEntrega.Size = new Size(109, 33);
            txtTiempoEntrega.TabIndex = 39;
            // 
            // txtMinimos
            // 
            txtMinimos.BackColor = Color.White;
            txtMinimos.BorderColor = SystemColors.ControlDark;
            txtMinimos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMinimos.IsNumber = true;
            txtMinimos.Location = new Point(426, 56);
            txtMinimos.Name = "txtMinimos";
            txtMinimos.Padding = new Padding(7);
            txtMinimos.Size = new Size(109, 33);
            txtMinimos.TabIndex = 38;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(541, 98);
            label21.Name = "label21";
            label21.Size = new Size(36, 20);
            label21.TabIndex = 68;
            label21.Text = "días";
            // 
            // txtMaximos
            // 
            txtMaximos.BackColor = Color.White;
            txtMaximos.BorderColor = SystemColors.ControlDark;
            txtMaximos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMaximos.IsNumber = true;
            txtMaximos.Location = new Point(426, 22);
            txtMaximos.Name = "txtMaximos";
            txtMaximos.Padding = new Padding(7);
            txtMaximos.Size = new Size(109, 33);
            txtMaximos.TabIndex = 37;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(274, 95);
            label20.Name = "label20";
            label20.Size = new Size(139, 20);
            label20.TabIndex = 64;
            label20.Text = "Tiempo de entrega:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(274, 61);
            label19.Name = "label19";
            label19.Size = new Size(69, 20);
            label19.TabIndex = 62;
            label19.Text = "Mínimos:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(274, 26);
            label18.Name = "label18";
            label18.Size = new Size(72, 20);
            label18.TabIndex = 60;
            label18.Text = "Máximos:";
            // 
            // txtOrden
            // 
            txtOrden.BorderStyle = BorderStyle.FixedSingle;
            txtOrden.Font = new Font("Segoe UI", 9.75F);
            txtOrden.Location = new Point(106, 90);
            txtOrden.Name = "txtOrden";
            txtOrden.Size = new Size(109, 29);
            txtOrden.TabIndex = 36;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(16, 93);
            label17.Name = "label17";
            label17.Size = new Size(68, 20);
            label17.TabIndex = 58;
            label17.Text = "Reorden:";
            // 
            // chkNotificacionMax
            // 
            chkNotificacionMax.AutoSize = true;
            chkNotificacionMax.Location = new Point(18, 26);
            chkNotificacionMax.Name = "chkNotificacionMax";
            chkNotificacionMax.Size = new Size(197, 24);
            chkNotificacionMax.TabIndex = 34;
            chkNotificacionMax.Text = "Notificación de máximos";
            chkNotificacionMax.UseVisualStyleBackColor = true;
            // 
            // chkNotificacionMin
            // 
            chkNotificacionMin.AutoSize = true;
            chkNotificacionMin.Location = new Point(18, 56);
            chkNotificacionMin.Name = "chkNotificacionMin";
            chkNotificacionMin.Size = new Size(194, 24);
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
            groupBox3.Location = new Point(555, 212);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(746, 143);
            groupBox3.TabIndex = 55;
            groupBox3.TabStop = false;
            // 
            // txtFactorProduccion
            // 
            txtFactorProduccion.BackColor = Color.White;
            txtFactorProduccion.BorderColor = SystemColors.ControlDark;
            txtFactorProduccion.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFactorProduccion.IsNumber = true;
            txtFactorProduccion.Location = new Point(166, 95);
            txtFactorProduccion.Name = "txtFactorProduccion";
            txtFactorProduccion.Padding = new Padding(7);
            txtFactorProduccion.Size = new Size(109, 33);
            txtFactorProduccion.TabIndex = 30;
            // 
            // txtFactorAlmacen
            // 
            txtFactorAlmacen.BackColor = Color.White;
            txtFactorAlmacen.BorderColor = SystemColors.ControlDark;
            txtFactorAlmacen.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFactorAlmacen.IsNumber = true;
            txtFactorAlmacen.Location = new Point(166, 56);
            txtFactorAlmacen.Name = "txtFactorAlmacen";
            txtFactorAlmacen.Padding = new Padding(7);
            txtFactorAlmacen.Size = new Size(109, 33);
            txtFactorAlmacen.TabIndex = 29;
            // 
            // txtCostos
            // 
            txtCostos.BackColor = Color.White;
            txtCostos.BorderColor = SystemColors.ControlDark;
            txtCostos.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCostos.IsNumber = true;
            txtCostos.Location = new Point(166, 17);
            txtCostos.Name = "txtCostos";
            txtCostos.Padding = new Padding(7);
            txtCostos.Size = new Size(109, 33);
            txtCostos.TabIndex = 28;
            // 
            // cmbUnidadProduccion
            // 
            cmbUnidadProduccion.Font = new Font("Segoe UI", 9.75F);
            cmbUnidadProduccion.FormattingEnabled = true;
            cmbUnidadProduccion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbUnidadProduccion.Location = new Point(525, 95);
            cmbUnidadProduccion.Name = "cmbUnidadProduccion";
            cmbUnidadProduccion.Size = new Size(133, 29);
            cmbUnidadProduccion.TabIndex = 33;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(345, 99);
            label16.Name = "label16";
            label16.Size = new Size(160, 20);
            label16.TabIndex = 67;
            label16.Text = "Unidad de producción:";
            // 
            // cmbEstatusProduccion
            // 
            cmbEstatusProduccion.Font = new Font("Segoe UI", 9.75F);
            cmbEstatusProduccion.FormattingEnabled = true;
            cmbEstatusProduccion.Location = new Point(525, 60);
            cmbEstatusProduccion.Name = "cmbEstatusProduccion";
            cmbEstatusProduccion.Size = new Size(206, 29);
            cmbEstatusProduccion.TabIndex = 31;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(368, 64);
            label15.Name = "label15";
            label15.Size = new Size(137, 20);
            label15.TabIndex = 65;
            label15.Text = "Estatus producción:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(28, 99);
            label4.Name = "label4";
            label4.Size = new Size(131, 20);
            label4.TabIndex = 60;
            label4.Text = "Factor producción:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(28, 64);
            label2.Name = "label2";
            label2.Size = new Size(112, 20);
            label2.TabIndex = 58;
            label2.Text = "Factor almacen:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 29);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
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
            groupBox2.Location = new Point(16, 212);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(520, 143);
            groupBox2.TabIndex = 54;
            groupBox2.TabStop = false;
            // 
            // chkSolicitarReferencia
            // 
            chkSolicitarReferencia.AutoSize = true;
            chkSolicitarReferencia.Location = new Point(290, 86);
            chkSolicitarReferencia.Name = "chkSolicitarReferencia";
            chkSolicitarReferencia.Size = new Size(155, 24);
            chkSolicitarReferencia.TabIndex = 27;
            chkSolicitarReferencia.Text = "Solicitar referencia";
            chkSolicitarReferencia.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarTipoCambio
            // 
            chkSolicitarTipoCambio.AutoSize = true;
            chkSolicitarTipoCambio.Location = new Point(290, 26);
            chkSolicitarTipoCambio.Name = "chkSolicitarTipoCambio";
            chkSolicitarTipoCambio.Size = new Size(191, 24);
            chkSolicitarTipoCambio.TabIndex = 25;
            chkSolicitarTipoCambio.Text = "Solicitar tipo de cambio";
            chkSolicitarTipoCambio.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarOC
            // 
            chkSolicitarOC.AutoSize = true;
            chkSolicitarOC.Location = new Point(290, 56);
            chkSolicitarOC.Name = "chkSolicitarOC";
            chkSolicitarOC.Size = new Size(204, 24);
            chkSolicitarOC.TabIndex = 26;
            chkSolicitarOC.Text = "Solicitar orden de compra";
            chkSolicitarOC.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarPedimento
            // 
            chkSolicitarPedimento.AutoSize = true;
            chkSolicitarPedimento.Location = new Point(18, 86);
            chkSolicitarPedimento.Name = "chkSolicitarPedimento";
            chkSolicitarPedimento.Size = new Size(238, 24);
            chkSolicitarPedimento.TabIndex = 24;
            chkSolicitarPedimento.Text = "Solicitar número de pedimento";
            chkSolicitarPedimento.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarLote
            // 
            chkSolicitarLote.AutoSize = true;
            chkSolicitarLote.Location = new Point(18, 26);
            chkSolicitarLote.Name = "chkSolicitarLote";
            chkSolicitarLote.Size = new Size(191, 24);
            chkSolicitarLote.TabIndex = 22;
            chkSolicitarLote.Text = "Solicitar número de lote";
            chkSolicitarLote.UseVisualStyleBackColor = true;
            // 
            // chkSolicitarCaducidad
            // 
            chkSolicitarCaducidad.AutoSize = true;
            chkSolicitarCaducidad.Location = new Point(18, 56);
            chkSolicitarCaducidad.Name = "chkSolicitarCaducidad";
            chkSolicitarCaducidad.Size = new Size(219, 24);
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
            groupBox4.Location = new Point(867, 22);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(434, 183);
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
            txtValorPaqueteEst.Location = new Point(355, 140);
            txtValorPaqueteEst.Name = "txtValorPaqueteEst";
            txtValorPaqueteEst.Padding = new Padding(7);
            txtValorPaqueteEst.Size = new Size(64, 33);
            txtValorPaqueteEst.TabIndex = 18;
            // 
            // txtValorUnidadMax
            // 
            txtValorUnidadMax.BackColor = Color.White;
            txtValorUnidadMax.BorderColor = SystemColors.ControlDark;
            txtValorUnidadMax.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtValorUnidadMax.IsNumber = true;
            txtValorUnidadMax.Location = new Point(355, 105);
            txtValorUnidadMax.Name = "txtValorUnidadMax";
            txtValorUnidadMax.Padding = new Padding(7);
            txtValorUnidadMax.Size = new Size(64, 33);
            txtValorUnidadMax.TabIndex = 17;
            // 
            // txtValorUnidadMedia
            // 
            txtValorUnidadMedia.BackColor = Color.White;
            txtValorUnidadMedia.BorderColor = SystemColors.ControlDark;
            txtValorUnidadMedia.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtValorUnidadMedia.IsNumber = true;
            txtValorUnidadMedia.Location = new Point(355, 66);
            txtValorUnidadMedia.Name = "txtValorUnidadMedia";
            txtValorUnidadMedia.Padding = new Padding(7);
            txtValorUnidadMedia.Size = new Size(64, 33);
            txtValorUnidadMedia.TabIndex = 16;
            // 
            // cmbPaqueteEstandar
            // 
            cmbPaqueteEstandar.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbPaqueteEstandar.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbPaqueteEstandar.Font = new Font("Segoe UI", 9.75F);
            cmbPaqueteEstandar.FormattingEnabled = true;
            cmbPaqueteEstandar.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbPaqueteEstandar.Location = new Point(161, 140);
            cmbPaqueteEstandar.Name = "cmbPaqueteEstandar";
            cmbPaqueteEstandar.Size = new Size(185, 29);
            cmbPaqueteEstandar.TabIndex = 20;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(23, 149);
            label10.Name = "label10";
            label10.Size = new Size(126, 20);
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
            cmbUnidadMaxima.Location = new Point(161, 105);
            cmbUnidadMaxima.Name = "cmbUnidadMaxima";
            cmbUnidadMaxima.Size = new Size(185, 29);
            cmbUnidadMaxima.TabIndex = 18;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(23, 114);
            label11.Name = "label11";
            label11.Size = new Size(117, 20);
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
            cmbUnidadMedia.Location = new Point(161, 70);
            cmbUnidadMedia.Name = "cmbUnidadMedia";
            cmbUnidadMedia.Size = new Size(185, 29);
            cmbUnidadMedia.TabIndex = 16;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(23, 79);
            label12.Name = "label12";
            label12.Size = new Size(106, 20);
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
            cmbUnidadMinima.Location = new Point(161, 35);
            cmbUnidadMinima.Name = "cmbUnidadMinima";
            cmbUnidadMinima.Size = new Size(185, 29);
            cmbUnidadMinima.TabIndex = 15;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(23, 44);
            label13.Name = "label13";
            label13.Size = new Size(114, 20);
            label13.TabIndex = 22;
            label13.Text = "Unidad mínima:";
            // 
            // cmbCategoria
            // 
            cmbCategoria.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCategoria.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCategoria.Font = new Font("Segoe UI", 9.75F);
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Location = new Point(177, 110);
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.Size = new Size(320, 29);
            cmbCategoria.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(16, 119);
            label9.Name = "label9";
            label9.Size = new Size(77, 20);
            label9.TabIndex = 51;
            label9.Text = "Categoria:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioButton4);
            groupBox1.Controls.Add(radioButton3);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Controls.Add(radioButton1);
            groupBox1.Location = new Point(646, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(205, 183);
            groupBox1.TabIndex = 45;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tipo de almacenamiento";
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(22, 116);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(162, 24);
            radioButton4.TabIndex = 14;
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
            radioButton3.TabIndex = 13;
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
            radioButton2.TabIndex = 12;
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
            radioButton1.TabIndex = 11;
            radioButton1.TabStop = true;
            radioButton1.Text = "FIFO - First In First Out";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 49);
            label6.Name = "label6";
            label6.Size = new Size(72, 20);
            label6.TabIndex = 36;
            label6.Text = "No. Parte:";
            // 
            // txtNoParte
            // 
            txtNoParte.BorderStyle = BorderStyle.FixedSingle;
            txtNoParte.Font = new Font("Segoe UI", 9.75F);
            txtNoParte.Location = new Point(177, 42);
            txtNoParte.Name = "txtNoParte";
            txtNoParte.Size = new Size(405, 29);
            txtNoParte.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 84);
            label7.Name = "label7";
            label7.Size = new Size(90, 20);
            label7.TabIndex = 38;
            label7.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Font = new Font("Segoe UI", 9.75F);
            txtDescripcion.Location = new Point(177, 77);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(445, 29);
            txtDescripcion.TabIndex = 4;
            // 
            // tabAvanzada
            // 
            tabAvanzada.Controls.Add(txtNoParteProv);
            tabAvanzada.Controls.Add(groupBox7);
            tabAvanzada.Controls.Add(label26);
            tabAvanzada.Controls.Add(cmbProveedor);
            tabAvanzada.Controls.Add(label25);
            tabAvanzada.Location = new Point(4, 29);
            tabAvanzada.Name = "tabAvanzada";
            tabAvanzada.Padding = new Padding(3);
            tabAvanzada.Size = new Size(1355, 558);
            tabAvanzada.TabIndex = 1;
            tabAvanzada.Text = "Avanzada";
            tabAvanzada.UseVisualStyleBackColor = true;
            // 
            // txtNoParteProv
            // 
            txtNoParteProv.BorderStyle = BorderStyle.FixedSingle;
            txtNoParteProv.Font = new Font("Segoe UI", 9.75F);
            txtNoParteProv.Location = new Point(246, 60);
            txtNoParteProv.Name = "txtNoParteProv";
            txtNoParteProv.Size = new Size(419, 29);
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
            groupBox7.Location = new Point(21, 121);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(709, 178);
            groupBox7.TabIndex = 82;
            groupBox7.TabStop = false;
            // 
            // rdNotificacionEmail
            // 
            rdNotificacionEmail.AutoSize = true;
            rdNotificacionEmail.Location = new Point(42, 39);
            rdNotificacionEmail.Name = "rdNotificacionEmail";
            rdNotificacionEmail.Size = new Size(175, 24);
            rdNotificacionEmail.TabIndex = 49;
            rdNotificacionEmail.TabStop = true;
            rdNotificacionEmail.Text = "Notificación via email";
            rdNotificacionEmail.UseVisualStyleBackColor = true;
            // 
            // txtCorreoAlterno
            // 
            txtCorreoAlterno.BorderStyle = BorderStyle.FixedSingle;
            txtCorreoAlterno.Font = new Font("Segoe UI", 9.75F);
            txtCorreoAlterno.Location = new Point(263, 107);
            txtCorreoAlterno.Name = "txtCorreoAlterno";
            txtCorreoAlterno.Size = new Size(419, 29);
            txtCorreoAlterno.TabIndex = 53;
            // 
            // rdNotificacionArchivos
            // 
            rdNotificacionArchivos.AutoSize = true;
            rdNotificacionArchivos.Location = new Point(42, 69);
            rdNotificacionArchivos.Name = "rdNotificacionArchivos";
            rdNotificacionArchivos.Size = new Size(192, 24);
            rdNotificacionArchivos.TabIndex = 50;
            rdNotificacionArchivos.Text = "Notificación via archivos";
            rdNotificacionArchivos.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(149, 110);
            label24.Name = "label24";
            label24.Size = new Size(108, 20);
            label24.TabIndex = 81;
            label24.Text = "Correo alterno:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(258, 35);
            label22.Name = "label22";
            label22.Size = new Size(145, 20);
            label22.TabIndex = 79;
            label22.Text = "Lista de distribución:";
            // 
            // cmbRutaNotificacion
            // 
            cmbRutaNotificacion.Font = new Font("Segoe UI", 9.75F);
            cmbRutaNotificacion.FormattingEnabled = true;
            cmbRutaNotificacion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbRutaNotificacion.Location = new Point(424, 69);
            cmbRutaNotificacion.Name = "cmbRutaNotificacion";
            cmbRutaNotificacion.Size = new Size(258, 29);
            cmbRutaNotificacion.TabIndex = 52;
            // 
            // cmbListaDistribucion
            // 
            cmbListaDistribucion.Font = new Font("Segoe UI", 9.75F);
            cmbListaDistribucion.FormattingEnabled = true;
            cmbListaDistribucion.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            cmbListaDistribucion.Location = new Point(424, 31);
            cmbListaDistribucion.Name = "cmbListaDistribucion";
            cmbListaDistribucion.Size = new Size(258, 29);
            cmbListaDistribucion.TabIndex = 51;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(258, 73);
            label23.Name = "label23";
            label23.Size = new Size(145, 20);
            label23.TabIndex = 80;
            label23.Text = "Ruta de notificación:";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(21, 69);
            label26.Name = "label26";
            label26.Size = new Size(199, 20);
            label26.TabIndex = 55;
            label26.Text = "Número de parte proveedor:";
            // 
            // cmbProveedor
            // 
            cmbProveedor.Font = new Font("Segoe UI", 9.75F);
            cmbProveedor.FormattingEnabled = true;
            cmbProveedor.Location = new Point(246, 20);
            cmbProveedor.Name = "cmbProveedor";
            cmbProveedor.Size = new Size(400, 29);
            cmbProveedor.TabIndex = 47;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(24, 29);
            label25.Name = "label25";
            label25.Size = new Size(80, 20);
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
            panel3.Location = new Point(0, 35);
            panel3.Name = "panel3";
            panel3.Size = new Size(1363, 65);
            panel3.TabIndex = 52;
            // 
            // cmbCliente
            // 
            cmbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbCliente.Font = new Font("Segoe UI", 9.75F);
            cmbCliente.FormattingEnabled = true;
            cmbCliente.Location = new Point(105, 16);
            cmbCliente.Name = "cmbCliente";
            cmbCliente.Size = new Size(465, 29);
            cmbCliente.TabIndex = 1;
            cmbCliente.SelectedIndexChanged += cmbCliente_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(25, 25);
            label8.Name = "label8";
            label8.Size = new Size(58, 20);
            label8.TabIndex = 34;
            label8.Text = "Cliente:";
            // 
            // cmbProyecto
            // 
            cmbProyecto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProyecto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbProyecto.Font = new Font("Segoe UI", 9.75F);
            cmbProyecto.FormattingEnabled = true;
            cmbProyecto.Location = new Point(684, 16);
            cmbProyecto.Name = "cmbProyecto";
            cmbProyecto.Size = new Size(465, 29);
            cmbProyecto.TabIndex = 2;
            cmbProyecto.SelectedIndexChanged += cmbProyecto_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(596, 20);
            label5.Name = "label5";
            label5.Size = new Size(70, 20);
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
            flowLayoutPanel1.Location = new Point(0, 691);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1363, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1178, 8);
            button2.Name = "button2";
            button2.Size = new Size(172, 35);
            button2.TabIndex = 45;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(1000, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(172, 35);
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
            panel2.Name = "panel2";
            panel2.Size = new Size(1363, 35);
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
            label1.Size = new Size(119, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo artcículo";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1327, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 10, 0, 0);
            pictureBox2.Size = new Size(36, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevoArticulo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1365, 748);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
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