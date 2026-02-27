using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Views.Interfaces
{
    public interface ICreateUserView
    {
        public string Username { get;  }
        public string Name { get;  }
        public string Password { get; }
        public string ConfirmPassword { get;  }
        public bool IsActive { get;  }

        event EventHandler CreateUser;
        event EventHandler UpdateUser;
        event EventHandler Exit;


    }
}
