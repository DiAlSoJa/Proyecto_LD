using LD.Contracts.Enums;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using LD.Forms.Views.Dialogs;
using LD.Forms.Views.Exceptions;
using LD.Forms.Views.Forms;
using LD.Forms.Views.Interfaces;

namespace LD.Forms.Views.Forms
{
    public partial class FrmLogin : DraggableForm, ILoginView
    {
        private bool mouseDown;
        private Point lastLocation;
        private bool bloqueo;
        private bool validacionForzoza;
        private Boolean respuesta;

        private AuthService _authService;
        private DialogMessageService _dialogMessageService;
        public event EventHandler? LoginSucceeded;

        public FrmLogin(AuthService authService, DialogMessageService dialogMessageService)
        {
            InitializeComponent();
            _authService = authService;
            _dialogMessageService = dialogMessageService;
            EnableDrag(panel1);
            EnableDrag(pictureBox1);

        }

        public string Usuario => txtUsuario.Text;
        public string Password => txtPassword.Text;


        public event EventHandler Login;
        public event EventHandler Exit;


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
                if (Usuario.Equals("") || Password.Equals(""))
                {
                    _dialogMessageService.Show("Ingrese usuario y contraseña", DialogMessageEnum.Warning);
                    return;
                }

                var response = await _authService.LoginAsync(Usuario, Password);
                if (!response.IsSuccess)
                {
                    _dialogMessageService.Show(response.Message, DialogMessageEnum.Warning);
                    return;
                }

                UserSession.AccessToken = response.Data;
                var getMeResponse = await _authService.GetMeAsync();
                if (!getMeResponse.IsSuccess || getMeResponse.Data is null)
                {
                    _dialogMessageService.Show(getMeResponse.Message, DialogMessageEnum.Warning);
                    return;
                }
                UserData.SetUserData(getMeResponse.Data);
                LoginSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _dialogMessageService.Show("Hubo un error inesperado", DialogMessageEnum.Error);

                this.setRespuesta(false);

            }
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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                btnLogin.Enabled = false;
                await LoaderManager.Run(roundedPanel1, async () =>
                {

                    await valida();
                }, "Validando credenciales");

            }
            finally
            {
                btnLogin.Enabled = true;
                LoaderManager.Hide(roundedPanel1);
            }
        }

        private void roundedPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

