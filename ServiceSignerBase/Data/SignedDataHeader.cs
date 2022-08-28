using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Data
{
    public class SignedDataHeader
    {
        // Name/Surname/Organization.name
        public string Pattern { get; set; }

        public string Alg { get; set; }

        public string  Signature { get; set; }

    }
}
