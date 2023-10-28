using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramServer.Application.Settings
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        string PasswordHashSecret {get; set;}
    }
}
