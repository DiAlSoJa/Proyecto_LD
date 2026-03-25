using Microsoft.Extensions.DependencyInjection;

namespace MauiAppLogin
{
    public partial class App : Application
    {
        private readonly AppShell _serviceProvider;
        public App(AppShell serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_serviceProvider);
        }
    }
}