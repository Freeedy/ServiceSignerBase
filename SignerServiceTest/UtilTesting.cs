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



        [Fact]
        public async Task DateTimetoByteConvertionTest( )
        {
            var date = DateTime.Now;
            byte[] expected = BitConverter.GetBytes(date.Ticks); 

            byte[] actual = Helper.ObjectToByteArray(date);
            Assert.Equal(expected, actual);
        }
    }
}
