using ProgramServer.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Domain.Attandances
{
    public class BluetoothLog : BaseEntity
    {
        public string? BluetoothCode { get; set; }
        public int ScannedById { get; set; }
        public DateTime ScanDate { get; set; }
    }
}
