using LD.Dialogs;
using LD.Forms;
using LD.Forms.Classes;
using LD.Forms.Exceptions;
using LD.Forms.Services;
using System.Text.Json;
using System.Threading.Tasks;

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
        
        private AuthService _authService;


        public FrmLogin()
        {
            InitializeComponent();
            _authService = new AuthService();
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

        private async Task valida()
        {
            try
            {
                String usuario = txtUsuario.Text;
                String password = txtPassword.Text;

                if (usuario.Equals("") || password.Equals(""))
                {
                    FrmWarning f = new FrmWarning("Ingrese usuario y contraseña");
                    f.ShowDialog();
                    return;
                }


                var response = await _authService.LoginAsync(usuario, password);
                if (!response.Success)
                {
                    FrmWarning f = new FrmWarning(response.Message);
                    f.ShowDialog();
                    return;
                }

                UserSession.AccessToken = response.Data;
                this.Hide();
                FrmPrincipal fm = new FrmPrincipal();
                fm.FormClosed += new FormClosedEventHandler(pr_FormClosed);
                fm.Show();
            }
            catch (ApiException ex)
            {
                FrmError frmError = new FrmError("Hubo un error inesperado");
                frmError.ShowDialog();
                this.setRespuesta(false);
            }
            catch(Exception ex)
            {
                FrmError frmError = new FrmError("Hubo un error inesperado");
                frmError.ShowDialog();
                this.setRespuesta(false);
                
            }
          

         





            /*
            
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
