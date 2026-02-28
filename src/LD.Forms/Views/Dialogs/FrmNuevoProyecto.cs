using LD.Contracts.Item;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Forms.Classes.DTOs;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoProyecto : DraggableForm
    {
      
        private readonly ProjectService _projectService;
        private  ProjectDto? ProjectSelected;
        public FrmNuevoProyecto(ProjectService projectService)
        {
            InitializeComponent();
            _projectService = projectService;
            EnableDrag(panel2);
            EnableDrag(panel1);

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

        private Task<ApiResponseDto<string>> CreateClient(ProjectRequest request) =>
          _projectService.CreateProject(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, ProjectRequest request) =>
            _projectService.UpdateProject(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(ProjectRequest request)
        {
            return ProjectSelected != null
                ? await EditClient(ProjectSelected?.ProjectId ?? 0, request)
                : await CreateClient(request);
        }
        private ProjectRequest BuildRequest()
        {
            return new ProjectRequest
            {
             
            };
        }
        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveClient(request);

                ShowResult(result);

                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    }
}
