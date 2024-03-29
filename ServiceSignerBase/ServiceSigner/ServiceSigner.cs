﻿using Org.BouncyCastle.Crypto;
using ServiceSignerBase.Data;
using ServiceSignerBase.Enums;
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
        string _publicKeyString = null;

        byte[] _privateKeyBytes;
        byte[] _publicKeyBytes = null;

        public ServiceSigner(SignAlgorithms algorithm, string privatekey, string publickey = null)
        {
            SetPrivateKey(privatekey);
            if (publickey != null) SetPublicKey(publickey);
          
            switch (algorithm)
            {
                case Enums.SignAlgorithms.RsaSha256:
                    _signer = new RsaServiceSigner();
                    break;

                default:
                    throw new NotSupportedException($"{algorithm} is not supported!");
            }
        }

        public ServiceSigner(SignAlgorithms algorithm, byte[] privatekey, byte[] publickey = null)
        {
           SetPrivateKey(privatekey);
            if (publickey!=null) SetPublicKey(publickey);

          
            switch (algorithm)
            {
                case Enums.SignAlgorithms.RsaSha256:
                    _signer = new RsaServiceSigner();
                    break;

                default:
                    throw new NotSupportedException($"{algorithm} is not supported!");
            }

        }


        public ServiceSigner(SignAlgorithms algorithm)
        {
            
            switch (algorithm)
            {
                case Enums.SignAlgorithms.RsaSha256:
                    _signer = new RsaServiceSigner();
                    break;

                default:
                    throw new NotSupportedException($"{algorithm} is not supported!");
            }
        }


        public void SetPrivateKey(string privateKey)
        { _privateKey = privateKey; }

        public void SetPrivateKey(byte[] privateKey)
        { _privateKeyBytes = privateKey; }

        public void SetPublicKey(string publickey)
        {
            _publicKeyString = publickey;
        }

        public void SetPublicKey(byte[] publicKey) { _publicKeyBytes = publicKey; }



        public SrvSignedContainer<T> SignData<T>(T data)
        {
            bool isclass = typeof(T).IsClass && typeof(T) != typeof(string);

            if (isclass)
            {
                return SignDataModel(data);
            }
            else
            {
                SrvSignedContainer<T> container = new SrvSignedContainer<T>();
                byte[] tobesigned = Helper.ObjectToByteArray(data);
                byte[] signature = _signer.SignBytes(tobesigned, _privateKey, Algorithm);

                container.Header = new SignedDataHeader { Alg = Algorithm, Pattern = null, Signature = signature.ToBase58String() };
                return container;
            }

        }
        //Tagged: Version 1
        //TODO: add type signing mechanism for container 
        //TODO: add type signed containers validation mechanism 
        public SrvSignedContainer<T> SignDataModel<T>(T MOdel)
        {
            SrvSignedContainer<T> container = new SrvSignedContainer<T>();


            if (MOdel == null) return null;
            container.Payload = MOdel;
            var atrts = AttributeHelper.GetPropertiesInfo(MOdel);
            if (atrts.Count == 0) return container;

            byte[] tobesigned = new byte[0];
            string headerPattern = "";

            for (int i = 0; i < atrts.Count; i++)
            {
                // tobesigned += atrts[i].Value.ToString();
                byte[] temp = Helper.ObjectToByteArray(atrts[i].Value);
                // tobesigned = tobesigned.Concat(temp).ToArray();
                tobesigned = Helper.ConcatenateBytes(tobesigned, temp);

                headerPattern += atrts[i].Route;
                if (i != atrts.Count - 1) headerPattern += "/";
            }

            byte[] signature = _signer.SignBytes(tobesigned, _privateKey, Algorithm);

            container.Header = new SignedDataHeader { Alg = Algorithm, Pattern = headerPattern, Signature = signature.ToBase58String() };
            return container;

        }



        public void ValidateSignatureContainer<T>(SrvSignedContainer<T> container, string publickey)
        {
            //if (container.Header == null || string.IsNullOrWhiteSpace(container.Header.Signature)) throw new ArgumentNullException($"{nameof(container.Header.Signature)} is null or empity");

            //byte[]  signeddata = new byte[0] ;

            //string[] pattern = container.Header.Pattern.Split('/');

            //foreach (string patt in pattern)
            //{
            //    var  val = AttributeHelper.GetPropValue(container.Payload, patt).Value;
            //    signeddata =signeddata.Concat( Helper.ObjectToByteArray(val)).ToArray();

            //    // signeddata += val.ToString();

            //}
            ////_signer.Verify(signeddata,container.Header.Signature ,publickey);

            //_signer.VerifySignature(signeddata.ToBase58String(), container.Header.Signature, publickey, Algorithm);

            container.ValidateSignature(publickey);
        }

    }
}
