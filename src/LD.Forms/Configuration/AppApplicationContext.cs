using LD.Forms.Services.FormServices;
using LD.Forms.Views.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Configuration
{
    public class AppApplicationContext : ApplicationContext
    {
        private readonly IServiceProvider _serviceProvider;

        private FrmLogin? _loginForm;
        private FrmPrincipal? _mainForm;
        private TabService _tabService;

        public AppApplicationContext(IServiceProvider serviceProvider,TabService tabService)
        {
            _serviceProvider = serviceProvider;
            _tabService = tabService;
            ShowLogin();
        }

        private void ShowLogin()
        {
            _loginForm = _serviceProvider.GetRequiredService<FrmLogin>();
            _loginForm.LoginSucceeded += OnLoginSucceeded;
            _loginForm.FormClosed += OnLoginClosed;
            _loginForm.Show();
        }

        private void OnLoginSucceeded(object? sender, EventArgs e)
        {
            _loginForm!.LoginSucceeded -= OnLoginSucceeded;
            _loginForm.FormClosed -= OnLoginClosed;

            _loginForm.Close();
            _loginForm.Dispose();

            ShowMain();
        }

        private void ShowMain()
        {
            _mainForm = _serviceProvider.GetRequiredService<FrmPrincipal>();
            _mainForm.LogoutRequested += OnLogoutRequested;
            _mainForm.FormClosed += OnMainClosed;
            _mainForm.Show();
        }

        private void OnLogoutRequested(object? sender, EventArgs e)
        {
            _mainForm!.LogoutRequested -= OnLogoutRequested;
            _mainForm.FormClosed -= OnMainClosed;

            _tabService.ClearAll();
            _mainForm.Close();
            _mainForm.Dispose();

            ShowLogin();
        }

        private void OnLoginClosed(object? sender, FormClosedEventArgs e)
        {
            ExitThread(); // cierra la app si cierran login sin loguearse
        }

        private void OnMainClosed(object? sender, FormClosedEventArgs e)
        {
            ExitThread(); // cierra la app si cierran principal
        }
    }
}

