using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Data
{
    public interface ISignedContract<T>
    {
        T Payload { get; set; }

        SignedDataHeader Header { get; set; }



        void ValidateSignature(string key );
    }
}
