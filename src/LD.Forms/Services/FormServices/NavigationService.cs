using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services.FormServices
{
    public class NavigationService 
    {
        private readonly IServiceProvider _serviceProvider;
        private Panel? _container;

        public Form? CurrentForm { get; private set; }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize(Panel container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Navigate(Type formType)
        {
            if (_container is null)
                throw new InvalidOperationException("NavigationService not initialized. Call Initialize(panel) in FrmPrincipal.");

            if (formType is null)
                throw new ArgumentNullException(nameof(formType));

            if (!typeof(Form).IsAssignableFrom(formType))
                throw new ArgumentException("formType must inherit from System.Windows.Forms.Form", nameof(formType));

            var form = (Form)_serviceProvider.GetRequiredService(formType);
            Navigate(form);
        }

        public void Navigate(Form formInstance)
        {
            if (_container is null)
                throw new InvalidOperationException("NavigationService not initialized. Call Initialize(panel) in FrmPrincipal.");

            if (formInstance is null)
                throw new ArgumentNullException(nameof(formInstance));

            // Cierra/desmonta el actual
            if (CurrentForm is not null && !ReferenceEquals(CurrentForm, formInstance))
            {
                // Si el actual fue "cacheado" por tabs, NO lo dispose aquí.
                // Solo lo ocultamos y lo sacamos del container.
                CurrentForm.Hide();
            }

            _container.SuspendLayout();
            try
            {
                _container.Controls.Clear();

                formInstance.TopLevel = false;
                formInstance.FormBorderStyle = FormBorderStyle.None;
                formInstance.Dock = DockStyle.Fill;

                _container.Controls.Add(formInstance);
                _container.Tag = formInstance;

                CurrentForm = formInstance;
                formInstance.Show();
                formInstance.BringToFront();
            }
            finally
            {
                _container.ResumeLayout(true);
            }
        }
    }
}
