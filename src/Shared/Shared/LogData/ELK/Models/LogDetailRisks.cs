using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogData.ELK.Models
{
    public enum LogDetailRisks : byte
    {
        NotRisky = 1,
        Normal = 2,
        Critical = 3
    }
}
