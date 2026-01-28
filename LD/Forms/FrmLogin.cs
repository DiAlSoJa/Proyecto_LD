using LD.Dialogs;
using LD.Forms;

namespace LD
{
    public partial class FrmLogin : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        // private Usuarios usuarios;
        //private Conexion con;
        private bool bloqueo;
        private bool validacionForzoza;
        private Boolean respuesta;
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (this.bloqueo)
            {
                this.setRespuesta(false);
                if (this.validacionForzoza == false)
                {
                    this.Close();
                }
                else
                {
                    FrmConfirm frmConfirm = new FrmConfirm("¿Está seguro de cerrar el sistema?");
                    frmConfirm.ShowDialog();
                    if (frmConfirm.getRespuesta())
                    {
                        this.setRespuesta(true);
                        Application.Exit();
                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void valida()
        {
            String usuario = txtUsuario.Text;
            String password = txtPassword.Text;

            this.Hide();
            FrmPrincipal fm = new FrmPrincipal();
            fm.FormClosed += new FormClosedEventHandler(pr_FormClosed);
            fm.Show();

            /*
            if (usuario.Equals("") || password.Equals(""))
            {
                FrmWarning f = new FrmWarning("Ingrese usuario y contraseña");
                f.ShowDialog();
            }
            else
            {
               /* this.usuarios = new Usuarios(usuario, password);
                if (this.bloqueo)
                {
                    this.usuarios.setConexion(this.con);
                    if (this.usuarios.validaSesion())
                    {
                        this.setRespuesta(true);
                        this.Close();
                        return;
                    }
                    else
                    {
                        FrmError frmError = new FrmError("Error en usuario y contraseña");
                        frmError.ShowDialog();
                        this.setRespuesta(false);
                        return;
                    }
                }
                else
                {
                    this.usuarios = this.usuarios.login();
                    if (!this.usuarios.Valido)
                    {
                        FrmError f = new FrmError(this.usuarios.Error);
                        f.ShowDialog();
                        return;
                    }
                    this.usuarios.setConexion(this.con);
                    // buscamos los formularios de plus
                    this.usuarios.buscaFormulariosPlus();
                }
             
                //si seleccionó recordar
                if (chkRecordar.Checked)
                {
                    Regedit regedit = new Regedit();
                    regedit.IdUsuario = txtUsuario.Text;
                    regedit.escribirRecordar();
                }*/

               /* this.Hide();
                FrmPrincipal fm = new FrmPrincipal();                
                fm.FormClosed += new FormClosedEventHandler(pr_FormClosed);
                fm.Show();
            }*/
        }

        void pr_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (this.bloqueo)
            {
                this.setRespuesta(false);
                this.Close();
            }
            else
            {
                Application.Exit();
            }
        }




        private void setRespuesta(Boolean respuesta)
        {
            this.respuesta = respuesta;
        }
        public Boolean getRespuesta()
        {
            return this.respuesta;
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            valida();
        }
    }
}
