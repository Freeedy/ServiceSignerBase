using ServiceSignerBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignerServiceTest.Models
{

    public class SomeModel
    {

        [Signable]
        public string Name { get; set; }


        public string TestData { get; set; }

        [Signable]
        public string Surname { get; set; }

        [Signable]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public InnerModel InnerModel { get; set; }
    }

    //name+surname
    public class InnerModel
    {
        public string Id { get; set; } = "TestID";

        [Signable]
        public int Year { get; set; }

        public ThirdObject HidedObject { get; set; }


    }

    public class ThirdObject
    {
        [Signable]
        public string HidedName { get; set; }

        [Signable]
        public long LongProp  { get; set; }
        public InnerModel ModelSam { get; set; }
    }
}
