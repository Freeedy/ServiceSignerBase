using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Signers
{
    public interface IServiceSigner
    {
        byte[] Sign(byte[] data , AsymmetricKeyParameter privateKey , string digestOid);

        bool Verify(byte[] data , byte[] signature, AsymmetricKeyParameter publicKey , string digestname);

         string SignData(string base64data, string privateKey, string digestname);

       // string SignData(byte[] base64data, byte[] privateKey, byte[] digestname);

        void VerifySignature(string base64data, string base64signature, string publicKeyString, string digestname);

    }
}
