using ServiceSignerBase.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
        
    }
}
