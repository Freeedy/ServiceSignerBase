using Org.BouncyCastle.Crypto;
using ServiceSignerBase.Data;
using ServiceSignerBase.Extentions;
using ServiceSignerBase.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase
{
    public class ServiceSigner  
    {
        public string Algorithm { get { return Constants.SignatureAlgorithmRsaDefault; } }

        private RsaServiceSigner _signer;

        string _privateKey; 
        public ServiceSigner(string privatekey , string publickey=null )
        {
            _privateKey = privatekey;
            _signer = new RsaServiceSigner();   
        }



        //Tagged: Version 1
        public SrvSignedContainer<T> SignDataModel<T>(T MOdel)
        {
            SrvSignedContainer<T> container = new SrvSignedContainer<T>();
            if (MOdel == null) return null;
            container.Payload = MOdel;
            var atrts = AttributeHelper.GetPropertiesInfo(MOdel);
            if (atrts.Count == 0) return container;

            string tobesigned = "";
            string headerPattern = "";

            for (int i = 0; i < atrts.Count; i++)
            {
                tobesigned += atrts[i].Value.ToString();
                headerPattern += atrts[i].Route;
                if (i != atrts.Count - 1) headerPattern += "/"; 
            }

            string signature = _signer.SignData(tobesigned.ToByteArray().ToBase64String(), _privateKey,Algorithm);

            container.Header = new SignedDataHeader { Alg = Algorithm ,Pattern=headerPattern ,Signature=signature};
            return container;
            //Constants.SignatureAlgorithmRsaDefault



          //  return null;
        }

        public void ValidateSignatureContainer<T>(SrvSignedContainer<T> container, string publickey)
        {
            if (container.Header == null || string.IsNullOrWhiteSpace(container.Header.Signature)) throw new ArgumentNullException($"{nameof(container.Header.Signature)} is null or empity");

            string signeddata = "";

            string[] pattern = container.Header.Pattern.Split('/');

            foreach (string patt in pattern)
            {
                var  val = AttributeHelper.GetPropValue(container.Payload, patt).Value;
                signeddata += val.ToString();

            }
            _signer.VerifySignature(signeddata.ToByteArray().ToBase64String(), container.Header.Signature, publickey, Algorithm);
        }

    }
}
