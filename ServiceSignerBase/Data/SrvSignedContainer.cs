using ServiceSignerBase.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceSignerBase.Exceptions;

namespace ServiceSignerBase.Data
{
    public  class SrvSignedContainer<T> : ISignedContract<T>
    {

        public SrvSignedContainer(T payload , string pattern , string algorithm )
        {
            Payload = payload;
            Header = new SignedDataHeader { Alg = algorithm  ,Pattern=pattern };
        }
        public SrvSignedContainer(T payload , string pattern ) :this(payload ,pattern, Constants.SignatureAlgorithmRsaDefault)
        {

        }
        public SrvSignedContainer()
        {

        }

        public string Version { get; set; } = "1.0.1"; 
        public T Payload { get ;  set; }


        public SignedDataHeader? Header { get; set; }

        public  void ValidateSignature(string publicKey)
        {
            if (Header==null) throw new ArgumentNullException($"Header Is null ");

            if (string.IsNullOrWhiteSpace(Header.Signature))
            {
                throw new SrvInvalidSignatureException("Signature emptiy!");
            }


            string[] pattern = Header.Pattern.Split("/"); 

            //TODO: change string to bytearray 
            byte[] payload =new byte[] { };


            foreach (string item in pattern)
            {
                var propval = AttributeHelper.GetPropValue(Payload, item);

                if (propval == null) throw new SrvInvalidSignatureException($"{item} property is null!");
                var propbytes = Helper.ObjectToByteArray(propval.Value);
                payload = Helper.ConcatenateBytes(payload,propbytes); 
            }



        }
        
    }
}
