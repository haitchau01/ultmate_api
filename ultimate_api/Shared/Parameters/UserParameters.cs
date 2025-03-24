using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Parameters
{
    public class UserParameters : RequestParameters
    {
        public UserParameters() => OrderBy = "firstname";
        public uint MinAge { get; set; }
        public uint MaxAge { get; set; } = int.MaxValue;
        public bool ValidAgeRange => MaxAge > MinAge;

        public string? SearchTerm { get; set; }
    }
}
