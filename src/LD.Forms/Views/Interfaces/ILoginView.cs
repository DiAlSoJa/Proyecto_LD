using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Views.Interfaces
{
    public interface ILoginView
    {
        string Usuario { get; }
        string Password { get; }

        event EventHandler Login;
        event EventHandler Exit;
    }
}
