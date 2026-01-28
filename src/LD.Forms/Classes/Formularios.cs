using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LD.Forms;

namespace LD.Classes
{
    public class Formularios
    {
        private Label lTitle;
        private Panel pCenter;
        private FlowLayoutPanel panelPestana;
        private Form frmPrincipal;
        private FrmClientes frmClientes;
        private FrmProyectos frmProyectos;
        private FrmAlmacenes frmAlmacenes;
        private FrmUbicaciones frmUbicaciones;
        private FrmArticulos frmArticulos;
        private FrmMovimientos frmMovimientos;

        // Menus 
        private FrmMenu frmMenu;


        public void openChildForm(string nombreFormulario)
        {
            Form childForm = new Form();
            switch (nombreFormulario)
            {
                case "Menu":
                    if (frmMenu == null)
                    {
                        frmMenu = new FrmMenu(this);
                    }
                    childForm = frmMenu;
                    this.lTitle.Text = "LMS 2.0 - Menú";
                    break;
                case "Clientes":
                    if (frmClientes == null)
                    {
                        frmClientes = new FrmClientes(this);
                    }
                    childForm = frmClientes;
                    this.lTitle.Text = "LMS 2.0 - Clientes";
                    break;
                case "Proyectos":
                    if (frmProyectos == null)
                    {
                        frmProyectos = new FrmProyectos(this);
                    }
                    childForm = frmProyectos;
                    this.lTitle.Text = "LMS 2.0 - Proyectos";
                    break;
                case "Almacenes":
                    if (frmAlmacenes == null)
                    {
                        frmAlmacenes = new FrmAlmacenes(this);
                    }
                    childForm = frmAlmacenes;
                    this.lTitle.Text = "LMS 2.0 - Almacenes";
                    break;
                case "Ubicaciones":
                    if (frmUbicaciones == null)
                    {
                        frmUbicaciones = new FrmUbicaciones(this);
                    }
                    childForm = frmUbicaciones;
                    this.lTitle.Text = "LMS 2.0 - Ubicaciones";
                    break;
                case "Articulos":
                    if (frmArticulos == null)
                    {
                        frmArticulos = new FrmArticulos(this);
                    }
                    childForm = frmArticulos;
                    this.lTitle.Text = "LMS 2.0 - Articulos";
                    break;
                case "Movimientos":
                    if (frmMovimientos == null)
                    {
                        frmMovimientos = new FrmMovimientos(this);
                    }
                    childForm = frmMovimientos;
                    this.lTitle.Text = "LMS 2.0 - Movimientos";
                    break;
               
            }

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pCenter.Controls.Add(childForm);
            pCenter.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();



            Boolean encontroPestana = false;
            string nn = nombreFormulario;
            nn = nn.Replace("á", "a");
            nn = nn.Replace("é", "e");
            nn = nn.Replace("í", "i");
            nn = nn.Replace("ó", "o");
            nn = nn.Replace("ú", "u");
            nn = nn.Replace("-", "");
            nn = nn.Trim();



            foreach (Panel p in this.panelPestana.Controls)
            {
                p.BackColor = System.Drawing.Color.LightSeaGreen;
                if (p.Name.Equals(nn))
                {
                    encontroPestana = true;
                }
            }


            if (!encontroPestana) // sino encontró pestaña  , creamos una
            {
                Panel panelPest = new System.Windows.Forms.Panel();
                Label lblPest = new System.Windows.Forms.Label();
                PictureBox picPest = new System.Windows.Forms.PictureBox();
                // 
                // panelPest
                // 


                panelPest.BackColor = System.Drawing.Color.SeaGreen;
                panelPest.Controls.Add(lblPest);
                if (!nn.Equals("Menu"))
                {
                    panelPest.Controls.Add(picPest);
                }

                panelPest.Location = new System.Drawing.Point(3, 3);
                panelPest.Name = nn;
                panelPest.Size = new System.Drawing.Size(166, 30);
                panelPest.AccessibleDescription = nombreFormulario;
                panelPest.TabIndex = 15;
                panelPest.Cursor = System.Windows.Forms.Cursors.Hand;

                // 
                // lblPest
                // 

                // 
                // picPest
                // 
                picPest.BackColor = System.Drawing.Color.Transparent;
                picPest.Cursor = System.Windows.Forms.Cursors.Hand;
                picPest.Image = global::LD.Properties.Resources.cerr;
                picPest.Location = new System.Drawing.Point(6, 3);
                picPest.Name = "picPest";
                picPest.AccessibleDescription = nombreFormulario;
                picPest.Size = new System.Drawing.Size(32, 23);
                picPest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
                picPest.TabIndex = 10;
                picPest.TabStop = false;
                picPest.Click += new System.EventHandler(this.imgPest_Click);


                lblPest.AutoSize = true;
                lblPest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblPest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
                lblPest.Location = new System.Drawing.Point(35, 9);
                lblPest.Name = "lblPest";
                lblPest.Size = new System.Drawing.Size(92, 16);
                lblPest.TabIndex = 15;
                lblPest.Text = nombreFormulario;
                lblPest.Cursor = System.Windows.Forms.Cursors.Hand;
                lblPest.AccessibleDescription = nombreFormulario;
                lblPest.Click += new System.EventHandler(this.lblPest_Click);

                panelPest.Click += new System.EventHandler(this.picPest_Click);
                this.panelPestana.Controls.Add(panelPest);


            }
            else
            {

                foreach (Panel p in this.panelPestana.Controls)
                {
                    p.BackColor = System.Drawing.Color.LightSeaGreen;

                    if (p.AccessibleDescription.Equals(nombreFormulario))
                    {
                        p.BackColor = System.Drawing.Color.SeaGreen;
                    }

                }


            }
        }


        public void inicia(Panel p, Form fP, Label l, FlowLayoutPanel panelPestana)
        {
            this.pCenter = p;
            this.frmPrincipal = fP;
            this.lTitle = l;
            this.panelPestana = panelPestana;



        }
        private void picPest_Click(object sender, EventArgs e)
        {
            Panel pPes = (Panel)sender;
            openChildForm(pPes.AccessibleDescription);

        }


        private void lblPest_Click(object sender, EventArgs e)
        {
            Label pPes = (Label)sender;
            openChildForm(pPes.AccessibleDescription);

        }

        private void imgPest_Click(object sender, EventArgs e)
        {
            PictureBox pPes = (PictureBox)sender;
            String nombreUltimo = "";
            Boolean encontrado = false;
            foreach (Panel p in this.panelPestana.Controls)
            {
                p.BackColor = System.Drawing.Color.LightSeaGreen;


                if (p.AccessibleDescription.Equals(pPes.AccessibleDescription) && !p.AccessibleDescription.Equals("Menu"))
                {
                    encontrado = true;
                    p.Dispose();

                }
                if (!encontrado)
                {
                    nombreUltimo = p.AccessibleDescription;
                }

            }
            if (encontrado)
            {
                if (this.panelPestana.Controls.Count == 0)
                {
                    openChildForm("Menu");
                }
                else if (nombreUltimo.Length > 0)
                {
                    openChildForm(nombreUltimo);
                }
            }



        }



    }
}
