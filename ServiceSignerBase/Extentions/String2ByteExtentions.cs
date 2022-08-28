using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase
{
    public static class StringByteExtentions
    {

        public static byte[] FromBase64String(this string source)
        {
            return Convert.FromBase64String(source);
        }

        public static string ToBase64String(this byte[] blob)
        {
            return Convert.ToBase64String(blob);
        }


        public static byte[] ToByteArray(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }


        public static AsymmetricKeyParameter ToPRivateKey(this string source)
        {
           // source = source.Trim("-----BEGIN RSA PRIVATE KEY-----".ToCharArray());
           // source=source.Trim("-----END RSA PRIVATE KEY-----".ToCharArray());

           
            using (var sr = new StringReader(source))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                
                var objnew= (AsymmetricCipherKeyPair)pemReader.ReadObject();



                return (RsaKeyParameters) objnew.Private;

      
               
            }
        }
        public static AsymmetricKeyParameter ToPublicKey(this string source)
        {
            // source = source.Trim("-----BEGIN RSA PRIVATE KEY-----".ToCharArray());
            // source=source.Trim("-----END RSA PRIVATE KEY-----".ToCharArray());


            using (var sr = new StringReader(source))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                
                var objnew = (RsaKeyParameters)pemReader.ReadObject();



                return objnew;



            }
        }

        public static string KeyParam2base64String(this AsymmetricKeyParameter key)
        {
            
            //  key.as
            return null;
        }

        public static AsymmetricKeyParameter StringKeyToKeyParam(this string keystring)
        {
           var keyparam = PrivateKeyFactory.CreateKey(keystring.FromBase64String());

            return keyparam;
        }

        public static string ToPembase64String(this AsymmetricKeyParameter key)
        {
            using (TextWriter textWriter = new StringWriter())
            {
                var pemWriter1 = new PemWriter(textWriter);
                pemWriter1.WriteObject(key);
                pemWriter1.Writer.Flush();

                string privateKey = textWriter.ToString();
                return privateKey;

            }
        }

    }
}
