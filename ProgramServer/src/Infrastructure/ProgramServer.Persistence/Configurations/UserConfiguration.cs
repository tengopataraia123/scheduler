using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using ProgramServer.Domain.Users;

namespace ProgramServer.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .HasAnnotation("RegularExpression", "[^0-9]");

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .HasAnnotation("RegularExpression", "[^0-9]");

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.HasOne(u => u.Role)
                .WithMany(r=>r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

