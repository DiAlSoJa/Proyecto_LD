using LD.Contracts.Enums;
using LD.Forms.Views.Common;
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

        public T ShowDialog<T>(Action<T>? configure = null) where T : DraggableForm
        {
            var form = _serviceProvider.GetRequiredService<T>();

            configure?.Invoke(form);
            form.ShowDialog();
            return form;
        }

    }
}
