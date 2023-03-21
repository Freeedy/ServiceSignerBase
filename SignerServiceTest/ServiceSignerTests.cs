using ServiceSignerBase;
using ServiceSignerBase.BaseEncoding;
using ServiceSignerBase.Extentions;
using SignerServiceTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public async Task GenerateKeyAndSignbase58_test()
        {
            var generator = Util.GetKeyPairProvider("rsa");

            var pair = generator.GenerateServiceKeyPair(2048);


            string nonce = "signeit".ToByteArray().ToBase64String();

            string privateKey = pair.Private.SerializePrivateKeyToBase58();

            string publickey = pair.Public.SerializePrivateKeyToBase58(); 
            string signature = Util.GetSigner("rsa").SignData(nonce, privateKey, Constants.SignatureAlgorithmRsaDefault);

            Assert.NotNull(signature);
        }

        [Fact]
        public async Task GenerateKeyAndSignAndVerify_test()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();


            var stringsignature = Base58.Encode( servicesigner.SignBytes("test".ToByteArray(), privstring, signalg));


            Console.WriteLine(stringsignature);

            servicesigner.VerifySignature(Base58.Encode("test11".ToByteArray()), stringsignature, pubstring, signalg);

            Console.WriteLine("OK");

        }

        [Fact] 
        public async Task GenerateKeys_SignAndVerify_Model_test()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();


            ServiceSigner signer = new ServiceSigner(privstring, pubstring);

            SomeModel model = new SomeModel()
            {
                Name = "farid",
                Surname = "Ismayilzada",
                TestData = "Test",
                InnerModel = new InnerModel()
                {
                    Year = "2022",
                    HidedObject = new ThirdObject { HidedName = "Secret" }
                }
            };

            var rs = signer.SignDataModel(model);

            var text = JsonSerializer.Serialize(rs);

            signer.ValidateSignatureContainer(rs, pubstring);
        }
    }
}
