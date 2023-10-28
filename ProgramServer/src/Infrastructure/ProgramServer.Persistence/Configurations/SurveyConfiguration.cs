using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Persistence.Configurations
{
	public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}

