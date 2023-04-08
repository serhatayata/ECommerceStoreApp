using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogData.ELK.Models
{
    public class LogDetail
    {
        public string MethodName { get; set; }
        public string Explanation { get; set; }
        public byte Risk { get; set; } = (byte)LogDetailRisks.Normal;
        public List<LogParameter> LogParameters { get; set; } = new List<LogParameter>();
        public string LoggingTime { get; set; } = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
    }
}
