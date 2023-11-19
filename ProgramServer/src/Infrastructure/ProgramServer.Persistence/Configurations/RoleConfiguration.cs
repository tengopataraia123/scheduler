using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Roles;

namespace ProgramServer.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(o=>o.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(r => r.RoleName)
                .HasMaxLength(50)
                .HasAnnotation("RegularExpression", "[^0-9]");

            builder.HasMany(r => r.Users)
               .WithOne(u => u.Role)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

