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
using ServiceSignerBase.Data;
using Xunit;
using ServiceSignerBase.Exceptions;

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
        public async Task GenerateKeyAndSignAndVerify_OK_test()
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
        public async Task GenerateKeys_SignAndVerify_Model_OK_test()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");

            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();


            ServiceSigner signer = new ServiceSigner(ServiceSignerBase.Enums.SignAlgorithms.RsaSha256,privstring, pubstring);

            SomeModel model = new SomeModel()
            {
                Name = "farid",
                Surname = "Ismayilzada",
                TestData = "Test",
                
                InnerModel = new InnerModel()
                {
                    Year = 2022,
                    HidedObject = new ThirdObject { HidedName = "Secret" ,LongProp = 50000}
                }
            };

            var rs = signer.SignDataModel(model);

            var text = JsonSerializer.Serialize(rs);

            var decer = JsonSerializer.Deserialize<SrvSignedContainer<SomeModel>>(text);




            decer.ValidateSignature(pubstring);

            signer.ValidateSignatureContainer(decer, pubstring);

        }

        [Fact]
        public async Task GenerateKeys_SignAndVerify_Model_Break_test()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");

            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();


            ServiceSigner signer = new ServiceSigner(ServiceSignerBase.Enums.SignAlgorithms.RsaSha256, privstring, pubstring);

            SomeModel model = new SomeModel()
            {
                Name = "farid",
                Surname = "Ismayilzada",
                TestData = "Test",

                InnerModel = new InnerModel()
                {
                    Year = 2022,
                    HidedObject = new ThirdObject { HidedName = "Secret", LongProp = 50000 }
                }
            };

            var rs = signer.SignDataModel(model);

            var text = JsonSerializer.Serialize(rs);

            var decer = JsonSerializer.Deserialize<SrvSignedContainer<SomeModel>>(text);


            decer.Payload.DateTime = DateTime.Today;
            decer.Payload.InnerModel.Year = 2000;




            Assert.Throws(typeof(SrvInvalidSignatureException),
                () => { decer.ValidateSignature(pubstring); });
        }
       
        [Fact]
        public async Task GenerateKeys_SignAndVerifyInt_test()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();

            ServiceSigner srvsigner = new ServiceSigner(ServiceSignerBase.Enums.SignAlgorithms.RsaSha256, privstring, pubstring);

            int tobesigned = 0;

            var result = srvsigner.SignData(tobesigned);

            var text = JsonSerializer.Serialize(result);


            result.ValidateSignature(pubstring);

            result.Payload = 5;
          //  result.ValidateSignature(privstring);
        }


        [Fact]
        public async Task GenerateKeys_SignAndVerifyInt_breakTest()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();

            ServiceSigner srvsigner = new ServiceSigner(ServiceSignerBase.Enums.SignAlgorithms.RsaSha256, privstring, pubstring);

            int tobesigned = 0;

            var result = srvsigner.SignData(tobesigned);

            var text = JsonSerializer.Serialize(result);


           

            result.Payload = 5;
            Assert.Throws(typeof(SrvInvalidSignatureException),
              () => { result.ValidateSignature(pubstring); });
           
        }

        [Fact]
        public async Task GenerateKeys_SignAndVerifyDateTime_breakTest()
        {
            string signalg = "SHA-256withRSA";
            var keypair = Util.GetKeyPairProvider("rsa").GenerateServiceKeyPair(2048);

            var servicesigner = Util.GetSigner("rsa");


            var privstring = keypair.Private.SerializePrivateKeyToBase58();
            var pubstring = keypair.Public.SerializePublicKeyToBase58();

            ServiceSigner srvsigner = new ServiceSigner(ServiceSignerBase.Enums.SignAlgorithms.RsaSha256, privstring, pubstring);

            DateTime tobesigned = DateTime.Now;

            var result = srvsigner.SignData(tobesigned);

            var text = JsonSerializer.Serialize(result);




            result.Payload = DateTime.Now;
            Assert.Throws(typeof(SrvInvalidSignatureException),
              () => { result.ValidateSignature(pubstring); });

        }


    }
}
