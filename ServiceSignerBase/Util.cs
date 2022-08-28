using ServiceSignerBase.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase
{
    public static class Util
    {
        public static IServiceSigner GetSigner (string name )
        {
            return new RsaServiceSigner(); 
        }

        public static IKeyGenerator GetKeyPairProvider(string name)
        {
            return new RsaServiceSigner();
        }
    }
}
