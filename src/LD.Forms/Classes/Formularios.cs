using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LD.Forms.Classes
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
                    lTitle.Text = "LMS 2.0 - Menú";
                    break;
                case "Clientes":
                    if (frmClientes == null)
                    {
                        frmClientes = new FrmClientes(this);
                    }
                    childForm = frmClientes;
                    lTitle.Text = "LMS 2.0 - Clientes";
                    break;
                case "Proyectos":
                    if (frmProyectos == null)
                    {
                        frmProyectos = new FrmProyectos(this);
                    }
                    childForm = frmProyectos;
                    lTitle.Text = "LMS 2.0 - Proyectos";
                    break;
                case "Almacenes":
                    if (frmAlmacenes == null)
                    {
                        frmAlmacenes = new FrmAlmacenes(this);
                    }
                    childForm = frmAlmacenes;
                    lTitle.Text = "LMS 2.0 - Almacenes";
                    break;
                case "Ubicaciones":
                    if (frmUbicaciones == null)
                    {
                        frmUbicaciones = new FrmUbicaciones(this);
                    }
                    childForm = frmUbicaciones;
                    lTitle.Text = "LMS 2.0 - Ubicaciones";
                    break;
                case "Articulos":
                    if (frmArticulos == null)
                    {
                        frmArticulos = new FrmArticulos(this);
                    }
                    childForm = frmArticulos;
                    lTitle.Text = "LMS 2.0 - Articulos";
                    break;
                case "Movimientos":
                    if (frmMovimientos == null)
                    {
                        frmMovimientos = new FrmMovimientos(this);
                    }
                    childForm = frmMovimientos;
                    lTitle.Text = "LMS 2.0 - Movimientos";
                    break;
               
            }

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pCenter.Controls.Add(childForm);
            pCenter.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();



            bool encontroPestana = false;
            string nn = nombreFormulario;
            nn = nn.Replace("á", "a");
            nn = nn.Replace("é", "e");
            nn = nn.Replace("í", "i");
            nn = nn.Replace("ó", "o");
            nn = nn.Replace("ú", "u");
            nn = nn.Replace("-", "");
            nn = nn.Trim();



            foreach (Panel p in panelPestana.Controls)
            {
                p.BackColor = Color.LightSeaGreen;
                if (p.Name.Equals(nn))
                {
                    encontroPestana = true;
                }
            }


            if (!encontroPestana) // sino encontró pestaña  , creamos una
            {
                Panel panelPest = new Panel();
                Label lblPest = new Label();
                PictureBox picPest = new PictureBox();
                // 
                // panelPest
                // 


                panelPest.BackColor = Color.SeaGreen;
                panelPest.Controls.Add(lblPest);
                if (!nn.Equals("Menu"))
                {
                    panelPest.Controls.Add(picPest);
                }

                panelPest.Location = new Point(3, 3);
                panelPest.Name = nn;
                panelPest.Size = new Size(166, 30);
                panelPest.AccessibleDescription = nombreFormulario;
                panelPest.TabIndex = 15;
                panelPest.Cursor = Cursors.Hand;

                // 
                // lblPest
                // 

                // 
                // picPest
                // 
                picPest.BackColor = Color.Transparent;
                picPest.Cursor = Cursors.Hand;
                picPest.Image = LD.Forms.Properties.Resources.cerr;
                picPest.Location = new Point(6, 3);
                picPest.Name = "picPest";
                picPest.AccessibleDescription = nombreFormulario;
                picPest.Size = new Size(32, 23);
                picPest.SizeMode = PictureBoxSizeMode.CenterImage;
                picPest.TabIndex = 10;
                picPest.TabStop = false;
                picPest.Click += new EventHandler(imgPest_Click);


                lblPest.AutoSize = true;
                lblPest.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                lblPest.ForeColor = SystemColors.ControlLightLight;
                lblPest.Location = new Point(35, 9);
                lblPest.Name = "lblPest";
                lblPest.Size = new Size(92, 16);
                lblPest.TabIndex = 15;
                lblPest.Text = nombreFormulario;
                lblPest.Cursor = Cursors.Hand;
                lblPest.AccessibleDescription = nombreFormulario;
                lblPest.Click += new EventHandler(lblPest_Click);

                panelPest.Click += new EventHandler(picPest_Click);
                panelPestana.Controls.Add(panelPest);


            }
            else
            {

                foreach (Panel p in panelPestana.Controls)
                {
                    p.BackColor = Color.LightSeaGreen;

                    if (p.AccessibleDescription.Equals(nombreFormulario))
                    {
                        p.BackColor = Color.SeaGreen;
                    }

                }


            }
        }


        public void inicia(Panel p, Form fP, Label l, FlowLayoutPanel panelPestana)
        {
            pCenter = p;
            frmPrincipal = fP;
            lTitle = l;
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
            string nombreUltimo = "";
            bool encontrado = false;
            foreach (Panel p in panelPestana.Controls)
            {
                p.BackColor = Color.LightSeaGreen;


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
                if (panelPestana.Controls.Count == 0)
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
