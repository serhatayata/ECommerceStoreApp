using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.LogData.ELK.Models
{
    public class LogParameter
    {
        /// <summary>
        /// Name of parameter
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of parameter
        /// </summary>
        public object? Value { get; set; }
        /// <summary>
        /// Type of parameter
        /// </summary>
        public string? Type { get; set; }
    }
}
