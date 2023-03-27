using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceSignerBase.Enums;

namespace ServiceSignerBase.Extentions
{
    public static class Constants
    {
        public const string SignatureAlgorithmRsaDefault = "SHA-256withRSA";

        public static Dictionary<string, SignAlgorithms> SignAlgorithms = new Dictionary<string, SignAlgorithms>()
        {
            { "SHA-256withRSA", Enums.SignAlgorithms.RsaSha256 },
            { "SHA-384withRSA", Enums.SignAlgorithms.RsaSha256 },
            { "RSASHA384", Enums.SignAlgorithms.RsaSha256 },
            { "RSASHA256", Enums.SignAlgorithms.RsaSha256 },
            { "RSA", Enums.SignAlgorithms.RsaSha256 }
        };
    }
}
