using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Exceptions
{
    public class SrvInvalidSignatureException:Exception
    {
        public SrvInvalidSignatureException(string message ):base(message)
        {

        }
        public SrvInvalidSignatureException(string message ,Exception inner ):base(message,inner)
        {

        }

        public SrvInvalidSignatureException() 
        {

        }
    }
}
