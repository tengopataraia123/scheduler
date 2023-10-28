using System;
using System.ComponentModel.DataAnnotations;

namespace MainServer.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}

