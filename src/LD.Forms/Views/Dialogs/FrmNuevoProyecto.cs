using LD.Contracts.Item;
using LD.Contracts.Project;
using LD.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoProyecto : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private readonly ProjectService _projectService;
        private  ProjectDto? ProjectSelected;
        public FrmNuevoProyecto(ProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
        }

        public async void SetProject(ProjectDto? project)
        {
            ProjectSelected = project;
            await CargarDatosAsync();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);



        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _projectService.GetProjectById(ProjectSelected?.ProjectId ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var client = response.Data;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
