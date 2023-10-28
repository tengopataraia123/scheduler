using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Persistence.Configurations
{
	public class ResponseConfiguration : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Choice)
                   .HasConversion<int>();

            builder.HasOne(r => r.Survey)
                   .WithMany()
                   .HasForeignKey(r => r.SurveyId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

