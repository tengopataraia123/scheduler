namespace ProgramServer.Application.DTOs
{
    public class AttendanceModel
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public IEnumerable<BluetoothCodeModel> BluetoothCodes { get; set; }
    }
}

