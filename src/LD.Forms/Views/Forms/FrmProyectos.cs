using LD.Contracts.Item;
using LD.Contracts.Project;
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

        public FrmProyectos(ProjectService projectService,DialogFormService dialogFormService)
        {
            InitializeComponent();
            _projectService = projectService;
            dataGridView1.DataSource = _projectsBinding;
            _dialogFormService = dialogFormService;
            _gridFilter = new GridFilter<ProjectDto>(dataGridView1, _projectsBinding);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoProyecto>();
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoProyecto>(config =>
            {
                config.SetProject(new());
            });
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

     
    }
}
