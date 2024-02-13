using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Events;

namespace ProgramServer.Persistence.Configurations
{
	public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            //builder.HasOne(e => e.Subject)
            //    .WithMany(s => s.Events)
            //    .HasForeignKey(e => e.SubjectId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

