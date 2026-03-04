using LD.Forms.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services.FormServices
{

    public sealed class TabService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;

        private FlowLayoutPanel? _tabsPanel;
        private Label? _titleLabel;

        private readonly Dictionary<string, TabEntry> _tabs = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _history = new();

        public string? ActiveKey { get; private set; }

        public TabService(IServiceProvider sp, NavigationService nav)
        {
            _serviceProvider = sp;
            _navigationService = nav;
        }

        public void Initialize(FlowLayoutPanel tabsPanel, Label titleLabel)
        {
            _tabsPanel = tabsPanel ?? throw new ArgumentNullException(nameof(tabsPanel));
            _titleLabel = titleLabel ?? throw new ArgumentNullException(nameof(titleLabel));

            _tabsPanel.WrapContents = false;
            _tabsPanel.AutoScroll = true;
        }

        public void Open(AppRoute route)
        {
            EnsureInit();

            if (!_tabs.TryGetValue(route.Key, out var entry))
            {
                // Creamos una instancia del form para que el tab conserve estado 
                var form = (Form)_serviceProvider.GetRequiredService(route.FormType);

                // UI del tab
                var tabControl = CreateTabControl(route);

                entry = new TabEntry(route, form, tabControl);
                _tabs.Add(route.Key, entry);
                _tabsPanel!.Controls.Add(tabControl);
            }

            Activate(route.Key);
        }

        public void Activate(string routeKey)
        {
            EnsureInit();

            if (!_tabs.TryGetValue(routeKey, out var entry))
                return;

            // actualiza historial
            _history.Remove(routeKey);
            _history.Add(routeKey);

            ActiveKey = routeKey;

            // estilos
            foreach (Control c in _tabsPanel!.Controls)
            {
                if (c is Panel p)
                    p.BackColor = Color.LightSeaGreen;
            }
            entry.TabPanel.BackColor = Color.SeaGreen;

            // título
            _titleLabel!.Text = $"LMS 2.0 - {entry.Route.Title}";

            // navega mostrando instancia cacheada
            _navigationService.Navigate(entry.FormInstance);
        }

        public void Close(string routeKey)
        {
            EnsureInit();

            if (!_tabs.TryGetValue(routeKey, out var entry))
                return;

            if (!entry.Route.Closable)
                return;

            // quitar UI
            _tabsPanel!.Controls.Remove(entry.TabPanel);
            entry.TabPanel.Dispose();

            // quitar estado
            _tabs.Remove(routeKey);
            _history.Remove(routeKey);

            // dispose del form cacheado
            entry.FormInstance.Close();
            entry.FormInstance.Dispose();

            // navegar a anterior o menú
            var nextKey = _history.LastOrDefault();
            if (nextKey is not null && _tabs.ContainsKey(nextKey))
                Activate(nextKey);
            else
                Open(AppRoutes.Menu);
        }
        public void ClearAll()
        {
            foreach(var tab in _tabs)
            {
                _tabsPanel!.Controls.Remove(tab.Value.TabPanel);
                tab.Value.TabPanel.Dispose();

                // quitar estado
                _tabs.Remove(tab.Key);
                _history.Remove(tab.Key);

                // dispose del form cacheado
                tab.Value.FormInstance.Close();
                tab.Value.FormInstance.Dispose();
            }


        }
        private Panel CreateTabControl(AppRoute route)
        {
            // Panel base (tab)
            var panel = new Panel
            {
                Name = route.Key,
                Size = new Size(166, 30),
                BackColor = Color.SeaGreen,
                Cursor = Cursors.Hand,
                Margin = new Padding(3),
                Tag = route.Key
            };

            // Botón cerrar (sin resources; simple "x")
            var btnClose = new Label
            {
                AutoSize = false,
                Text = "×",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(22, 22),
                Location = new Point(6, 4),
                Cursor = Cursors.Hand,
                Visible = route.Closable,
                Tag = route.Key
            };

            // Texto tab
            var lbl = new Label
            {
                AutoSize = true,
                Text = route.Title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                Location = new Point(route.Closable ? 32 : 10, 7),
                Cursor = Cursors.Hand,
                Tag = route.Key
            };

            // Eventos
            panel.Click += (_, _) => Activate(route.Key);
            lbl.Click += (_, _) => Activate(route.Key);
            btnClose.Click += (_, _) => Close(route.Key);

            panel.Controls.Add(lbl);
            if (route.Closable)
                panel.Controls.Add(btnClose);

            return panel;
        }

        private void EnsureInit()
        {
            if (_tabsPanel is null || _titleLabel is null)
                throw new InvalidOperationException("TabService not initialized. Call Initialize(flowLayoutPanel, titleLabel) in FrmPrincipal.");
        }

        private sealed record TabEntry(AppRoute Route, Form FormInstance, Panel TabPanel);
    }
}
