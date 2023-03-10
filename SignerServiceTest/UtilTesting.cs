using ServiceSignerBase.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace SignerServiceTest
{
    public class UtilTesting
    {

        //[Fact]
        [Theory]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(8)]
        [InlineData(100)]
        public async Task InttoByteConvertionTest( int aa )
        {
             byte[] expected =BitConverter.GetBytes(aa);

            byte[] actual = Helper.ObjectToByteArray(aa);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(DateTime.Now)]
        [InlineData(Convert.ToDateTime("2023-03-01"))]
        public async Task DateTimetoByteConvertionTest(DateTime dt )
        {
            byte[] expected = BitConverter.GetBytes(dt.Ticks); 

            byte[] actual = Helper.ObjectToByteArray(dt);
            Assert.Equal(expected, actual);
        }
    }
}
