using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Persistence.Configurations
{
	public class SubjectUserConfiguration : IEntityTypeConfiguration<SubjectUser>
    {
        public void Configure(EntityTypeBuilder<SubjectUser> builder)
        {
            builder.HasKey(o=>o.Id);
            builder.HasIndex(su => new { su.SubjectId, su.UserId });

            builder.HasOne(su => su.Subject)
                .WithMany(s => s.SubjectUsers)
                .HasForeignKey(su => su.SubjectId);
        }
    }
}

