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
    public class BluetoothLogsConfiguration : IEntityTypeConfiguration<BluetoothLog>
    {
        public void Configure(EntityTypeBuilder<BluetoothLog> builder)
        {
            builder.Property(o => o.ScanDate).HasColumnType("timestamp without time zone");
        }
    }
}
