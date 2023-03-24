using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using ServiceSignerBase.BaseEncoding;
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

        public static string ToBase58String(this byte[] blob)
        {
            return Base58.Encode(blob);
        }

        public static byte[] FromBase58String(string source)
        {
            return Base58.Decode(source);
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

                var objnew = (AsymmetricCipherKeyPair)pemReader.ReadObject();



                return (RsaKeyParameters)objnew.Private;



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

        public static byte[] PrivateKeyParam2Bytes(this AsymmetricKeyParameter key)
        {

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(key);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();

            //  key.as
            return serializedPrivateBytes;
        }

        public static byte[] PublicKeyParam2Bytes(this AsymmetricKeyParameter key)
        {

            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();

            //  key.as
            return serializedPublicBytes;
        }




        public static AsymmetricKeyParameter PublicKeyFromBytes(this byte[] keybytes)
        {
            var keyparam = PublicKeyFactory.CreateKey(keybytes);

            return keyparam;
        }


        public static AsymmetricKeyParameter PrivateKeyFromBytes(this byte[] keybytes)
        {
            var keyparam = PrivateKeyFactory.CreateKey(keybytes);

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

        public static string SerializePrivateKeyToBase58(this AsymmetricKeyParameter key)
        {

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(key);
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            string serializedPrivate = Base58.Encode(serializedPrivateBytes);
            return serializedPrivate;
        }


        public static string SerializePublicKeyToBase58(this AsymmetricKeyParameter key)
        {
            SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(key);
            byte[] serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            return Base58.Encode(serializedPublicBytes);
        }

        public static AsymmetricKeyParameter DeserializePrivateKeyFromBase58(this string privatebase58)
        {
          return   PrivateKeyFactory.CreateKey(Base58.Decode(privatebase58));
        }
       

        public static AsymmetricKeyParameter DeserializePublicKeyFromBase58(this string publicbase58)
        {
            return PublicKeyFactory.CreateKey(Base58.Decode(publicbase58));
        }
    }
}
