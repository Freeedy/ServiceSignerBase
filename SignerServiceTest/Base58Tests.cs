using ServiceSignerBase.BaseEncoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignerServiceTest
{
    public class Base58Tests
    {

        [Fact]
        public async Task TestBase58Encoding()
        {
            string tobeenc = "test"; //3yZe7d
            string encoded = Base58.Encode(Encoding.UTF8.GetBytes(tobeenc));

            Assert.Equal("3yZe7d", encoded);
        }
    }
}
