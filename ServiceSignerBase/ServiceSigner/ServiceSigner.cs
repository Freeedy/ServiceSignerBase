using Org.BouncyCastle.Crypto;
using ServiceSignerBase.Data;
using ServiceSignerBase.Extentions;
using ServiceSignerBase.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.ServiceSigner
{
    public class ServiceSigner  
    {
        public string Algorithm { get { return "SHA-256withRSA"; } }

        private RsaServiceSigner _signer; 

        public ServiceSigner(string privatekey , string publickey )
        {
            _signer = new RsaServiceSigner();   
        }



        public SrvSignedContainer<T> SignDataModel<T>(T MOdel)
        {
            if (MOdel == null) return null;
            var atrts = AttributeHelper.GetPropertiesInfo(MOdel); 

            

            return null;
        }



    }
}
