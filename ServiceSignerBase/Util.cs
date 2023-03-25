using ServiceSignerBase.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceSignerBase.Enums;
using ServiceSignerBase.Extentions;

namespace ServiceSignerBase
{
    public static class Util
    {
       

        public static IServiceSigner GetSigner (string name )
        {
            return new RsaServiceSigner(); 
        }

        public static IKeyGenerator GetKeyPairProvider(string name)
        {
            return new RsaServiceSigner();
        }

        public static  SignAlgorithms? GetSignAlgorithm(string name)
        {
            if (Constants.SignAlgorithms.ContainsKey(name))
            {
                return Constants.SignAlgorithms[name];
            }
            return null;
        }


       
    }
}
