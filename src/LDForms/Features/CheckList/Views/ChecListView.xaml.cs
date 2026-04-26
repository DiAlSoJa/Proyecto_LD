using LD.FormsX.Views.CheckList.Tabs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList
{
    public partial class CheckListView : UserControl
    {
        public CheckListView(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            EquiposTabHost.Content              = serviceProvider.GetRequiredService<EquiposTab>();
            ResumenTabHost.Content              = serviceProvider.GetRequiredService<ResumenTab>();
            ResumenBateriasTabHost.Content      = serviceProvider.GetRequiredService<ResumenBateriasTab>();
            ConfiguracionPreguntasTabHost.Content = serviceProvider.GetRequiredService<ConfiguracionPreguntasTab>();
        }
    }
}
