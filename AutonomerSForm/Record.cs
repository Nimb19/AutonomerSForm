using System;

namespace AutonomerSForm
{
    public class Record
    {
        public const string TableName = "Records";

        public Guid Uid { get; set; }
        public DateTime Date { get; set; }
        public string CarNumber { get; set; }
        public byte[] Image { get; set; }

        public override string ToString()
        {
            return $"{Date} {CarNumber} ({Uid})";
        }
    }
}
