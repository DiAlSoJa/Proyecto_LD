namespace LD.Forms.Views.Dialogs
{
    partial class FrmNuevoProyecto
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
            tab = new TabControl();
            tabDatosGenerales = new TabPage();
            checkAutoPicking = new CheckBox();
            groupBox4 = new GroupBox();
            comboSalida = new ComboBox();
            label8 = new Label();
            comboRetrabajo = new ComboBox();
            label7 = new Label();
            comboAlmacenamiento = new ComboBox();
            label6 = new Label();
            comboEntrada = new ComboBox();
            label5 = new Label();
            groupBox3 = new GroupBox();
            checkEtiquetas = new CheckBox();
            checkSobredimension = new CheckBox();
            checkAlmacenFiscal = new CheckBox();
            checkDistribucion = new CheckBox();
            checkBackoder = new CheckBox();
            groupBox2 = new GroupBox();
            radioCaducidad = new RadioButton();
            radioNumeroLote = new RadioButton();
            radioLifo = new RadioButton();
            radioFifo = new RadioButton();
            checkActivo = new CheckBox();
            comboAlmacen = new ComboBox();
            comboCliente = new ComboBox();
            groupBox1 = new GroupBox();
            button3 = new Button();
            button1 = new Button();
            dtCamposCliente = new DataGridView();
            Orden = new DataGridViewTextBoxColumn();
            Campod = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            Cnf = new DataGridViewComboBoxColumn();
            Valorp = new DataGridViewTextBoxColumn();
            Configgg = new DataGridViewComboBoxColumn();
            dtCamposSistema = new DataGridView();
            Campo = new DataGridViewTextBoxColumn();
            label2 = new Label();
            label4 = new Label();
            txtProjectName = new TextBox();
            label3 = new Label();
            tabNotificaciones = new TabPage();
            groupBox6 = new GroupBox();
            textTiempoUrgente = new TextBox();
            label9 = new Label();
            textTiempoNormal = new TextBox();
            label12 = new Label();
            groupBox5 = new GroupBox();
            comboNotInterna = new ComboBox();
            comboNotEmbarque = new ComboBox();
            comboNotRecibo = new ComboBox();
            checkNotInterna = new CheckBox();
            checkNotEmbarque = new CheckBox();
            checkNotRecibo = new CheckBox();
            tabPrefijos = new TabPage();
            groupBox9 = new GroupBox();
            checkRegistroRequerido = new CheckBox();
            textNumeroOrdenEntrega = new TextBox();
            label15 = new Label();
            label16 = new Label();
            textPrefijoOrdenEntrega = new TextBox();
            groupBox8 = new GroupBox();
            textNumeroKitting = new TextBox();
            label13 = new Label();
            label14 = new Label();
            textPrefijoKitting = new TextBox();
            groupBox7 = new GroupBox();
            textNumeroAsn = new TextBox();
            label10 = new Label();
            label11 = new Label();
            textPrefijoAsn = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            button2 = new Button();
            btnSave = new Button();
            panel2 = new Panel();
            label1 = new Label();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            tab.SuspendLayout();
            tabDatosGenerales.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dtCamposCliente).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dtCamposSistema).BeginInit();
            tabNotificaciones.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            tabPrefijos.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(tab);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1385, 764);
            panel1.TabIndex = 0;
            // 
            // tab
            // 
            tab.Controls.Add(tabDatosGenerales);
            tab.Controls.Add(tabNotificaciones);
            tab.Controls.Add(tabPrefijos);
            tab.Dock = DockStyle.Fill;
            tab.Location = new Point(0, 35);
            tab.Name = "tab";
            tab.SelectedIndex = 0;
            tab.Size = new Size(1383, 672);
            tab.TabIndex = 37;
            // 
            // tabDatosGenerales
            // 
            tabDatosGenerales.Controls.Add(checkAutoPicking);
            tabDatosGenerales.Controls.Add(groupBox4);
            tabDatosGenerales.Controls.Add(groupBox3);
            tabDatosGenerales.Controls.Add(groupBox2);
            tabDatosGenerales.Controls.Add(checkActivo);
            tabDatosGenerales.Controls.Add(comboAlmacen);
            tabDatosGenerales.Controls.Add(comboCliente);
            tabDatosGenerales.Controls.Add(groupBox1);
            tabDatosGenerales.Controls.Add(label2);
            tabDatosGenerales.Controls.Add(label4);
            tabDatosGenerales.Controls.Add(txtProjectName);
            tabDatosGenerales.Controls.Add(label3);
            tabDatosGenerales.Location = new Point(4, 29);
            tabDatosGenerales.Name = "tabDatosGenerales";
            tabDatosGenerales.Padding = new Padding(3);
            tabDatosGenerales.Size = new Size(1375, 639);
            tabDatosGenerales.TabIndex = 0;
            tabDatosGenerales.Text = "Datos Generales";
            tabDatosGenerales.UseVisualStyleBackColor = true;
            // 
            // checkAutoPicking
            // 
            checkAutoPicking.AutoSize = true;
            checkAutoPicking.Location = new Point(423, 129);
            checkAutoPicking.Name = "checkAutoPicking";
            checkAutoPicking.Size = new Size(110, 24);
            checkAutoPicking.TabIndex = 5;
            checkAutoPicking.Text = "AutoPicking";
            checkAutoPicking.UseVisualStyleBackColor = true;
            checkAutoPicking.CheckedChanged += checkAutoPicking_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(comboSalida);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(comboRetrabajo);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(comboAlmacenamiento);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(comboEntrada);
            groupBox4.Controls.Add(label5);
            groupBox4.Location = new Point(493, 168);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(423, 181);
            groupBox4.TabIndex = 42;
            groupBox4.TabStop = false;
            groupBox4.Text = "Unidades";
            // 
            // comboSalida
            // 
            comboSalida.Font = new Font("Segoe UI", 9.75F);
            comboSalida.FormattingEnabled = true;
            comboSalida.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            comboSalida.Location = new Point(190, 126);
            comboSalida.Name = "comboSalida";
            comboSalida.Size = new Size(185, 29);
            comboSalida.TabIndex = 18;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(52, 135);
            label8.Name = "label8";
            label8.Size = new Size(53, 20);
            label8.TabIndex = 50;
            label8.Text = "Salida:";
            // 
            // comboRetrabajo
            // 
            comboRetrabajo.Font = new Font("Segoe UI", 9.75F);
            comboRetrabajo.FormattingEnabled = true;
            comboRetrabajo.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            comboRetrabajo.Location = new Point(190, 91);
            comboRetrabajo.Name = "comboRetrabajo";
            comboRetrabajo.Size = new Size(185, 29);
            comboRetrabajo.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(52, 101);
            label7.Name = "label7";
            label7.Size = new Size(77, 20);
            label7.TabIndex = 48;
            label7.Text = "Retrabajo:";
            // 
            // comboAlmacenamiento
            // 
            comboAlmacenamiento.Font = new Font("Segoe UI", 9.75F);
            comboAlmacenamiento.FormattingEnabled = true;
            comboAlmacenamiento.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            comboAlmacenamiento.Location = new Point(190, 55);
            comboAlmacenamiento.Name = "comboAlmacenamiento";
            comboAlmacenamiento.Size = new Size(185, 29);
            comboAlmacenamiento.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(52, 65);
            label6.Name = "label6";
            label6.Size = new Size(125, 20);
            label6.TabIndex = 46;
            label6.Text = "Almacenamiento:";
            // 
            // comboEntrada
            // 
            comboEntrada.Font = new Font("Segoe UI", 9.75F);
            comboEntrada.FormattingEnabled = true;
            comboEntrada.Items.AddRange(new object[] { "PALLET", "PIEZA" });
            comboEntrada.Location = new Point(190, 21);
            comboEntrada.Name = "comboEntrada";
            comboEntrada.Size = new Size(185, 29);
            comboEntrada.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(52, 30);
            label5.Name = "label5";
            label5.Size = new Size(63, 20);
            label5.TabIndex = 22;
            label5.Text = "Entrada:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkEtiquetas);
            groupBox3.Controls.Add(checkSobredimension);
            groupBox3.Controls.Add(checkAlmacenFiscal);
            groupBox3.Controls.Add(checkDistribucion);
            groupBox3.Controls.Add(checkBackoder);
            groupBox3.Location = new Point(287, 168);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(186, 181);
            groupBox3.TabIndex = 41;
            groupBox3.TabStop = false;
            groupBox3.Text = "Almacenamiento";
            // 
            // checkEtiquetas
            // 
            checkEtiquetas.AutoSize = true;
            checkEtiquetas.Location = new Point(33, 147);
            checkEtiquetas.Name = "checkEtiquetas";
            checkEtiquetas.Size = new Size(92, 24);
            checkEtiquetas.TabIndex = 14;
            checkEtiquetas.Text = "Etiquetas";
            checkEtiquetas.UseVisualStyleBackColor = true;
            // 
            // checkSobredimension
            // 
            checkSobredimension.AutoSize = true;
            checkSobredimension.Location = new Point(33, 117);
            checkSobredimension.Name = "checkSobredimension";
            checkSobredimension.Size = new Size(139, 24);
            checkSobredimension.TabIndex = 13;
            checkSobredimension.Text = "Sobredimensión";
            checkSobredimension.UseVisualStyleBackColor = true;
            // 
            // checkAlmacenFiscal
            // 
            checkAlmacenFiscal.AutoSize = true;
            checkAlmacenFiscal.Location = new Point(33, 85);
            checkAlmacenFiscal.Name = "checkAlmacenFiscal";
            checkAlmacenFiscal.Size = new Size(129, 24);
            checkAlmacenFiscal.TabIndex = 12;
            checkAlmacenFiscal.Text = "Almacén Fiscal";
            checkAlmacenFiscal.UseVisualStyleBackColor = true;
            // 
            // checkDistribucion
            // 
            checkDistribucion.AutoSize = true;
            checkDistribucion.Location = new Point(33, 55);
            checkDistribucion.Name = "checkDistribucion";
            checkDistribucion.Size = new Size(111, 24);
            checkDistribucion.TabIndex = 11;
            checkDistribucion.Text = "Distribución";
            checkDistribucion.UseVisualStyleBackColor = true;
            // 
            // checkBackoder
            // 
            checkBackoder.AutoSize = true;
            checkBackoder.Location = new Point(33, 27);
            checkBackoder.Name = "checkBackoder";
            checkBackoder.Size = new Size(93, 24);
            checkBackoder.TabIndex = 10;
            checkBackoder.Text = "Backoder";
            checkBackoder.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioCaducidad);
            groupBox2.Controls.Add(radioNumeroLote);
            groupBox2.Controls.Add(radioLifo);
            groupBox2.Controls.Add(radioFifo);
            groupBox2.Location = new Point(26, 168);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(242, 181);
            groupBox2.TabIndex = 40;
            groupBox2.TabStop = false;
            groupBox2.Text = "Tipo de almacenamiento";
            // 
            // radioCaducidad
            // 
            radioCaducidad.AutoSize = true;
            radioCaducidad.Location = new Point(25, 117);
            radioCaducidad.Name = "radioCaducidad";
            radioCaducidad.Size = new Size(164, 24);
            radioCaducidad.TabIndex = 9;
            radioCaducidad.TabStop = true;
            radioCaducidad.Text = "Fecha de Caducidad";
            radioCaducidad.UseVisualStyleBackColor = true;
            // 
            // radioNumeroLote
            // 
            radioNumeroLote.AutoSize = true;
            radioNumeroLote.Location = new Point(25, 86);
            radioNumeroLote.Name = "radioNumeroLote";
            radioNumeroLote.Size = new Size(138, 24);
            radioNumeroLote.TabIndex = 8;
            radioNumeroLote.TabStop = true;
            radioNumeroLote.Text = "Número de Lote";
            radioNumeroLote.UseVisualStyleBackColor = true;
            // 
            // radioLifo
            // 
            radioLifo.AutoSize = true;
            radioLifo.Location = new Point(25, 55);
            radioLifo.Name = "radioLifo";
            radioLifo.Size = new Size(184, 24);
            radioLifo.TabIndex = 7;
            radioLifo.TabStop = true;
            radioLifo.Text = "LIFO (Last In - First Out)";
            radioLifo.UseVisualStyleBackColor = true;
            // 
            // radioFifo
            // 
            radioFifo.AutoSize = true;
            radioFifo.Location = new Point(25, 26);
            radioFifo.Name = "radioFifo";
            radioFifo.Size = new Size(185, 24);
            radioFifo.TabIndex = 6;
            radioFifo.TabStop = true;
            radioFifo.Text = "FIFO (First In - First Out)";
            radioFifo.UseVisualStyleBackColor = true;
            // 
            // checkActivo
            // 
            checkActivo.AutoSize = true;
            checkActivo.Location = new Point(178, 129);
            checkActivo.Name = "checkActivo";
            checkActivo.Size = new Size(73, 24);
            checkActivo.TabIndex = 4;
            checkActivo.Text = "Activo";
            checkActivo.UseVisualStyleBackColor = true;
            // 
            // comboAlmacen
            // 
            comboAlmacen.FormattingEnabled = true;
            comboAlmacen.Location = new Point(178, 95);
            comboAlmacen.Name = "comboAlmacen";
            comboAlmacen.Size = new Size(235, 28);
            comboAlmacen.TabIndex = 3;
            // 
            // comboCliente
            // 
            comboCliente.FormattingEnabled = true;
            comboCliente.Location = new Point(178, 26);
            comboCliente.Name = "comboCliente";
            comboCliente.Size = new Size(527, 28);
            comboCliente.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(dtCamposCliente);
            groupBox1.Controls.Add(dtCamposSistema);
            groupBox1.Location = new Point(26, 366);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1201, 245);
            groupBox1.TabIndex = 36;
            groupBox1.TabStop = false;
            groupBox1.Text = "Escaneo";
            // 
            // button3
            // 
            button3.Location = new Point(280, 69);
            button3.Name = "button3";
            button3.Size = new Size(48, 37);
            button3.TabIndex = 5;
            button3.Text = "<";
            button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(280, 26);
            button1.Name = "button1";
            button1.Size = new Size(48, 37);
            button1.TabIndex = 4;
            button1.Text = ">";
            button1.UseVisualStyleBackColor = true;
            // 
            // dtCamposCliente
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(253, 252, 213);
            dtCamposCliente.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dtCamposCliente.BackgroundColor = SystemColors.ButtonHighlight;
            dtCamposCliente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtCamposCliente.Columns.AddRange(new DataGridViewColumn[] { Orden, Campod, dataGridViewTextBoxColumn1, Cnf, Valorp, Configgg });
            dtCamposCliente.Location = new Point(342, 26);
            dtCamposCliente.Name = "dtCamposCliente";
            dtCamposCliente.RowHeadersWidth = 51;
            dtCamposCliente.Size = new Size(834, 183);
            dtCamposCliente.TabIndex = 3;
            // 
            // Orden
            // 
            Orden.HeaderText = "Orden";
            Orden.MinimumWidth = 6;
            Orden.Name = "Orden";
            Orden.Width = 60;
            // 
            // Campod
            // 
            Campod.HeaderText = "Campo del sistema";
            Campod.MinimumWidth = 6;
            Campod.Name = "Campod";
            Campod.Width = 125;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Campos del cliente";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 200;
            // 
            // Cnf
            // 
            Cnf.HeaderText = "Configuración de escaneo";
            Cnf.MinimumWidth = 6;
            Cnf.Name = "Cnf";
            Cnf.Resizable = DataGridViewTriState.True;
            Cnf.SortMode = DataGridViewColumnSortMode.Automatic;
            Cnf.Width = 125;
            // 
            // Valorp
            // 
            Valorp.HeaderText = "Valor para escaneo";
            Valorp.MinimumWidth = 6;
            Valorp.Name = "Valorp";
            Valorp.Width = 125;
            // 
            // Configgg
            // 
            Configgg.HeaderText = "Configuración de guardado";
            Configgg.MinimumWidth = 6;
            Configgg.Name = "Configgg";
            Configgg.Resizable = DataGridViewTriState.True;
            Configgg.SortMode = DataGridViewColumnSortMode.Automatic;
            Configgg.Width = 125;
            // 
            // dtCamposSistema
            // 
            dataGridViewCellStyle2.BackColor = Color.FromArgb(253, 252, 213);
            dtCamposSistema.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dtCamposSistema.BackgroundColor = SystemColors.ButtonHighlight;
            dtCamposSistema.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dtCamposSistema.Columns.AddRange(new DataGridViewColumn[] { Campo });
            dtCamposSistema.Location = new Point(11, 26);
            dtCamposSistema.Name = "dtCamposSistema";
            dtCamposSistema.RowHeadersWidth = 51;
            dtCamposSistema.Size = new Size(263, 183);
            dtCamposSistema.TabIndex = 2;
            // 
            // Campo
            // 
            Campo.HeaderText = "Campos del sistema";
            Campo.MinimumWidth = 6;
            Campo.Name = "Campo";
            Campo.Width = 200;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 32);
            label2.Name = "label2";
            label2.Size = new Size(58, 20);
            label2.TabIndex = 21;
            label2.Text = "Cliente:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 103);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 25;
            label4.Text = "Almacén:";
            // 
            // txtProjectName
            // 
            txtProjectName.BorderStyle = BorderStyle.FixedSingle;
            txtProjectName.Font = new Font("Segoe UI", 9.75F);
            txtProjectName.Location = new Point(178, 60);
            txtProjectName.Name = "txtProjectName";
            txtProjectName.Size = new Size(527, 29);
            txtProjectName.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 67);
            label3.Name = "label3";
            label3.Size = new Size(70, 20);
            label3.TabIndex = 23;
            label3.Text = "Proyecto:";
            // 
            // tabNotificaciones
            // 
            tabNotificaciones.Controls.Add(groupBox6);
            tabNotificaciones.Controls.Add(groupBox5);
            tabNotificaciones.Location = new Point(4, 29);
            tabNotificaciones.Name = "tabNotificaciones";
            tabNotificaciones.Padding = new Padding(3);
            tabNotificaciones.Size = new Size(1375, 639);
            tabNotificaciones.TabIndex = 1;
            tabNotificaciones.Text = "Notificaciones";
            tabNotificaciones.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(textTiempoUrgente);
            groupBox6.Controls.Add(label9);
            groupBox6.Controls.Add(textTiempoNormal);
            groupBox6.Controls.Add(label12);
            groupBox6.Location = new Point(17, 215);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(890, 109);
            groupBox6.TabIndex = 43;
            groupBox6.TabStop = false;
            groupBox6.Text = "Tiempo de respuesta esperado (hrs)";
            // 
            // textTiempoUrgente
            // 
            textTiempoUrgente.BorderStyle = BorderStyle.FixedSingle;
            textTiempoUrgente.Font = new Font("Segoe UI", 9.75F);
            textTiempoUrgente.Location = new Point(440, 35);
            textTiempoUrgente.Name = "textTiempoUrgente";
            textTiempoUrgente.Size = new Size(121, 29);
            textTiempoUrgente.TabIndex = 33;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(362, 44);
            label9.Name = "label9";
            label9.Size = new Size(65, 20);
            label9.TabIndex = 45;
            label9.Text = "Urgente:";
            // 
            // textTiempoNormal
            // 
            textTiempoNormal.BorderStyle = BorderStyle.FixedSingle;
            textTiempoNormal.Font = new Font("Segoe UI", 9.75F);
            textTiempoNormal.Location = new Point(133, 35);
            textTiempoNormal.Name = "textTiempoNormal";
            textTiempoNormal.Size = new Size(121, 29);
            textTiempoNormal.TabIndex = 32;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(54, 44);
            label12.Name = "label12";
            label12.Size = new Size(62, 20);
            label12.TabIndex = 22;
            label12.Text = "Normal:";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(comboNotInterna);
            groupBox5.Controls.Add(comboNotEmbarque);
            groupBox5.Controls.Add(comboNotRecibo);
            groupBox5.Controls.Add(checkNotInterna);
            groupBox5.Controls.Add(checkNotEmbarque);
            groupBox5.Controls.Add(checkNotRecibo);
            groupBox5.Location = new Point(17, 28);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(890, 172);
            groupBox5.TabIndex = 37;
            groupBox5.TabStop = false;
            groupBox5.Text = "Notificaciones";
            // 
            // comboNotInterna
            // 
            comboNotInterna.FormattingEnabled = true;
            comboNotInterna.Location = new Point(315, 127);
            comboNotInterna.Name = "comboNotInterna";
            comboNotInterna.Size = new Size(235, 28);
            comboNotInterna.TabIndex = 31;
            // 
            // comboNotEmbarque
            // 
            comboNotEmbarque.FormattingEnabled = true;
            comboNotEmbarque.Location = new Point(315, 84);
            comboNotEmbarque.Name = "comboNotEmbarque";
            comboNotEmbarque.Size = new Size(235, 28);
            comboNotEmbarque.TabIndex = 29;
            // 
            // comboNotRecibo
            // 
            comboNotRecibo.FormattingEnabled = true;
            comboNotRecibo.Location = new Point(315, 40);
            comboNotRecibo.Name = "comboNotRecibo";
            comboNotRecibo.Size = new Size(235, 28);
            comboNotRecibo.TabIndex = 27;
            // 
            // checkNotInterna
            // 
            checkNotInterna.AutoSize = true;
            checkNotInterna.Location = new Point(48, 129);
            checkNotInterna.Name = "checkNotInterna";
            checkNotInterna.Size = new Size(162, 24);
            checkNotInterna.TabIndex = 30;
            checkNotInterna.Text = "Notificación Interna";
            checkNotInterna.UseVisualStyleBackColor = true;
            // 
            // checkNotEmbarque
            // 
            checkNotEmbarque.AutoSize = true;
            checkNotEmbarque.Location = new Point(48, 88);
            checkNotEmbarque.Name = "checkNotEmbarque";
            checkNotEmbarque.Size = new Size(205, 24);
            checkNotEmbarque.TabIndex = 28;
            checkNotEmbarque.Text = "Notificación de Embarque";
            checkNotEmbarque.UseVisualStyleBackColor = true;
            // 
            // checkNotRecibo
            // 
            checkNotRecibo.AutoSize = true;
            checkNotRecibo.Location = new Point(48, 44);
            checkNotRecibo.Name = "checkNotRecibo";
            checkNotRecibo.Size = new Size(183, 24);
            checkNotRecibo.TabIndex = 26;
            checkNotRecibo.Text = "Notificación de Recibo";
            checkNotRecibo.UseVisualStyleBackColor = true;
            // 
            // tabPrefijos
            // 
            tabPrefijos.Controls.Add(groupBox9);
            tabPrefijos.Controls.Add(groupBox8);
            tabPrefijos.Controls.Add(groupBox7);
            tabPrefijos.Location = new Point(4, 29);
            tabPrefijos.Name = "tabPrefijos";
            tabPrefijos.Padding = new Padding(3);
            tabPrefijos.Size = new Size(1375, 639);
            tabPrefijos.TabIndex = 2;
            tabPrefijos.Text = "Prefijos";
            tabPrefijos.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(checkRegistroRequerido);
            groupBox9.Controls.Add(textNumeroOrdenEntrega);
            groupBox9.Controls.Add(label15);
            groupBox9.Controls.Add(label16);
            groupBox9.Controls.Add(textPrefijoOrdenEntrega);
            groupBox9.Location = new Point(17, 171);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(897, 181);
            groupBox9.TabIndex = 55;
            groupBox9.TabStop = false;
            groupBox9.Text = "Entrega DO:";
            // 
            // checkRegistroRequerido
            // 
            checkRegistroRequerido.AutoSize = true;
            checkRegistroRequerido.Location = new Point(336, 128);
            checkRegistroRequerido.Name = "checkRegistroRequerido";
            checkRegistroRequerido.Size = new Size(226, 24);
            checkRegistroRequerido.TabIndex = 40;
            checkRegistroRequerido.Text = "Requiere registro de llegadas";
            checkRegistroRequerido.UseVisualStyleBackColor = true;
            // 
            // textNumeroOrdenEntrega
            // 
            textNumeroOrdenEntrega.BorderStyle = BorderStyle.FixedSingle;
            textNumeroOrdenEntrega.Font = new Font("Segoe UI", 9.75F);
            textNumeroOrdenEntrega.Location = new Point(336, 27);
            textNumeroOrdenEntrega.Name = "textNumeroOrdenEntrega";
            textNumeroOrdenEntrega.Size = new Size(151, 29);
            textNumeroOrdenEntrega.TabIndex = 38;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(37, 45);
            label15.Name = "label15";
            label15.Size = new Size(242, 20);
            label15.TabIndex = 45;
            label15.Text = "Número de orden de entrega (DO):";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(37, 84);
            label16.Name = "label16";
            label16.Size = new Size(260, 20);
            label16.TabIndex = 47;
            label16.Text = "Prefijo para la orden de entrega (DO):";
            // 
            // textPrefijoOrdenEntrega
            // 
            textPrefijoOrdenEntrega.BorderStyle = BorderStyle.FixedSingle;
            textPrefijoOrdenEntrega.Font = new Font("Segoe UI", 9.75F);
            textPrefijoOrdenEntrega.Location = new Point(336, 75);
            textPrefijoOrdenEntrega.Name = "textPrefijoOrdenEntrega";
            textPrefijoOrdenEntrega.Size = new Size(151, 29);
            textPrefijoOrdenEntrega.TabIndex = 39;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(textNumeroKitting);
            groupBox8.Controls.Add(label13);
            groupBox8.Controls.Add(label14);
            groupBox8.Controls.Add(textPrefijoKitting);
            groupBox8.Location = new Point(501, 28);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(414, 125);
            groupBox8.TabIndex = 54;
            groupBox8.TabStop = false;
            groupBox8.Text = "Kitting:";
            // 
            // textNumeroKitting
            // 
            textNumeroKitting.BorderStyle = BorderStyle.FixedSingle;
            textNumeroKitting.Font = new Font("Segoe UI", 9.75F);
            textNumeroKitting.Location = new Point(178, 36);
            textNumeroKitting.Name = "textNumeroKitting";
            textNumeroKitting.Size = new Size(151, 29);
            textNumeroKitting.TabIndex = 36;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(37, 45);
            label13.Name = "label13";
            label13.Size = new Size(135, 20);
            label13.TabIndex = 45;
            label13.Text = "Número de Kitting;";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(37, 84);
            label14.Name = "label14";
            label14.Size = new Size(124, 20);
            label14.TabIndex = 47;
            label14.Text = "Prefijo de Kitting:";
            // 
            // textPrefijoKitting
            // 
            textPrefijoKitting.BorderStyle = BorderStyle.FixedSingle;
            textPrefijoKitting.Font = new Font("Segoe UI", 9.75F);
            textPrefijoKitting.Location = new Point(178, 75);
            textPrefijoKitting.Name = "textPrefijoKitting";
            textPrefijoKitting.Size = new Size(151, 29);
            textPrefijoKitting.TabIndex = 37;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(textNumeroAsn);
            groupBox7.Controls.Add(label10);
            groupBox7.Controls.Add(label11);
            groupBox7.Controls.Add(textPrefijoAsn);
            groupBox7.Location = new Point(17, 28);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(414, 125);
            groupBox7.TabIndex = 53;
            groupBox7.TabStop = false;
            groupBox7.Text = "ASN";
            // 
            // textNumeroAsn
            // 
            textNumeroAsn.BorderStyle = BorderStyle.FixedSingle;
            textNumeroAsn.Font = new Font("Segoe UI", 9.75F);
            textNumeroAsn.Location = new Point(178, 36);
            textNumeroAsn.Name = "textNumeroAsn";
            textNumeroAsn.Size = new Size(151, 29);
            textNumeroAsn.TabIndex = 34;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(37, 45);
            label10.Name = "label10";
            label10.Size = new Size(120, 20);
            label10.TabIndex = 45;
            label10.Text = "Número de ASN:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(37, 84);
            label11.Name = "label11";
            label11.Size = new Size(109, 20);
            label11.TabIndex = 47;
            label11.Text = "Prefijo de ASN:";
            // 
            // textPrefijoAsn
            // 
            textPrefijoAsn.BorderStyle = BorderStyle.FixedSingle;
            textPrefijoAsn.Font = new Font("Segoe UI", 9.75F);
            textPrefijoAsn.Location = new Point(178, 75);
            textPrefijoAsn.Name = "textPrefijoAsn";
            textPrefijoAsn.Size = new Size(151, 29);
            textPrefijoAsn.TabIndex = 35;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.Controls.Add(btnSave);
            flowLayoutPanel1.Dock = DockStyle.Bottom;
            flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel1.Location = new Point(0, 707);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5);
            flowLayoutPanel1.Size = new Size(1383, 55);
            flowLayoutPanel1.TabIndex = 8;
            // 
            // button2
            // 
            button2.Image = Properties.Resources.cancel;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(1199, 8);
            button2.Name = "button2";
            button2.Size = new Size(171, 35);
            button2.TabIndex = 25;
            button2.Text = "Cerrar";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnSave
            // 
            btnSave.Image = Properties.Resources.save;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(1022, 8);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(171, 35);
            btnSave.TabIndex = 24;
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
            panel2.Size = new Size(1383, 35);
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
            label1.Size = new Size(120, 22);
            label1.TabIndex = 3;
            label1.Text = "Nuevo proyecto";
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Image = Properties.Resources.cancelar;
            pictureBox2.Location = new Point(1346, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Padding = new Padding(5, 11, 0, 0);
            pictureBox2.Size = new Size(37, 35);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // FrmNuevoProyecto
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1385, 764);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNuevoProyecto";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmWarning";
            panel1.ResumeLayout(false);
            tab.ResumeLayout(false);
            tabDatosGenerales.ResumeLayout(false);
            tabDatosGenerales.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dtCamposCliente).EndInit();
            ((System.ComponentModel.ISupportInitialize)dtCamposSistema).EndInit();
            tabNotificaciones.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabPrefijos.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
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
        private Label label4;
        private TextBox txtProjectName;
        private Label label3;
        private Label label2;
        private GroupBox groupBox1;
        private TabControl tab;
        private TabPage tabDatosGenerales;
        private TabPage tabNotificaciones;
        private TabPage tabPrefijos;
        private ComboBox comboCliente;
        private ComboBox comboAlmacen;
        private CheckBox checkActivo;
        private GroupBox groupBox2;
        private RadioButton radioCaducidad;
        private RadioButton radioNumeroLote;
        private RadioButton radioLifo;
        private RadioButton radioFifo;
        private GroupBox groupBox3;
        private CheckBox checkAlmacenFiscal;
        private CheckBox checkDistribucion;
        private CheckBox checkBackoder;
        private CheckBox checkEtiquetas;
        private CheckBox checkSobredimension;
        private GroupBox groupBox4;
        private Label label5;
        private ComboBox comboEntrada;
        private ComboBox comboSalida;
        private Label label8;
        private ComboBox comboRetrabajo;
        private Label label7;
        private ComboBox comboAlmacenamiento;
        private Label label6;
        private CheckBox checkAutoPicking;
        private GroupBox groupBox5;
        private CheckBox checkNotInterna;
        private CheckBox checkNotEmbarque;
        private CheckBox checkNotRecibo;
        private ComboBox comboNotInterna;
        private ComboBox comboNotEmbarque;
        private ComboBox comboNotRecibo;
        private GroupBox groupBox6;
        private Label label12;
        private TextBox textTiempoUrgente;
        private Label label9;
        private TextBox textTiempoNormal;
        private TextBox textPrefijoAsn;
        private Label label11;
        private TextBox textNumeroAsn;
        private Label label10;
        private GroupBox groupBox8;
        private TextBox textNumeroKitting;
        private Label label13;
        private Label label14;
        private TextBox textPrefijoKitting;
        private GroupBox groupBox7;
        private GroupBox groupBox9;
        private TextBox textNumeroOrdenEntrega;
        private Label label15;
        private Label label16;
        private TextBox textPrefijoOrdenEntrega;
        private CheckBox checkRegistroRequerido;
        private DataGridView dataGridView1;
        private DataGridView dtCamposCliente;
        private DataGridView dtCamposSistema;
        private DataGridViewTextBoxColumn Campo;
        private Button button3;
        private Button button1;
        private DataGridViewTextBoxColumn Orden;
        private DataGridViewTextBoxColumn Campod;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewComboBoxColumn Cnf;
        private DataGridViewTextBoxColumn Valorp;
        private DataGridViewComboBoxColumn Configgg;
    }
}