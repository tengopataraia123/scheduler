using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;
using MainServer.Domain.Programs;
using MainServer.Persistence.Extensions;
using MainServer.Domain.Users;
using MainServer.Domain.Auth.Roles;

namespace MainServer.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProgramEntity> Programs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProgramEncryptionKey> ProgramEncryptionKeys { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //ModelBuilderExtension.Seed(builder);
            base.OnModelCreating(builder);
        }
    }
}

