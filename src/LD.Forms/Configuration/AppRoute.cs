using LD.Forms.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Configuration
{

    // Route = metadata para tabs + navegación (sin strings mágicos en la UI)
    public sealed record AppRoute(string Key, string Title, Type FormType, bool Closable = true)
    {
        public static AppRoute For<TForm>(string key, string title, bool closable = true)
            where TForm : Form
            => new(key, title, typeof(TForm), closable);
    }

    public static class AppRoutes
    {
        public static readonly AppRoute Menu =
            AppRoute.For<FrmMenu>("menu", "Menú", closable: false);

        public static readonly AppRoute Clientes =
            AppRoute.For<FrmClientes>("clientes", "Clientes");

        public static readonly AppRoute Proyectos =
            AppRoute.For<FrmProyectos>("proyectos", "Proyectos");

        public static readonly AppRoute Almacenes =
            AppRoute.For<FrmAlmacenes>("almacenes", "Almacenes");

        public static readonly AppRoute Ubicaciones =
            AppRoute.For<FrmUbicaciones>("ubicaciones", "Ubicaciones");

        public static readonly AppRoute Articulos =
            AppRoute.For<FrmArticulos>("articulos", "Artículos");

        public static readonly AppRoute Movimientos =
            AppRoute.For<FrmMovimientos>("movimientos", "Movimientos");

        public static readonly AppRoute Inventario =
            AppRoute.For<FrmInventario>("inventario", "Inventario");

        public static readonly AppRoute Aleatorio =
            AppRoute.For<FrmAleatorio>("inventario_ciclico", "Inventario Cíclico");

        public static readonly AppRoute Usuarios =
            AppRoute.For<FrmUsuarios>("usuarios", "Usuarios");

        public static readonly AppRoute Auditar =
            AppRoute.For<FrmAuditar>("auditar", "Auditar");

        public static readonly AppRoute ASN =
            AppRoute.For<FrmASN>("asn", "ASN");

    }

}
