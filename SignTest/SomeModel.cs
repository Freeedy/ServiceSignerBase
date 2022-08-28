using ServiceSignerBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignTest
{
    public class SomeModel
    {

        [Signable]
        public string Name { get; set; }


        public string TestData { get; set; }

        [Signable]
        public string Surname { get; set; }

       public  InnerModel InnerModel { get; set; }
    }

    //name+surname
    public class InnerModel
    {
        public string Id { get; set; } = "TestID";

        [Signable]
        public string Year { get; set; }

        public ThirdObject HidedObject { get; set; }

    }

    public class ThirdObject
    {
        [Signable]
        public string  HidedName { get; set; }
    }
}
