using ServiceSignerBase;
using ServiceSignerBase.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignerServiceTest
{
    public class ServiceSignerTests
    {



        [Fact]
        public async Task GenerateKeystest()
        {
            var generator = Util.GetKeyPairProvider("rsa");

            var pair = generator.GenerateServiceKeyPair(2048);

            Assert.NotNull(pair);
        }

        [Fact]
        public async Task GenerateKeyAndSign_test()
        {
            var generator = Util.GetKeyPairProvider("rsa");

            var pair = generator.GenerateServiceKeyPair(2048);


            string nonce ="signeit".ToByteArray().ToBase64String();

            string privateKey= pair.Private.ToPembase64String();

            string signature = Util.GetSigner("rsa").SignData(nonce , privateKey ,Constants.SignatureAlgorithmRsaDefault);
            
            Assert.NotNull(signature);
        }

        [Fact]
        public async Task GenerateKeyAndSignAndVerify_test()
        {

        }
    }
}
