using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utilities.Models
{
    public abstract class Paging
    {
        public virtual int Page { get; set; }
        public virtual int PageSize { get; set; }
    }
}
