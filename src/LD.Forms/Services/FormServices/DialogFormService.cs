using LD.Contracts.Enums;
using LD.Forms.Views.Dialogs;
using LD.Forms.Views.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services.FormServices
{
    public class DialogFormService
    {
        private readonly IServiceProvider _serviceProvider;
        public DialogFormService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DialogResult ShowDialog<T>(Action<T>? configure = null) where T : Form
        {
            var form = _serviceProvider.GetRequiredService<T>();

            configure?.Invoke(form);

            return form.ShowDialog();
        }

    }
}
