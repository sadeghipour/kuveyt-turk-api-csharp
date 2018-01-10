using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuveytTurkAPICSharp.Types
{
    public class GenericResponse<T>
    {
        public virtual List<Result> Results { get; set; }
        public virtual bool Success { get; set; }
        public T Value { get; set; }
    }
}
