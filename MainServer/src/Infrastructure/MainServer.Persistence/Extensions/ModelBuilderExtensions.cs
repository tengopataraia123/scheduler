using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MainServer.Domain.Programs;

namespace MainServer.Persistence.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProgramEntity>().HasData(
                new ProgramEntity { Id = 1, Code="SoftEng1357Spring001", Url = "localhost/api/programs/SoftEng", Name = "SoftEng" }
            );
        }
    }
}

