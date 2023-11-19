﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramServer.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}

