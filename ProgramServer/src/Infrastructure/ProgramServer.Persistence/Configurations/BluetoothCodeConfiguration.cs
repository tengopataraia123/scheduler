using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProgramServer.Domain.Attandances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Persistence.Configurations
{
    public class BluetoothCodeConfiguration : IEntityTypeConfiguration<BluetoothCode>
    {
        public void Configure(EntityTypeBuilder<BluetoothCode> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Code).HasMaxLength(5);

            builder.Property(o => o.ActivationTime).HasColumnType("timestamp without time zone");

            builder.HasOne(o => o.Attendance).WithMany().HasForeignKey(o => o.AttendanceId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
