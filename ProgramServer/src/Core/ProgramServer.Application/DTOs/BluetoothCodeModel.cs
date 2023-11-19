using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.DTOs
{
    public class BluetoothCodeModel
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public DateTime ActivateTime { get; set; }
    }
}
