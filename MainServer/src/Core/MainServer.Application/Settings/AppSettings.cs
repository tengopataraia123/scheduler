using System;
using System.Collections.Generic;
using System.Text;

namespace MainServer.Application.Settings
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public string PasswordHashSecret { get; set; }
    }
}
