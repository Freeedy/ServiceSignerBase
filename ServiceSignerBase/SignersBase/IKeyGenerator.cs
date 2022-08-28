using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Signers
{
    public interface IKeyGenerator
    {
        AsymmetricCipherKeyPair GenerateServiceKeyPair( int keylenght );
    }
}
