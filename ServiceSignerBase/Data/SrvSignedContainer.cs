using ServiceSignerBase.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceSignerBase.Enums;
using ServiceSignerBase.Exceptions;
using ServiceSignerBase.Signers;

namespace ServiceSignerBase.Data
{
    public class SrvSignedContainer<T> : ISignedContract<T>
    {

        public SrvSignedContainer(T payload, string pattern, string algorithm)
        {
            Payload = payload;
            Header = new SignedDataHeader { Alg = algorithm, Pattern = pattern };
        }
        public SrvSignedContainer(T payload, string pattern) : this(payload, pattern, Constants.SignatureAlgorithmRsaDefault)
        {

        }
        public SrvSignedContainer()
        {

        }

        public string Version { get; set; } = "1.0.1";
        public T Payload { get; set; }

        public string SignedModelType
        {
            get { return typeof(T).Name; }
        }


        public SignedDataHeader? Header { get; set; }

        public void ValidateSignature(string publicKey)
        {
            if (Header == null) throw new ArgumentNullException($"Header Is null ");

            if (string.IsNullOrWhiteSpace(Header.Signature))
            {
                throw new SrvInvalidSignatureException("Signature empty!");
            }
            if (string.IsNullOrWhiteSpace(Header.Pattern))
            {
                ValidatePrimitiveSignature(publicKey);
            }
            else
            {
                ValidateModelSignature(publicKey);
            }




        }


        private void ValidateModelSignature(string publicKey)
        {

            if (!(typeof(T).IsClass && typeof(T) != typeof(string)))
            {
                throw new NotSupportedException($"{typeof(T)} not supported because of Pattern");
            }

            string[] pattern = Header.Pattern.Split("/");

            //TODO: change string to bytearray 
            byte[] payload = new byte[] { };


            foreach (string item in pattern)
            {
                var propval = AttributeHelper.GetPropValue(Payload, item);

                if (propval == null) throw new SrvInvalidSignatureException($"{item} property is null!");
                var propbytes = Helper.ObjectToByteArray(propval.Value);
                payload = Helper.ConcatenateBytes(payload, propbytes);
            }

            IServiceSigner signer;
            var algorithm = Util.GetSignAlgorithm(Header.Alg);
            switch (algorithm)
            {
                case SignAlgorithms.RsaSha256:
                    signer = new RsaServiceSigner();
                    break;

                default:
                    throw new NotSupportedException($" {Header.Alg} not supported ");
            }


            //TODO  : Fix Algorithm name problems 

            signer.VerifySignature(payload, Header.Signature, publicKey, Header.Alg);


        }

        private void ValidatePrimitiveSignature(string publicKey)
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string))
            {
                throw new NotSupportedException($"{typeof(T)} not supported because of Pattern");

            }
            byte[] data = Helper.ObjectToByteArray(Payload);

            IServiceSigner signer;
            var algorithm = Util.GetSignAlgorithm(Header.Alg);
            switch (algorithm)
            {
                case SignAlgorithms.RsaSha256:
                    signer = new RsaServiceSigner();
                    break;

                default:
                    throw new NotSupportedException($" {Header.Alg} not supported ");
            }



            signer.VerifySignature(data, Header.Signature, publicKey, Header.Alg);
        }
    }
}
