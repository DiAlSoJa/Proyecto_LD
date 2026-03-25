using LD.Client.Services;
using LD.Contracts.Product;
using LD.Contracts.Project;
using LD.Contracts.User;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmProyectos : Form
    {
        private Formularios formularios;
        private readonly ProjectService _projectService;
        private BindingSource _projectsBinding = new();
        private ProjectDto? selectedProject { get; set; }
        private GridFilter<ProjectDto> _gridFilter;
        private readonly DialogFormService _dialogFormService;

        public FrmProyectos(ProjectService projectService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _projectService = projectService;
            dataGridView1.DataSource = _projectsBinding;
            _dialogFormService = dialogFormService;
            _gridFilter = new GridFilter<ProjectDto>(dataGridView1, _projectsBinding);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var form= _dialogFormService.ShowDialog<FrmNuevoProyecto>();
            if (form.ResponseForm)
                await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo Proyectos");
        }
        private async void EditBtn_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoProyecto>(config =>
            {
                config.SetProject(selectedProject);
            });
            if(form.ResponseForm)
                await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo Proyectos");

        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo Proyectos");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var result = await _projectService.GetProjects();

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                _projectsBinding.DataSource = result.Data;

                _gridFilter.SetData(result.Data);
                dataGridView1 = _gridFilter.BuildFilterColumns();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var project = dataGridView1.CurrentRow.DataBoundItem as ProjectDto;

                if (project == null)
                    return;

                selectedProject = project;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
