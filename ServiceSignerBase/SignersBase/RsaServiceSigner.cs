using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using ServiceSignerBase.BaseEncoding;
using ServiceSignerBase.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Signers
{
    public class RsaServiceSigner : IKeyGenerator, IServiceSigner
    {
        public AsymmetricCipherKeyPair GenerateServiceKeyPair( int keylenght)
        {
           

            var rsaKeyPairGen = new RsaKeyPairGenerator();
            rsaKeyPairGen.Init(new KeyGenerationParameters(new SecureRandom(), keylenght));
            return rsaKeyPairGen.GenerateKeyPair();
        }

        //TODO: Should change when multiple algorithm supported 
        public string SignData(string base64data , string privateKey , string digestname= "SHA-256withRSA")
        {
            return Sign(base64data.FromBase64String() , privateKey.ToPRivateKey(), digestname).ToBase64String();
        }

        public byte[] SignBytes(byte[] datatoSign, string privatekey, string digestname = "SHA-256withRSA")
        {
            return Sign(datatoSign, privatekey.DeserializePrivateKeyFromBase58(), digestname); 
        }

        public void VerifySignature(string base58data , string base58signature , string publicKeyString , string digestname= "SHA-256withRSA")
        {
            if(! Verify(Base58.Decode(base58data) ,Base58.Decode(base58signature) ,
                publicKeyString.DeserializePublicKeyFromBase58(), digestname))
            {
                throw new SrvInvalidSignatureException($" Invalid Service Signature , DigestAlg :{digestname}"); 
            }
        }
        public void VerifySignature(byte[] data, byte[] signature, string publicKeyString, string digestname = "SHA-256withRSA")
        {
            if (!Verify(data, signature,
                    publicKeyString.DeserializePublicKeyFromBase58(), digestname))
            {
                throw new SrvInvalidSignatureException($" Invalid Service Signature , DigestAlg :{digestname}");
            }
        }
        public void VerifySignature(byte[] data, string base58signature, string publicKeyString, string digestname = "SHA-256withRSA")
        {
            if (!Verify(data, Base58.Decode(base58signature),
                    publicKeyString.DeserializePublicKeyFromBase58(), digestname))
            {
                throw new SrvInvalidSignatureException($" Invalid Service Signature , DigestAlg :{digestname}");
            }
        }

        public byte[] Sign(byte[] data, AsymmetricKeyParameter privateKey, string digestname)
        {
            ISigner sign = SignerUtilities.GetSigner(digestname);
            sign.Init(true, privateKey);
            sign.BlockUpdate(data, 0, data.Length);

          
            return sign.GenerateSignature();
        }

      
        public bool Verify(byte[] data,byte[] signature , AsymmetricKeyParameter publicKey, string digestname)
        {
            ISigner signClientSide = SignerUtilities.GetSigner(digestname);
            signClientSide.Init(false, publicKey);
            signClientSide.BlockUpdate(data, 0, data.Length);
            

            return signClientSide.VerifySignature(signature);
        }
    }
}
