using System;
using System.Collections.Generic;
using System.Text;

namespace MainServer.Application.Settings
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        string PasswordHashSecret {get; set;}
    }
}
