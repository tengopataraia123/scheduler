using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.Roles;
using ProgramServer.Domain.Subjects;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.SubjectUsers;
using ProgramServer.Domain.Surveys;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Attandances;

namespace ProgramServer.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SubjectUser> SubjectUsers { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<BluetoothCode> BluetoothCodes { get; set; }
        public DbSet<BluetoothLog> BluetoothLogs { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //ModelBuilderExtension.Seed(builder);
            base.OnModelCreating(builder);
        }
    }
}

