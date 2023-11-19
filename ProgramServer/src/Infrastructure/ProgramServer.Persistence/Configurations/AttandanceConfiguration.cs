using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Attendances;

namespace ProgramServer.Persistence.Configurations
{
	public class AttandanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasOne(o=>o.User).WithMany().HasForeignKey(o=>o.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o=>o.Event).WithMany().HasForeignKey(o=>o.EventId);

        }
    }
}

