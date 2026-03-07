using LD.Client.Services;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Presenters
{
    public class LoginPresenter
    {
        private readonly ILoginView _view;
        private readonly AuthService _authService;

        public LoginPresenter(ILoginView view, AuthService authService)
        {
            _view = view;
            _authService = authService;

            _view.Login += OnLoginRequested;
            _view.Exit += OnExitRequested;
        }

        private async void OnLoginRequested(object? sender, EventArgs e)
        {
            try
            {

            }
            catch
            {
               
            }
            finally
            {
  
            }
        }

        private void OnExitRequested(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
