using Org.BouncyCastle.Asn1.X509.Qualified;
using ServiceSignerBase.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ServiceSignerBase.Extentions
{
    public static class Helper
    {

        public static byte[] ObjectToByteArray(Object obj)
        {
            switch (obj)
            {
                case string str:
                    return Encoding.UTF8.GetBytes(str);
                case DateTime dt:
                    var ticks = dt.Ticks;
                    return BitConverter.GetBytes(ticks);

                case byte b: return BitConverter.GetBytes(b);
                case int i: return BitConverter.GetBytes(i);
                case double d: return BitConverter.GetBytes(d);
                case short s: return BitConverter.GetBytes(s);
                case ushort u: return BitConverter.GetBytes(u);
                case ulong v: return BitConverter.GetBytes(v);
                case long l: return BitConverter.GetBytes(l);
                case float f: return BitConverter.GetBytes(f);
                case bool bb: return BitConverter.GetBytes(bb);
                
                default:
                    if (obj is IEncoding encod)
                    {
                        return encod.EncodetoBytes();
                    }
                    throw new  NotSupportedException($"{obj.GetType().Name} is not supported" );
            }
          
        }
    }
}
