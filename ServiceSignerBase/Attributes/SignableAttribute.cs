using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSignerBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Method|AttributeTargets.Enum |
        AttributeTargets.Field| AttributeTargets.Struct)]
    public class SignableAttribute:Attribute
    {
        public SignableAttribute():base()
        {

        }
    }
}
