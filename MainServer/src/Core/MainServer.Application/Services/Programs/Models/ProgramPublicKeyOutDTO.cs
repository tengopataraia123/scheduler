﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Application.Services.Programs.Models
{
    public class ProgramPublicKeyOutDTO
    {
        public string PublicKey { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
    }
}
