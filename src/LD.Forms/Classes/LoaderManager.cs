using LD.Forms.Controls;
using LD.Forms.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Classes
{
    public static class LoaderManager
    {
        private static LoaderControl? _loader;
        private const string LoaderName = "__GLOBAL_LOADER__";
        public static void Show(Control parent, string text = "Cargando...")
        {
            if (parent == null || parent.IsDisposed) return;

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => Show(parent, text)));
                return;
            }

            // Si ya existe, solo actualiza texto y lo sube al frente
            var existing = parent.Controls.Find(LoaderName, true).FirstOrDefault() as LoaderControl;
            if (existing != null)
            {
                existing.SetText(text);
                existing.BringToFront();
                existing.Visible = true;
                return;
            }

            var loader = new LoaderControl
            {
                Name = LoaderName
            };
            loader.SetText(text);

            parent.Controls.Add(loader);
            loader.BringToFront();
            loader.Visible = true;
            loader.Focus();
        }

        public static void Hide(Control parent)
        {
            if (parent == null || parent.IsDisposed) return;

            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => Hide(parent)));
                return;
            }

            var existing = parent.Controls.Find(LoaderName, true).FirstOrDefault();
            if (existing == null) return;

            parent.Controls.Remove(existing);
            existing.Dispose();
        }

        // Bonus PRO: wrapper para no olvidar el finally
        public static async Task Run(Control parent, Func<Task> action, string text = "Cargando...")
        {
            try
            {
                Show(parent, text);
                await action();
            }
            finally
            {
                Hide(parent);
            }
        }
    }
}
